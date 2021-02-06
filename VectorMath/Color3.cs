using System.Diagnostics;
using System.Drawing;

namespace Ajv.VectorMath
{
    [DebuggerDisplay("({R}, {G}, {B})")]
    public struct Color3
    {
        public double R { get; set; }
        public double G { get; set; }
        public double B { get; set; }

        public Color3(double r, double g, double b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public static Color3 operator -(Color3 a, Color3 b)
        {
            return new Color3(a.R - b.R, a.G - b.G, a.B - b.B);
        }

        public static Color3 operator +(Color3 a, Color3 b)
        {
            return new Color3(a.R + b.R, a.G + b.G, a.B + b.B);
        }

        public static Color3 operator *(Color3 a, double factor)
        {
            return new Color3(a.R * factor, a.G * factor, a.B * factor);
        }

        public static Color3 operator *(double factor, Color3 a)
        {
            return new Color3(a.R * factor, a.G * factor, a.B * factor);
        }

        public static Color3 operator /(Color3 a, double factor)
        {
            return new Color3(a.R / factor, a.G / factor, a.B / factor);
        }

        public static Color3 operator *(Color3 a, Color3 b)
        {
            return new Color3(a.R * b.R, a.G * b.G, a.B * b.B);
        }

        public Color ToColor()
        {
            int r = (int)(R.Limit(0.0, 1.0) * 255.0);
            int g = (int)(G.Limit(0.0, 1.0) * 255.0);
            int b = (int)(B.Limit(0.0, 1.0) * 255.0);
            return Color.FromArgb(255, r, g, b);
        }

        public static Color3 FromColor(Color color)
        {
            return new Color3 { R = color.R / 255.0, G = color.G / 255.0, B = color.B / 255.0 };
        }
    }
}
