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

        public Matrix4 WorldToTexture { get; set; }
        public Matrix4 TextureToWorld { get; set; }
        public Bitmap Texture { get; set; }

        public Entity()
        {
        }

        public abstract Intersection GetClosestIntersection(Ray ray, IntersectionMode mode, double minDist = Intersection.MinDistance, double maxDist = Intersection.MaxDistance);

        public abstract Vector3 TransformNormal(Vector3 normal);
        public abstract Vector2 GetTextureCoordinates(Vector3 transformedNormal);
    }
}
