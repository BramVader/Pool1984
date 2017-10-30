using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;

namespace Pool1984
{
    class Ball : Primitive
    {
        public static double TextureAngle = 0.55;

        public Func<double, Vector3> GetCenter = t => new Vector3();
        public Func<Vector3, double, Vector3> GetWorldToTextureTransformation = (v, t) => v;
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
            ParameterExpression vpar = Expression.Parameter(typeof(Vector3), "v");
            ParameterExpression tpar = Expression.Parameter(typeof(double), "t");

            Expression centerExpr = null;
            Expression textureToWorldTransformationExpr = null;
            Expression worldToTextureTransformationExpr = null;
            foreach (var keyframe in frames.Where(it => it.StartPosition.Ball == this).OrderByDescending(it => it.StartTime))
            {
                Expression timeCheck =
                    Expression.AndAlso(
                        Expression.GreaterThanOrEqual(tpar, Expression.Constant(keyframe.StartTime)),
                        Expression.LessThan(tpar, Expression.Constant(keyframe.EndTime))
                    );

                // Interpolating matrices over time
                if (keyframe.StartPosition.TextureToWorld.Valid && keyframe.EndPosition.TextureToWorld.Valid)
                {
                    textureToWorldTransformationExpr = Expression.Condition(
                        timeCheck,
                        BuildMatrixInterpolation(vpar, tpar, keyframe.StartTime, keyframe.EndTime,
                            keyframe.StartPosition.TextureToWorld,
                            keyframe.EndPosition.TextureToWorld
                        ),
                        textureToWorldTransformationExpr ?? Expression.Constant(default(Vector3))
                    );
                }

                if (keyframe.StartPosition.WorldToTexture.Valid && keyframe.EndPosition.WorldToTexture.Valid)
                {
                    worldToTextureTransformationExpr = Expression.Condition(
                        timeCheck,
                        BuildMatrixInterpolation(vpar, tpar, keyframe.StartTime, keyframe.EndTime,
                            keyframe.StartPosition.WorldToTexture,
                            keyframe.EndPosition.WorldToTexture
                        ),
                        worldToTextureTransformationExpr ?? Expression.Constant(default(Vector3))
                    );
                }

                // Interpolating position.Center over time
                centerExpr = Expression.Condition(
                    timeCheck,
                    BuildInterpolationExpression(tpar, keyframe.StartTime, keyframe.EndTime,
                        keyframe.StartPosition.Center,
                        keyframe.EndPosition.Center
                    ),
                    centerExpr ?? Expression.Constant(default(Vector3))
                );

            }
            GetCenter = Expression.Lambda<Func<double, Vector3>>(centerExpr, tpar).Compile();
            GetTextureToWorldTransformation =
                textureToWorldTransformationExpr != null ?
                Expression.Lambda<Func<Vector3, double, Vector3>>(textureToWorldTransformationExpr, vpar, tpar).Compile() :
                (v, t) => v;
            GetWorldToTextureTransformation =
                worldToTextureTransformationExpr != null ?
                Expression.Lambda<Func<Vector3, double, Vector3>>(worldToTextureTransformationExpr, vpar, tpar).Compile() :
                (v, t) => v;
        }

        public Ball()
        {
            Radius = 1.0;
        }

        public Intersection GetClosestIntersection(Ray ray, IntersectionMode mode, BallPosition position, double minDist = Intersection.MinDistance, double maxDist = Intersection.MaxDistance)
        {
            return GetClosestIntersection(ray, mode, position.Center, minDist, maxDist);
        }

        public override Intersection GetClosestIntersection(Ray ray, IntersectionMode mode, double time, double minDist = Intersection.MinDistance, double maxDist = Intersection.MaxDistance)
        {
            return GetClosestIntersection(ray, mode, GetCenter(time), minDist, maxDist);
        }

        private Intersection GetClosestIntersection(Ray ray, IntersectionMode mode, Vector3 center, double minDist = Intersection.MinDistance, double maxDist = Intersection.MaxDistance)
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
