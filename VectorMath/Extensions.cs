namespace Ajv.VectorMath
{
    public static class Extensions
    {

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
