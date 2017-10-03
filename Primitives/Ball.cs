using System;
using System.Diagnostics;
using System.Drawing;

namespace Pool1984
{
    class Ball : Entity
    {
        public Vector3 Center { get; set; }
        public double Radius { get; set; }

        public double MinAngle1 { get; set; }
        public double MaxAngle1 { get; set; }
        public double MinAngle2 { get; set; }
        public double MaxAngle2 { get; set; }

        public double CubeMapOffset { get; set; }

        // Original bitmap data
        public PointF PixelCenter { get; set; }
        public SizeF PixelSize { get; set; }
        public float Degrees { get; set; }
        public Bitmap SphereMap { get; set; }
        public Bitmap CubeMap { get; set; }
        public Number Number { get; set; }
        public PointF[][] Boxes { get; set; }

        public Ellipse Ellipse
        {
            get
            {
                return new Ellipse
                {
                    PixelCenter = this.PixelCenter,
                    PixelSize = this.PixelSize,
                    Degrees = this.Degrees
                };
            }
        }

        public Ball()
        {
            Radius = 1.0;
        }

        public override Intersection GetClosestIntersection(Ray ray, IntersectionMode mode, double minDist = Intersection.MinDistance, double maxDist = Intersection.MaxDistance)
        {
            Intersection intsec = new Intersection() { Entity = this };

            // Calculate intersection with ball
            Vector3 ct = ray.Origin - Center;
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
                    intsec.Normal = (intsec.Position - Center) / Radius;
                }
            }
            return intsec;
        }

        public override Vector3 TransformNormal(Vector3 normal)
        {
            return normal * WorldToTexture;
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
