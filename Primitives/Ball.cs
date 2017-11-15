using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Pool1984
{
    [DebuggerDisplay("{Name}")]
    class Ball : Primitive
    {
        public static double TextureAngle = 0.55;

        public Bitmap SphereMap { get; set; }
        public Number Number { get; set; }

        public Expression<Func<double, Vector3>> GetCenterExpr = t => new Vector3();
        public Func<double, Vector3> GetCenter = t => new Vector3();

        public Expression<Func<Vector3, double, Vector3>> GetWorldToTextureTransformationExpr = (v, t) => v;
        public Func<Vector3, double, Vector3> GetWorldToTextureTransformation = (v, t) => v;

        public Expression<Func<Vector3, double, Vector3>> GetTextureToWorldTransformationExpr = (v, t) => v;
        public Func<Vector3, double, Vector3> GetTextureToWorldTransformation = (v, t) => v;

        public double Radius { get; set; }
        public Color3 BandColor { get; set; }

        public Expression BuildMatrixInterpolation(ParameterExpression vpar, ParameterExpression tpar, double startTime, double endTime, Matrix4 a, Matrix4 b)
        {
            Func<int, Expression> getElement = n =>
                n == 0 ? Expression.Property(vpar, "X") :
                n == 1 ? Expression.Property(vpar, "Y") :
                n == 2 ? Expression.Property(vpar, "Z") :
                (Expression)null;

            Expression[] args = new Expression[3];
            for (int n = 0; n < 3; n++)
            {
                Expression expr1 = null;
                for (int m = 0; m < 4; m++)
                {
                    Expression expr2 = getElement(m);
                    if (a[m, n] == b[m, n])         // No interpolation needed
                    {
                        if (a[m, n] == 0.0)         // Multiply by zero: skip operation
                        {
                            expr2 = null;
                        }
                        else if (a[m, n] != 1.0)    // Non-zero coefficient
                        {
                            if (expr2 != null)
                            {
                                expr2 = Expression.Multiply(expr2, Expression.Constant(a[m, n]));
                            }
                            else
                            {
                                expr2 = Expression.Constant(a[m, n]);
                            }
                        }
                    }
                    else  // Interpolation required
                    {
                        Expression expr3 = BuildInterpolationExpression(tpar, startTime, endTime, a[m, n], b[m, n]);
                        if (expr2 != null)
                        {
                            expr2 = Expression.Multiply(expr2, expr3);
                        }
                        else
                        {
                            expr2 = expr3;
                        }
                    }

                    if (expr2 != null)
                    {
                        expr1 = expr1 == null ? expr2 : Expression.Add(expr1, expr2);
                    }

                    args[n] = expr1 ?? Expression.Constant(0.0);
                }
            }

            return Expression.New(
                typeof(Vector3).GetConstructors().Single(it => it.GetParameters().Length == 3),  // Get the Vector3 constructor with 3 parameters
                args
            );
        }


        private class ReplaceParameterExpressionsVisitor : ExpressionVisitor
        {
            private Dictionary<ParameterExpression, ParameterExpression> map;

            public ReplaceParameterExpressionsVisitor(IEnumerable<ParameterExpression> oldParms, IEnumerable<ParameterExpression> newParms)
            {
                this.map = oldParms.Zip(newParms, (a, b) => new { a, b }).ToDictionary(it => it.a, it => it.b);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (map.TryGetValue(node, out var newParm))
                    return newParm;
                return node;
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.Expression is ConstantExpression cexpr)
                {
                    var del = Expression.Lambda(node).Compile();
                    return Expression.Constant(del.DynamicInvoke());
                }
                return base.VisitMember(node);
            }

            protected override Expression VisitBinary(BinaryExpression node)
            {
                ConstantExpression cexpr;
                switch (node.NodeType)
                {
                    case ExpressionType.Subtract:
                        cexpr = Visit(node.Right) as ConstantExpression;
                        if (cexpr != null && (double)cexpr.Value == 0.0)
                            return Visit(node.Left);
                        break;
                    case ExpressionType.Divide:
                        cexpr = Visit(node.Right) as ConstantExpression;
                        if (cexpr != null && (double)cexpr.Value == 1.0)
                            return Visit(node.Left);
                        break;
                }
                return base.VisitBinary(node);
            }
        }

        public Expression BuildRotationInterpolation(ParameterExpression vpar, ParameterExpression tpar, double startTime, double endTime, Matrix4 orientation, Vector3 axisOfRotation, double angle, bool reverse)
        {
            Expression<Func<Vector3, double, Vector3>> lambdaExpr;
            double dTime = endTime - startTime;
            if (!reverse)
                if (angle != 0.0)
                    lambdaExpr = (v, t) => v * orientation * Matrix4.RotateAxisXY(axisOfRotation, -angle * (t - startTime) / dTime);
                else
                    lambdaExpr = (v, t) => v * orientation;
            else
                if (angle != 0.0)
                    lambdaExpr = (v, t) => v * Matrix4.RotateAxisXY(axisOfRotation, angle * (t - startTime) / dTime) * orientation;
                else
                    lambdaExpr = (v, t) => v * orientation;
            var expr = new ReplaceParameterExpressionsVisitor(lambdaExpr.Parameters, new ParameterExpression[] { vpar, tpar }).Visit(lambdaExpr.Body);
             return expr;
        }

        private Expression BuildInterpolationExpression<T>(ParameterExpression tpar, double startTime, double endTime, T startValue, T endValue)
        {
            if (startValue.Equals(endValue))
            {
                return Expression.Constant(startValue);
            }

            // (time - startTime) * (endValue - startValue)
            Expression exprA =
                Expression.Multiply(
                    startTime > 0.0 ?
                        Expression.Subtract(tpar, Expression.Constant(startTime)) :
                        (Expression)tpar,
                    Expression.Constant((T)((dynamic)endValue - (dynamic)startValue))
                );

            // startValue + (time - startTime) * (endValue - startValue) / (endTime - startTime)
            return
                Expression.Add(
                        Expression.Constant(startValue),
                        endTime - startTime < 1.0 ?
                            Expression.Divide(
                                exprA,
                                Expression.Constant(endTime - startTime)
                            ) :
                            exprA
                    );
        }

        public void ApplyKeyframes(IEnumerable<Keyframe> frames)
        {
            var keyframes = frames.Where(it => it.StartPosition.Ball == this).OrderBy(it => it.StartTime).ToList();

            // Insert missing keyframes for a full 0..1 coverage
            double tLast = 0.0;
            BallPosition pLast = null;
            int i = 0;
            while (i < keyframes.Count)
            {
                var keyframe = keyframes[i];
                if (keyframe.StartTime > tLast)
                {
                    keyframes.Insert(i, new Keyframe { StartTime = tLast, EndTime = keyframe.StartTime, StartPosition = keyframe.StartPosition, EndPosition = keyframe.StartPosition });
                    i++;
                }
                tLast = keyframe.EndTime;
                pLast = keyframe.EndPosition;
                i++;
            }
            if (tLast < 1.0)
            {
                keyframes.Add(new Keyframe { StartTime = tLast, EndTime = 1.0, StartPosition = pLast, EndPosition = pLast });
            }

            // Calculate axis of rotation and angles
            Vector3 up = new Vector3(0.0, 0.0, 1.0);
            bool hasTextureTransformation = false;
            foreach (var keyframe in keyframes)
            {
                keyframe.Direction = keyframe.EndPosition.Center - keyframe.StartPosition.Center;
                double distance = keyframe.Direction.Length;

                if (distance > 0.0)
                {
                    keyframe.AxisOfRotation = Vector3.Cross(keyframe.Direction, up).Normalize();
                    keyframe.Angle = distance / Radius;
                }
                else
                {
                    keyframe.Angle = 0.0;   // Ball not moving during keyframe
                }

                hasTextureTransformation |= keyframe.StartPosition.TextureToWorld.Valid || keyframe.EndPosition.TextureToWorld.Valid;
            }

            // Calculate starting matrix of each keyframe
            if (hasTextureTransformation)
            {
                bool recalc;
                do
                {
                    recalc = false;
                    foreach (var keyframe in keyframes)
                    {
                        recalc |= !keyframe.StartPosition.TextureToWorld.Valid || !keyframe.EndPosition.TextureToWorld.Valid;

                        if (keyframe.StartPosition.TextureToWorld.Valid && !keyframe.EndPosition.TextureToWorld.Valid)
                        {
                            keyframe.EndPosition.TextureToWorld = keyframe.StartPosition.TextureToWorld * Matrix4.RotateAxisXY(keyframe.AxisOfRotation, -keyframe.Angle);
                            keyframe.EndPosition.WorldToTexture = Matrix4.RotateAxisXY(keyframe.AxisOfRotation, keyframe.Angle) * keyframe.StartPosition.WorldToTexture;
                        }
                        else if (!keyframe.StartPosition.TextureToWorld.Valid && keyframe.EndPosition.TextureToWorld.Valid)
                        {
                            keyframe.StartPosition.TextureToWorld = keyframe.EndPosition.TextureToWorld * Matrix4.RotateAxisXY(keyframe.AxisOfRotation, keyframe.Angle);
                            keyframe.StartPosition.WorldToTexture = Matrix4.RotateAxisXY(keyframe.AxisOfRotation, -keyframe.Angle) * keyframe.EndPosition.WorldToTexture;
                        }
                    }
                } while (recalc);
            }

            keyframes.Reverse();
            ParameterExpression vpar = Expression.Parameter(typeof(Vector3), "v");
            ParameterExpression tpar = Expression.Parameter(typeof(double), "t");
            Expression centerExpr = null;
            Expression textureToWorldTransformationExpr = null;
            Expression worldToTextureTransformationExpr = null;
            Expression expr;
            foreach (var keyframe in keyframes)
            {
                Expression timeCheck = keyframe.EndTime < 1.0 ?
                    Expression.LessThan(tpar, Expression.Constant(keyframe.EndTime)) :
                    (Expression)null;

                // Interpolating matrices over time
                if (keyframe.StartPosition.TextureToWorld.Valid && keyframe.EndPosition.TextureToWorld.Valid)
                {
                    expr = BuildRotationInterpolation(vpar, tpar, keyframe.StartTime, keyframe.EndTime,
                        orientation: keyframe.StartPosition.TextureToWorld,
                        axisOfRotation: keyframe.AxisOfRotation,
                        angle: keyframe.Angle,
                        reverse: false);
                    textureToWorldTransformationExpr =
                        timeCheck == null || textureToWorldTransformationExpr == null ?
                        expr :
                        Expression.Condition(
                            timeCheck,
                            expr,
                            textureToWorldTransformationExpr
                        );
                }

                if (keyframe.StartPosition.WorldToTexture.Valid && keyframe.EndPosition.WorldToTexture.Valid)
                {
                    expr = BuildRotationInterpolation(vpar, tpar, keyframe.StartTime, keyframe.EndTime,
                        keyframe.StartPosition.WorldToTexture,
                        axisOfRotation: keyframe.AxisOfRotation,
                        angle: keyframe.Angle,
                        reverse: true);
                    worldToTextureTransformationExpr =
                        timeCheck == null || worldToTextureTransformationExpr == null ?
                        expr :
                        Expression.Condition(
                            timeCheck,
                            expr,
                            worldToTextureTransformationExpr
                        );
                }

                // Interpolating position.Center over time
                expr = BuildInterpolationExpression(tpar, keyframe.StartTime, keyframe.EndTime,
                    keyframe.StartPosition.Center,
                    keyframe.EndPosition.Center
                );

                centerExpr =
                    timeCheck == null || centerExpr == null ?
                    expr :
                    Expression.Condition(
                        timeCheck,
                        expr,
                        centerExpr
                    );
            }
            GetCenterExpr = Expression.Lambda<Func<double, Vector3>>(centerExpr, tpar);
            GetCenter = GetCenterExpr.Compile();

            GetTextureToWorldTransformationExpr =
                textureToWorldTransformationExpr != null ?
                Expression.Lambda<Func<Vector3, double, Vector3>>(textureToWorldTransformationExpr, vpar, tpar) :
                (v, t) => v;
            GetWorldToTextureTransformationExpr =
                worldToTextureTransformationExpr != null ?
                Expression.Lambda<Func<Vector3, double, Vector3>>(worldToTextureTransformationExpr, vpar, tpar) :
                (v, t) => v;

            GetTextureToWorldTransformation = GetTextureToWorldTransformationExpr.Compile();
            GetWorldToTextureTransformation = GetWorldToTextureTransformationExpr.Compile();
        }

        public Ball()
        {
            Radius = 1.0;
        }

        public override Intersection GetClosestIntersection(Ray ray, IntersectionMode mode, double time, double minDist = Intersection.MinDistance, double maxDist = Intersection.MaxDistance)
        {
            return GetClosestIntersection(ray, mode, GetCenter(time), minDist, maxDist);
        }

        public Intersection GetClosestIntersection(Ray ray, IntersectionMode mode, Vector3 center, double minDist = Intersection.MinDistance, double maxDist = Intersection.MaxDistance)
        {
            Intersection intsec = new Intersection() { Entity = this };

            // Calculate intersection with ball
            Vector3 ct = ray.Origin - center;
            double b = 2.0 * Vector3.Dot(ray.Direction, ct);
            double c = Vector3.Dot(ct, ct) - Radius * Radius;
            double d = b * b - 4.0 * c;
            if (d >= 0.0)
            {
                intsec.Distance = (-b - Math.Sqrt(d)) * 0.5;
                intsec.Hit = intsec.Distance > minDist && intsec.Distance < maxDist;
                if (intsec.Hit && mode != IntersectionMode.Hit)
                {
                    // Calculate intersection
                    intsec.Position = ray.Origin + intsec.Distance * ray.Direction;
                    intsec.Normal = (intsec.Position - center) / Radius;
                }
            }
            return intsec;
        }

        public override Vector2 GetTextureCoordinates(Vector3 transformedNormal, double time)
        {
            double angle1 = Math.Atan2(transformedNormal.Y, transformedNormal.X) / TextureAngle;
            double angle2 = Math.Atan2(transformedNormal.Z, new Vector2(transformedNormal.X, transformedNormal.Y).Length) / TextureAngle;
            return new Vector2(
                angle1, -angle2
            );
        }
    }
}
