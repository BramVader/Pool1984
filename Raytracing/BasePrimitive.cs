using Ajv.VectorMath;

namespace Ajv.Raytracing
{
    public abstract class BasePrimitive
    {
        public string Name { get; set; }
        public abstract Intersection GetClosestIntersection(Ray ray, IntersectionMode mode, double time, double minDist = Intersection.MinDistance, double maxDist = Intersection.MaxDistance);

    }
}
