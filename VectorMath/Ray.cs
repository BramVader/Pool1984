namespace Ajv.VectorMath
{
    public struct Ray
    {
        public Vector3 Origin { get; set; }

        public Vector3 Direction { get; set; }

        public static Vector3 Closest(params Ray[] rays)
        {
            Vector3 result = default(Vector3);
            int sum = 0;
            for (int n = 0; n < rays.Length - 1; n++)
                for (int m = n + 1; m < rays.Length; m++)
                {
                    Ray a = rays[n];
                    Ray b = rays[m];

                    Vector3 c = b.Origin - a.Origin;
                    double f = Vector3.Dot(a.Direction, a.Direction) * Vector3.Dot(b.Direction, b.Direction) - Vector3.Dot(a.Direction, b.Direction) * Vector3.Dot(a.Direction, b.Direction);
                    if (f != 0.0)
                    {
                        Vector3 d = a.Origin + a.Direction * (-Vector3.Dot(a.Direction, b.Direction) * Vector3.Dot(b.Direction, c) + Vector3.Dot(a.Direction, c) * Vector3.Dot(b.Direction, b.Direction)) / f;
                        Vector3 e = b.Origin + b.Direction * (Vector3.Dot(a.Direction, b.Direction) * Vector3.Dot(a.Direction, c) - Vector3.Dot(b.Direction, c) * Vector3.Dot(a.Direction, a.Direction)) / f;
                        result = result + (d + e) / 2.0;
                        sum++;
                    }
                }
            if (sum == 0)
                return default(Vector3);
            else
                return result / sum;
        }
    }
}
