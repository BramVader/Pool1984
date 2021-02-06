using Ajv.Raytracing;
using Ajv.VectorMath;
using System.Collections.Generic;

namespace Ajv.Pool1984
{
    class Model: BaseModel
    {
        public Color3 AmbientColor { get; set; } = new Color3(0.01, 0.01, 0.01);

        public CubeTexture CubeMap { get; set; }

        public double Reflection { get; set; } = 0.3;

        public int NrSamplesX { get; set; } = 4;

        public int NrSamplesY { get; set; } = 4;

        public int IterationDepth { get; set; } = 2;
    }
}
