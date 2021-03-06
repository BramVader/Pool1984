﻿using Ajv.Raytracing;
using Ajv.VectorMath;
using System.Diagnostics;
using System.Drawing;

namespace Ajv.Pool1984
{
    [DebuggerDisplay("{Name} -> {TargetBall}")]
    class BallPosition
    {
        public string Name { get; set; }
        public string TargetBall { get; set; }

        public Ball Ball { get; set; }

        public Vector3 Center { get; set; }
        public Vector3 TextureOrientation { get; set; }

        public double CubeMapOffset { get; set; }

        // Calculated texture transformation + range
        public Matrix4 WorldToTexture { get; set; }
        public Matrix4 TextureToWorld { get; set; }

        // Original bitmap data
        public PointF PixelCenter { get; set; }
        public SizeF PixelSize { get; set; }
        public float Degrees { get; set; }
        public Bitmap CubeMap { get; set; }
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

        public Intersection GetClosestIntersection(Ray ray, IntersectionMode mode, double minDist = Intersection.MinDistance, double maxDist = Intersection.MaxDistance)
        {
            return Ball.GetClosestIntersection(ray, mode, Center, minDist, maxDist);
        }
    }
}
