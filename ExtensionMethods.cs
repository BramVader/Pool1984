using System.Drawing;
using System.Windows.Forms;

namespace Pool1984
{
    public static class ExtensionMethods
    {
        private static Control FindControlAtPoint(Control container, Point pos)
        {
            Control child;
            foreach (Control c in container.Controls)
            {
                if (c.Visible && c.Bounds.Contains(pos))
                {
                    child = FindControlAtPoint(c, new Point(pos.X - c.Left, pos.Y - c.Top));
                    if (child == null) return c;
                    else return child;
                }
            }
            return null;
        }

        public static Control GetControlAt(this Form form, Point pos)
        {
             if (form.Bounds.Contains(pos))
                return FindControlAtPoint(form, form.PointToClient(pos));
            return null;
        }

        public static double Limit(this double value, double min, double max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static int Limit(this int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static double SmoothStep(this double value, double min, double max)
        {
            return Limit((value - min) * (max - min), min, max);
        }
    }
}
