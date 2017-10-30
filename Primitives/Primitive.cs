using System;
using System.Diagnostics;
using System.Drawing;

namespace Pool1984
{
    abstract class Primitive
    {
        public string Name { get; set; }
        public Color3 DiffuseColor { get; set; }
        public double Reflection { get; set; }

        public Texture Texture { get; set; }

        public Primitive()
        {
        }

           
        public abstract Intersection GetClosestIntersection(Ray ray, IntersectionMode mode, double time, double minDist = Intersection.MinDistance, double maxDist = Intersection.MaxDistance);

        public abstract Vector2 GetTextureCoordinates(Vector3 transformedNormal, double time);
    }
}
