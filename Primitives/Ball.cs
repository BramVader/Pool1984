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
        public Func<double, Vector3> GetCenter = d => new Vector3();
        public Func<double, Vector3> GetTextureOrientation = d => new Vector3();
        public Func<double, double> GetMinAngle1 = d => 0.0;
        public Func<double, double> GetMaxAngle1 = d => 0.0;
        public Func<double, double> GetMinAngle2 = d => 0.0;
        public Func<double, double> GetMaxAngle2 = d => 0.0;

        public double Radius { get; set; }
        public Color3 BandColor { get; set; }

        public override Matrix4 GetWorldToTexture(double time)
        {
            return Matrix4.Rotate(GetTextureOrientation(time));
        }

        public override Matrix4 GetTextureToWorld(double time)
        {
            return Matrix4.RotateInv(GetTextureOrientation(time));
        }

        private Expression BuildInterpolationExpression<T>(ParameterExpression tpar, Expression prevExpression, double startTime, double endTime, T startValue, T endValue)
        {
            // (time - startTime) * (endVector - startVector)
            Expression exprA =
                Expression.Multiply(
                    startTime > 0.0 ?
                        Expression.Subtract(tpar, Expression.Constant(startTime)) :
                        (Expression)tpar,
                    Expression.Constant((T)((dynamic)endValue - (dynamic)startValue))
                );

            // startVector + (time - startTime) * (endVector - startVector) / (endTime - startTime)
            Expression exprB =
                Expression.Add(
                        Expression.Constant(startValue),
                        endTime - startTime < 1.0 ?
                            Expression.Divide(
                                exprA,
                                Expression.Constant(endTime - startTime)
                            ) :
                            exprA
                    );

            // time < endTime ? 
            //    startVector + (time - startTime) * (endVector - startVector) / (endTime - startTime) :
            //    endTime
            Expression exprC = endTime < 1.0 ?
                    Expression.Condition(
                        Expression.LessThan(tpar, Expression.Constant(endTime)),
                        exprB,
                        Expression.Constant(endValue)
                    ) :
                    exprB;

            // time < starTime ?
            //    {prevExpression} ?? startVector :
            //    time < endTime ? 
            //       startVector + (time - startTime) * (endVector - startVector) / (endTime - startTime) :
            //       endTime
            return
                startTime > 0.0 ?
                Expression.Condition(
                    Expression.LessThan(tpar, Expression.Constant(startTime)),
                    prevExpression ?? Expression.Constant(startValue),
                    exprC
                ) :
                exprC;
        }

        public void ApplyKeyframes(IEnumerable<Keyframe> frames)
        {
            ParameterExpression tpar = Expression.Parameter(typeof(double), "t");

            Expression centerExpr = null;
            Expression rotationExpr = null;
            Expression minAngle1Expr = null;
            Expression maxAngle1Expr = null;
            Expression minAngle2Expr = null;
            Expression maxAngle2Expr = null;
            foreach (var keyframe in frames.Where(it => it.StartPosition.Ball == this).OrderBy(it => it.StartTime))
            {
                // Interpolating position.Center over time
                centerExpr = BuildInterpolationExpression(tpar, centerExpr, keyframe.StartTime, keyframe.EndTime,
                    keyframe.StartPosition.Center, 
                    keyframe.EndPosition.Center
                );

                // Interpolating textureOrientation over time
                rotationExpr = BuildInterpolationExpression(tpar, rotationExpr, keyframe.StartTime, keyframe.EndTime, 
                    keyframe.StartPosition.TextureOrientation, 
                    keyframe.EndPosition.TextureOrientation
                );

                // Interpolating minAngle1 over time
                minAngle1Expr = BuildInterpolationExpression(tpar, minAngle1Expr, keyframe.StartTime, keyframe.EndTime,
                    keyframe.StartPosition.MinAngle1,
                    keyframe.EndPosition.MinAngle1
                );

                // Interpolating minAngle1 over time
                maxAngle1Expr = BuildInterpolationExpression(tpar, maxAngle1Expr, keyframe.StartTime, keyframe.EndTime,
                    keyframe.StartPosition.MaxAngle1,
                    keyframe.EndPosition.MaxAngle1
                );

                // Interpolating minAngle1 over time
                minAngle2Expr = BuildInterpolationExpression(tpar, minAngle2Expr, keyframe.StartTime, keyframe.EndTime,
                    keyframe.StartPosition.MinAngle2,
                    keyframe.EndPosition.MinAngle2
                );

                // Interpolating minAngle1 over time
                maxAngle2Expr = BuildInterpolationExpression(tpar, maxAngle2Expr, keyframe.StartTime, keyframe.EndTime,
                    keyframe.StartPosition.MaxAngle2,
                    keyframe.EndPosition.MaxAngle2
                );
            }
            GetCenter = Expression.Lambda<Func<double, Vector3>>(centerExpr, tpar).Compile();
            GetTextureOrientation = Expression.Lambda<Func<double, Vector3>>(rotationExpr, tpar).Compile();
            GetMinAngle1 = Expression.Lambda<Func<double, double>>(minAngle1Expr, tpar).Compile();
            GetMaxAngle1 = Expression.Lambda<Func<double, double>>(maxAngle1Expr, tpar).Compile();
            GetMinAngle2 = Expression.Lambda<Func<double, double>>(minAngle2Expr, tpar).Compile();
            GetMaxAngle2 = Expression.Lambda<Func<double, double>>(maxAngle2Expr, tpar).Compile();
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
            double angle1 = Math.Atan2(transformedNormal.Y, transformedNormal.X);
            double angle2 = Math.Atan2(transformedNormal.Z, new Vector2(transformedNormal.X, transformedNormal.Y).Length);
            return new Vector2(
                (angle1 - (GetMinAngle1(time) + GetMaxAngle1(time)) * 0.5) * 2.0 / (GetMaxAngle1(time) - GetMinAngle1(time)),
                ((GetMinAngle2(time) + GetMaxAngle2(time)) * 0.5 - angle2) * 2.0 / (GetMaxAngle2(time) - GetMinAngle2(time))
            );
        }
    }
}
