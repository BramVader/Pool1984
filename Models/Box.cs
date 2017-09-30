using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Pool1984
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
