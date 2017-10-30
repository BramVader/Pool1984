using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pool1984
{
    struct Quaternion
    {
        private double[] elements;

        public Quaternion(double[] elements)
        {
            this.elements = elements;
        }

        public Quaternion(double a, double b, double c, double d)
        {
            this.elements = new double[] { a, b, c, d };
        }

        public static Quaternion operator *(Quaternion a, Quaternion b)
        {
            return new Quaternion(
                a.A * b.A - a.B * b.B - a.C * b.C - a.D * b.D,
                a.A * b.B + a.B * b.A + a.C * b.D - a.D * b.C,
                a.A * b.C + a.B * b.D + a.C * b.A + a.D * b.B,
                a.A * b.D + a.B * b.C - a.C * b.B + a.D * b.A
            );
        }

        public double A
        {
            get { return elements[0]; }
            set { elements[0] = value; }
        }

        public double B
        {
            get { return elements[1]; }
            set { elements[1] = value; }
        }

        public double C
        {
            get { return elements[2]; }
            set { elements[2] = value; }
        }

        public double D
        {
            get { return elements[3]; }
            set { elements[3] = value; }
        }

    }
}
