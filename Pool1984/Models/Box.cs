using Ajv.VectorMath;
using System.Drawing;

namespace Ajv.Pool1984
{
    class Box
    {
        public PointF[] PixelCoord { get; set; }
        public Vector3[] Vertices { get; set; }

        public Box()
        {
            PixelCoord = new PointF[4];
            Vertices = new Vector3[4];
        }
    }
}
