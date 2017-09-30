using System;
using System.Diagnostics;
using System.Drawing;

namespace Pool1984
{
    class Plane : Entity
    {
        public Vector3 Center { get; set; }
        public Vector3 Normal { get; set; }

        public Plane()
        {
        }

        public override Intersection GetClosestIntersection(Ray ray, IntersectionMode mode, double minDist = Intersection.MinDistance, double maxDist = Intersection.MaxDistance)
        {
            Intersection intsec = new Intersection() { Entity = this };

            // Calculate intersection with ball
            Vector3 ct = ray.Origin - Center;
            double k = Vector3.Dot(ray.Direction, Normal);
            if (k != 0)
            {
                intsec.Distance = -Vector3.Dot(ct, Normal) / k;
                intsec.Hit = intsec.Distance > minDist && intsec.Distance < maxDist;

                if (intsec.Hit && mode > IntersectionMode.Hit)
                {
                    intsec.Position = ray.Origin + intsec.Distance * ray.Direction;
                    intsec.Normal = Normal;
                }
            }
            return intsec;
        }

        public override Vector2 GetTextureCoordinates(Intersection closest)
        {
            throw new NotImplementedException();
        }
    }
}
