using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pool1984
{
    class Model
    {
        public Camera Camera { get; set; }

        public IEnumerable<Primitive> Primitives { get; set; }

        public IEnumerable<Light> Lights { get; set; }

        public Color3 AmbientColor { get; set; } = new Color3(0.01, 0.01, 0.01);

        public CubeTexture CubeMap { get; set; }
    }
}
