using System;
using System.Diagnostics;
using System.Drawing;
using Ajv.Raytracing;
using Ajv.VectorMath;

namespace Ajv.Pool1984
{
    abstract class Primitive : BasePrimitive
    {
        public Color3 DiffuseColor { get; set; }

        public Texture Texture { get; set; }

        public abstract Vector2 GetTextureCoordinates(Vector3 transformedNormal, double time);
    }
}
