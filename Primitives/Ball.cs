using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;

namespace Pool1984
{
    class Ball : Entity
    {
        public Func<double, Vector3> GetCenter = d => new Vector3();
        public Func<double, Vector3> GetTextureOrientation = d => new Vector3();

        public double Radius { get; set; }

        public double MinAngle1 { get; set; }
        public double MaxAngle1 { get; set; }
        public double MinAngle2 { get; set; }
        public double MaxAngle2 { get; set; }

        public double CubeMapOffset { get; set; }

        public override Matrix4 GetWorldToTexture(double time)
        {
            return Matrix4.Rotate(GetTextureOrientation(time));
        }

        public override Matrix4 GetTextureToWorld(double time)
        {
            return Matrix4.RotateInv(GetTextureOrientation(time));
        }

        private Expression BuildInterpolationExpression(ParameterExpression tpar, Expression prevExpression, double startTime, double endTime, Vector3 startVector, Vector3 endVector)
        {
            // (time - startTime) * (endVector - startVector)
            Expression exprA =
                Expression.Multiply(
                    startTime > 0.0 ?
                        Expression.Subtract(tpar, Expression.Constant(startTime)) :
                        (Expression)tpar,
                    Expression.Constant(endVector - startVector)
                );

            // startVector + (time - startTime) * (endVector - startVector) / (endTime - startTime)
            Expression exprB =
                Expression.Add(
                        Expression.Constant(startVector),
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
                        Expression.Constant(endVector)
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
                    prevExpression ?? Expression.Constant(startVector),
                    exprC
                ) :
                exprC;
        }

        public void ApplyKeyframes(IEnumerable<Keyframe> frames)
        {
            ParameterExpression tpar = Expression.Parameter(typeof(double), "t");

            Expression centerExpr = null;
            Expression rotationExpr = null;
            foreach (var keyframe in frames.Where(it => it.StartPosition.Ball == this).OrderBy(it => it.StartTime))
            {
                // Interpolating position.Center over time
                centerExpr = BuildInterpolationExpression(tpar, centerExpr, keyframe.StartTime, keyframe.EndTime, keyframe.StartPosition.Center, keyframe.EndPosition.Center);

                // Interpolating textureOrientation over time
                rotationExpr = BuildInterpolationExpression(tpar, rotationExpr, keyframe.StartTime, keyframe.EndTime, keyframe.StartPosition.TextureOrientation, keyframe.EndPosition.TextureOrientation);
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

        public override Vector2 GetTextureCoordinates(Vector3 transformedNormal)
        {
            double angle1 = Math.Atan2(transformedNormal.Y, transformedNormal.X);
            double angle2 = Math.Atan2(transformedNormal.Z, new Vector2(transformedNormal.X, transformedNormal.Y).Length);
            return new Vector2(
                (angle1 - (MinAngle1 + MaxAngle1) * 0.5) * 2.0 / (MaxAngle1 - MinAngle1),
                ((MinAngle2 + MaxAngle2) * 0.5 - angle2) * 2.0 / (MaxAngle2 - MinAngle2)
            );
        }
    }
}
