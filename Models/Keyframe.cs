using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pool1984
{
    class Keyframe
    {
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public BallPosition StartPosition { get; set; }
        public BallPosition EndPosition { get; set; }
    }
}
