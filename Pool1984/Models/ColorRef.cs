using Ajv.VectorMath;
using System.Drawing;

namespace Ajv.Pool1984
{
    internal class ColorRef
    {
        public PointF PixelCenter { get; set; }
        public float Radius { get; set; }

        public Color3 Measured { get; set; }
        public Color3 Actual { get; set; }
    }
}