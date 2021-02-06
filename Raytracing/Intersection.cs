using Ajv.VectorMath;

namespace Ajv.Raytracing
{
    public struct Intersection
    {
        public const double MinDistance = 1E-5;
        public const double MaxDistance = 1E12;

        public BasePrimitive Entity { get; set; }

        public bool Hit { get; set; }
        public double Distance { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
    }
}
