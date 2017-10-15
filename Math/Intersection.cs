using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pool1984
{
    struct Intersection
    {
        public const double MinDistance = 1E-5;
        public const double MaxDistance = 1E12;

        public Primitive Entity { get; set; }

        public bool Hit { get; set; }
        public double Distance { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
    }
}
