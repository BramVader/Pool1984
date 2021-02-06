using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ajv.Pool1984
{
    class Number
    {
        public string TargetPosition { get; set; }
        public BallPosition Position { get; set; }

        public PointF PixelCenter { get; set; }
        public SizeF PixelSize { get; set; }
        public float Degrees { get; set; }

        public PointF OrientStart { get; set; }
        public PointF OrientEnd { get; set; }

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
    }
}
