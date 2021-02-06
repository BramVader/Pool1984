using System.Diagnostics;
using static System.Math;

namespace Ajv.VectorMath
{
    [DebuggerDisplay("({X}, {Y}, {Z})")]
    public struct Vector3
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector3(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public double Length
        {
            get { return Sqrt(X * X + Y * Y + Z * Z); }
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3 operator *(Vector3 a, double factor)
        {
            return new Vector3(a.X * factor, a.Y * factor, a.Z * factor);
        }

        public static Vector3 operator *(double factor, Vector3 a)
        {
            return new Vector3(a.X * factor, a.Y * factor, a.Z * factor);
        }

        public static Vector3 operator /(Vector3 a, double factor)
        {
            return new Vector3(a.X / factor, a.Y / factor, a.Z / factor);
        }

        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X
            );
        }

        public static double Dot(Vector3 a, Vector3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }
        public static Vector3 Normalize(Vector3 v)
        {
            double l = v.Length;
            if (l > 0.0)
                return v / l;
            return new Vector3();
        }

        public Vector3 Normalize()
        {
            return Normalize(this);
        }

        public override string ToString()
        {
            return $" [{X:0.00}, {Y:0.00}, {Z:0.00}] ";
        }
    }
}
