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

        public Func<double, Vector3> GetCenter = d => new Vector3();
        public Func<double, Vector3> GetTextureOrientation = d => new Vector3();

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
            double CorrectAngle (double a, double b)
            {
                while (b - a > Math.PI) b -= Math.PI * 2.0;
                while (b - a < -Math.PI) b += Math.PI * 2.0;
                return b;
            };

            ParameterExpression tpar = Expression.Parameter(typeof(double), "t");

            Expression centerExpr = null;
            Expression rotationExpr = null;
            foreach (var keyframe in frames.Where(it => it.StartPosition.Ball == this).OrderBy(it => it.StartTime))
            {
                // Interpolating position.Center over time
                centerExpr = BuildInterpolationExpression(tpar, centerExpr, keyframe.StartTime, keyframe.EndTime,
                    keyframe.StartPosition.Center, 
                    keyframe.EndPosition.Center
                );

                Vector3 endOrientation = new Vector3(
                    CorrectAngle(keyframe.StartPosition.TextureOrientation.X, keyframe.EndPosition.TextureOrientation.X),
                    CorrectAngle(keyframe.StartPosition.TextureOrientation.Y, keyframe.EndPosition.TextureOrientation.Y),
                    CorrectAngle(keyframe.StartPosition.TextureOrientation.Z, keyframe.EndPosition.TextureOrientation.Z)
                );

                // Interpolating textureOrientation over time
                rotationExpr = BuildInterpolationExpression(tpar, rotationExpr, keyframe.StartTime, keyframe.EndTime, 
                    keyframe.StartPosition.TextureOrientation,
                    endOrientation
                );
            }
            GetCenter = Expression.Lambda<Func<double, Vector3>>(centerExpr, tpar).Compile();
            GetTextureOrientation = Expression.Lambda<Func<double, Vector3>>(rotationExpr, tpar).Compile();
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
