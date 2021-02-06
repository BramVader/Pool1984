using System.Diagnostics;
using System.Globalization;
using System.Text;
using static System.Math;

namespace Ajv.VectorMath
{
    [DebuggerDisplay("{this.ToString()}")]
    public struct Matrix4
    {
        private readonly double[,] elements;

        public Matrix4(double[,] elements)
        {
            this.elements = elements;
        }

        public bool Valid
        {
            get { return elements != null; }
        }

        public override string ToString()
        {
            StringBuilder st = new StringBuilder("[");
            for (int j = 0; j < 4; j++)
            {
                if (j > 0) st.Append(", ");
                st.Append("[");
                for (int i = 0; i < 4; i++)
                {
                    if (i > 0) st.Append(", ");
                    st.Append(elements[j, i].ToString("0.000", CultureInfo.InvariantCulture));
                }
                st.Append("]");
            }
            st.Append("]");
            return st.ToString();
        }

        public static Matrix4 operator *(Matrix4 a, Matrix4 b)
        {
            double[,] result = new double[4, 4];
            for (int j = 0; j < 4; j++)
                for (int i = 0; i < 4; i++)
                {
                    double sum = 0.0;
                    for (int r = 0; r < 4; r++)
                    {
                        sum += a[i, r] * b[r, j];
                    }
                    result[j, i] = sum;
                }

            // Keep homogeneous divide
            double w = result[3, 3];
            if (w != 0.0 && w != 1.0)
            {
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                    {
                        result[j, i] /= w;
                    }
                result[3, 3] = 1.0;
            }
            return new Matrix4(result);
        }

        public static Vector3 operator *(Vector3 a, Matrix4 b)
        {
            return new Vector3(
                a.X * b[0, 0] + a.Y * b[1, 0] + a.Z * b[2, 0] + b[3, 0],
                a.X * b[0, 1] + a.Y * b[1, 1] + a.Z * b[2, 1] + b[3, 1],
                a.X * b[0, 2] + a.Y * b[1, 2] + a.Z * b[2, 2] + b[3, 2]
            );
        }

        public double this[int u, int v]
        {
            get { return elements[v, u]; }
            set { elements[v, u] = value; }
        }

        public static Matrix4 RotateAxis(Vector3 axis, double angle)
        {
            double sn = Sin(angle);
            double cs = Cos(angle);
            double tc = 1 - cs;
            return new Matrix4(new[,]
            {
                { tc * axis.X * axis.X + cs, tc * axis.X * axis.Y - sn * axis.Z, tc * axis.X * axis.Z + sn * axis.Y, 0.0 },
                { tc * axis.X * axis.Y + sn * axis.Z, tc * axis.Y * axis.Y + cs, tc * axis.Y * axis.Z - sn * axis.X, 0.0 },
                { tc * axis.X * axis.Z - sn * axis.Y, tc * axis.Y * axis.Z + sn * axis.X, tc * axis.Z * axis.Z + cs, 0.0 },
                { 0.0, 0.0, 0.0, 1.0 }
            });
        }

        public static Matrix4 RotateAxisXY(Vector3 axis, double angle)
        {
            double sn = Sin(angle);
            double cs = Cos(angle);
            double tc = 1 - cs;
            return new Matrix4(new[,]
            {
                { tc * axis.X * axis.X + cs, tc * axis.X * axis.Y, sn * axis.Y, 0.0 },
                { tc * axis.X * axis.Y, tc * axis.Y * axis.Y + cs, -sn * axis.X, 0.0 },
                { tc * -sn * axis.Y, sn * axis.X, cs, 0.0 },
                { 0.0, 0.0, 0.0, 1.0 }
            });
        }

        public static Matrix4 RotateX(double angle)
        {
            double sn = Sin(angle);
            double cs = Cos(angle);
            return new Matrix4(new[,]
            {
                { 1.0, 0.0, 0.0, 0.0 },
                { 0.0, cs,  -sn, 0.0 },
                { 0.0, sn,  cs,  0.0 },
                { 0.0, 0.0, 0.0, 1.0 }
            });
        }

        public static Matrix4 RotateY(double angle)
        {
            double sn = Sin(angle);
            double cs = Cos(angle);
            return new Matrix4(new[,]
            {
                { cs,  0.0, sn,  0.0 },
                { 0.0, 1.0, 0.0, 0.0 },
                { -sn, 0.0,  cs, 0.0 },
                { 0.0, 0.0, 0.0, 1.0 }
            });
        }

        public static Matrix4 RotateZ(double angle)
        {
            double sn = Sin(angle);
            double cs = Cos(angle);
            return new Matrix4(new[,]
            {
                { cs,  -sn, 0.0, 0.0 },
                { sn,  cs,  0.0, 0.0 },
                { 0.0, 0.0, 1.0, 0.0 },
                { 0.0, 0.0, 0.0, 1.0 }
            });
        }

        public static Matrix4 Rotate(Vector3 zyx)
        {
            return
                RotateZ(zyx.Z) *
                RotateY(zyx.Y) *
                RotateX(zyx.X);
        }

        public static Matrix4 RotateInv(Vector3 zyx)
        {
            return
                RotateX(-zyx.X) *
                RotateY(-zyx.Y) *
                RotateZ(-zyx.Z);
        }

        public static Matrix4 Translate(Vector3 v)
        {
            return new Matrix4(new[,]
            {
                { 1.0, 0.0, 0.0, v.X },
                { 0.0, 1.0, 0.0, v.Y },
                { 0.0, 0.0, 1.0, v.Z },
                { 0.0, 0.0, 0.0, 1.0 }
            });
        }

        public static Matrix4 AffineInvert(Matrix4 m)
        {
            Matrix3 RT = Matrix3.Transpose(new Matrix3(m.elements));
            Vector3 rT = new Vector3(m[3, 0], m[3, 1], m[3, 2]) * RT;
            return new Matrix4(new[,]
            {
                { m[0, 0], m[0, 1], m[0, 2], -rT.X },
                { m[1, 0], m[1, 1], m[1, 2], -rT.Y },
                { m[2, 0], m[2, 1], m[2, 2], -rT.Z },
                { 0.0, 0.0, 0.0, 1.0 }
            });
        }

    }
}
