using System.Diagnostics;
using System.Drawing;
using static System.Math;

namespace Ajv.VectorMath
{
    [DebuggerDisplay("({X}, {Y})")]
    public struct Vector2
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double Length
        {
            get { return Sqrt(X * X + Y * Y); }
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2 operator *(Vector2 a, double factor)
        {
            return new Vector2(a.X * factor, a.Y * factor);
        }

        public static Vector2 operator *(double factor, Vector2 a)
        {
            return new Vector2(a.X * factor, a.Y * factor);
        }

        public static Vector2 operator /(Vector2 a, double factor)
        {
            return new Vector2(a.X / factor, a.Y / factor);
        }

        public static double Dot(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }
        public static Vector2 Normalize(Vector2 v)
        {
            double l = v.Length;
            if (l > 0.0)
                return v / l;
            return new Vector2();
        }

        public Vector2 Normalize()
        {
            return Normalize(this);
        }

        public static implicit operator Vector2(PointF p)
        {
            return new Vector2(p.X, p.Y);
        }

        public static explicit operator PointF(Vector2 p)
        {
            return new PointF((float)p.X, (float)p.Y);
        }

        public static double? Distance(Vector2 a, Vector2 b, Vector2 p)
        {
            Vector2 ab = b - a;
            Vector2 ap = p - a;
            double lenSq = Dot(ab, ab);
            if (lenSq == 0.0)
                return null;
            double k = Dot(ap, ab) / lenSq;
            if (k < 0.0 || k > 1.0)
                return null;
            return (p - (a + k * ab)).Length;
        }

    }
}
