using Ajv.VectorMath;
using System.Diagnostics;

namespace Ajv.Pool1984
{
    [DebuggerDisplay("[{StartTime,F1}..{EndTime,F1}] {StartPosition.Name}-{EndPosition.Name}")]
    class Keyframe
    {
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public BallPosition StartPosition { get; set; }
        public BallPosition EndPosition { get; set; }

        public Vector3 Direction { get; set; }
        public Vector3 AxisOfRotation { get; set; }
        public double Angle { get; set; }
    }
}
