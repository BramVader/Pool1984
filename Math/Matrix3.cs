using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pool1984
{
    struct Matrix3
    {
        private double[,] elements;

        public Matrix3(double[,] elements)
        {
            this.elements = elements;
        }

        public static Matrix3 operator *(Matrix3 a, Matrix3 b)
        {
            double[,] result = new double[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    double sum = 0.0;
                    for (int r = 0; r < 3; r++)
                    {
                        sum += a[i, r] * b[r, j];
                    }
                    result[j, i] = sum;
                }
            return new Matrix3(result);
        }

        public static Vector3 operator *(Vector3 a, Matrix3 b)
        {
            return new Vector3(
                a.X * b[0, 0] + a.Y * b[1, 0] + a.Z * b[2, 0],
                a.X * b[0, 1] + a.Y * b[1, 1] + a.Z * b[2, 1],
                a.X * b[0, 2] + a.Y * b[1, 2] + a.Z * b[2, 2]
            );
        }

        public double this[int u, int v]
        {
            get { return elements[v, u]; }
            set { elements[v, u] = value; }
        }

        public static Matrix3 Transpose(Matrix3 m)
        {
            return new Matrix3(new[,]
            {
                { m[0, 0], m[0, 1], m[0, 2] },
                { m[1, 0], m[1, 1], m[1, 2] },
                { m[2, 0], m[2, 1], m[2, 2] }
            });
        }

        public double GetDeterminant()
        {
            return
                 elements[0, 0] * elements[1, 1] * elements[2, 2] +
                 elements[0, 1] * elements[1, 2] * elements[2, 0] +
                 elements[0, 2] * elements[1, 0] * elements[2, 1] +

                -elements[0, 2] * elements[1, 1] * elements[2, 0] +
                -elements[0, 1] * elements[1, 0] * elements[2, 2] +
                -elements[0, 0] * elements[1, 2] * elements[2, 1];
        }

        public static Matrix3 Invert(Matrix3 m)
        {
            double[,] result = new double[3, 3];
            double det = m.GetDeterminant();
            return new Matrix3(new[,] {
                {
                     (m[1, 1] * m[2, 2] - m[2, 1] * m[1, 2]) / det,
                    -(m[1, 0] * m[2, 2] - m[2, 0] * m[1, 2]) / det,
                     (m[1, 0] * m[2, 1] - m[2, 0] * m[1, 1]) / det
                },
                {
                    -(m[0, 1] * m[2, 2] - m[2, 1] * m[0, 2]) / det,
                     (m[0, 0] * m[2, 2] - m[2, 0] * m[0, 2]) / det,
                    -(m[0, 0] * m[2, 1] - m[2, 0] * m[0, 1]) / det
                },
                {
                     (m[0, 1] * m[1, 2] - m[1, 1] * m[0, 2]) / det,
                    -(m[0, 0] * m[1, 2] - m[1, 0] * m[0, 2]) / det,
                     (m[0, 0] * m[1, 1] - m[1, 0] * m[0, 1]) / det
                }
            });
        }
    }
}
