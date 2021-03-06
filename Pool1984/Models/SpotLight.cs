﻿using Ajv.Raytracing;
using Ajv.VectorMath;
using System.Drawing;

namespace Ajv.Pool1984
{
    class SpotLight: BaseLight
    {
        public class Spot
        {
            public string TargetPosition { get; set; }

            public PointF PixelCenter { get; set; }
            public SizeF PixelSize1 { get; set; }
            public SizeF PixelSize2 { get; set; }
            public float Degrees { get; set; }

            public Ray ReflectedRay { get; set; }
            public double Radius1 { get; set; }
            public double Radius2 { get; set; }

            public Ellipse InnerEllipse
            {
                get
                {
                    return new Ellipse
                    {
                        PixelCenter = this.PixelCenter,
                        PixelSize = this.PixelSize1,
                        Degrees = this.Degrees
                    };
                }
            }

            public Ellipse OuterEllipse
            {
                get
                {
                    return new Ellipse
                    {
                        PixelCenter = this.PixelCenter,
                        PixelSize = this.PixelSize2,
                        Degrees = this.Degrees
                    };
                }
            }

        }

        public Spot[] Spots { get; set; }

        public Vector3 Center { get; set; }
        public double Radius1 { get; set; }
        public double Radius2 { get; set; }

        public Color3 Color { get; set; }

        public SpotLight()
        {
            Color = new Color3(1.0, 1.0, 1.0);
        }
    }
}
