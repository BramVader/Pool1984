using System;

namespace Pool1984
{
    class Camera
    {
        private Vector3 from, at, up;
        private double apertureH, apertureV;

        private Vector3 look, hor, ver;
        private double angle;

        public Vector3 From
        {
            get { return from; }
            set
            {
                from = value;
                Recalculate();
            }
        }

        public Vector3 At
        {
            get { return at; }
            set
            {
                at = value;
                Recalculate();
            }
        }

        public Vector3 Up
        {
            get { return up; }
            set
            {
                up = value;
                Recalculate();
            }
        }

        public double ApertureH
        {
            get { return apertureH; }
            set
            {
                apertureH = value;
                Recalculate();
            }
        }

        public double ApertureV
        {
            get { return apertureV; }
            set
            {
                apertureV = value;
                Recalculate();
            }
        }

        public double Angle
        {
            get { return angle; }
            set
            {
                angle = value;
                Recalculate();
            }
        }

        public Vector3 Look { get { return look; } }

        public Vector3 Hor { get { return hor; } }

        public Vector3 Ver { get { return ver; } }

        public Camera()
        {
            angle = 35;
            apertureH = 13.6;
            apertureV = 11.5;
            from = new Vector3(0.0, 0.0, 20.0);
            at = new Vector3(0.0, 0.0, 0.0);
            Recalculate();
        }

        private double lookVecSqr;
        private double lookHorSqr;
        private double lookVerSqr;

        private void Recalculate()
        {
            up = new Vector3(Math.Cos(angle * Math.PI / 180.0), Math.Sin(angle * Math.PI / 180.0), 0.0);
            look = at - from;
            double dist = look.Length;
            double hsize = Math.Tan(apertureH * Math.PI / 180.0) * dist;
            double vsize = Math.Tan(apertureV * Math.PI / 180.0) * dist;
            hor = Vector3.Normalize(Vector3.Cross(look, up)) * hsize;
            ver = Vector3.Normalize(Vector3.Cross(hor, look)) * vsize;
            lookVecSqr = dist * dist;
            lookHorSqr = hsize * hsize;
            lookVerSqr = vsize * vsize;
        }

        public bool IsVertexVisible(Vector3 vertex)
        {
            vertex -= from;
            double a = Vector3.Dot(vertex, look);
            return a > 0.01;
        }

        public Vector2 VertexToCoord(Vector3 vertex)
        {
            vertex -= from;
            double a = Vector3.Dot(vertex, look);
            if (a > 0.01)
            {
                a = lookVecSqr / a;
                return new Vector2(
                    a * Vector3.Dot(vertex, hor) / lookHorSqr,
                    a * Vector3.Dot(vertex, ver) / lookVerSqr
                );
            }
            return default(Vector2);
        }

        public Ray CoordToRay(Vector2 coord)
        {
            return new Ray { Origin = from, Direction = Vector3.Normalize(look + ((double)coord.X) * hor + ((double)coord.Y) * ver) };
        }

        public Camera Clone()
        {
            var newCamera = new Camera
            {
                angle = this.angle,
                from = this.from,
                at = this.at,
                up = this.up,
                apertureH = this.apertureH,
                apertureV = this.apertureV
            };
            newCamera.Recalculate();
            return newCamera;
        }

    }
}
