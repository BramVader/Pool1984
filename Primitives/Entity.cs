using System;
using System.Diagnostics;
using System.Drawing;

namespace Pool1984
{
    abstract class Entity
    {
        public string Name { get; set; }
        public Color3 DiffuseColor { get; set; }
        public Color3 BandColor { get; set; }

        public Bitmap Texture { get; set; }

        public Entity()
        {
        }

        public abstract Matrix4 GetWorldToTexture(double time);

        public abstract Matrix4 GetTextureToWorld(double time);

        public abstract Intersection GetClosestIntersection(Ray ray, IntersectionMode mode, double time, double minDist = Intersection.MinDistance, double maxDist = Intersection.MaxDistance);

        public abstract Vector2 GetTextureCoordinates(Vector3 transformedNormal);
    }
}
