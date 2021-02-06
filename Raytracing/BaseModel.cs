using Ajv.VectorMath;
using System.Collections.Generic;

namespace Ajv.Raytracing
{
    public class BaseModel
    {
        public Camera Camera { get; set; }

        public IEnumerable<BaseLight> Lights { get; set; }
        public IEnumerable<BasePrimitive> Primitives { get; set; }
    }
}
