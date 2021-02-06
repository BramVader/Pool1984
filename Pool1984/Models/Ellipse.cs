using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ajv.Pool1984
{
    class Ellipse
    {
        public PointF PixelCenter { get; set; }
        public SizeF PixelSize { get; set; }
        public float Degrees { get; set; }

        public IEnumerable<PointF> GetOutline(int nrPoints)
        {
            double sn = Math.Sin(Degrees * Math.PI / 180.0);
            double cs = Math.Cos(Degrees * Math.PI / 180.0);
            for (int n = 0; n <= nrPoints; n++)
            {
                double x1 = 0.5 * PixelSize.Width * Math.Cos(n * 2.0 * Math.PI / nrPoints);
                double y1 = 0.5 * PixelSize.Height * Math.Sin(n * 2.0 * Math.PI / nrPoints);
                yield return
                    new PointF(
                        (float)(PixelCenter.X + x1 * cs + y1 * sn),
                        (float)(PixelCenter.Y + y1 * cs - x1 * sn)
                    );
            }
        }
    }
}
