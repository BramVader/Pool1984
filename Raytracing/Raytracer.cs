using Ajv.VectorMath;
using System;
using System.Drawing;

namespace Ajv.Raytracing
{
    public abstract class Raytracer
    {
        public class Line
        {
            public byte[] LineData { get; set; }

            public int Y { get; set; }
        }

        protected readonly Random rnd = new Random();
        protected readonly int nrSamplesX;
        protected readonly int nrSamplesY;
        protected readonly int nrSamples;
        protected readonly int width;
        protected readonly int height;
        protected readonly BaseModel model;

        public Raytracer(int nrSamplesX, int nrSamplesY, int width, int height, BaseModel model)
        {
            this.nrSamplesX = nrSamplesX;
            this.nrSamplesY = nrSamplesY;
            this.nrSamples = nrSamplesX * nrSamplesY;
            this.width = width;
            this.height = height;
            this.model = model;
        }

        private Vector2 PixelToCoord(PointF pixel)
        {
            return new Vector2(
                pixel.X * 2.0 / width - 1.0,
                1.0 - pixel.Y * 2.0 / height
            );
        }

        public Color3 RenderPixel(PointF point)
        {
            Color3 color = new Color3();
            int sample = 0;
            for (int sampleY = 0; sampleY < nrSamplesY; sampleY++)
            {
                for (int sampleX = 0; sampleX < nrSamplesX; sampleX++)
                {
                    PointF point2 = new PointF(
                        point.X + (float)(sampleX + rnd.NextDouble()) / nrSamplesX,
                        point.Y + (float)(sampleY + rnd.NextDouble()) / nrSamplesY
                    );
                    Vector2 coord = PixelToCoord(point2);
                    Ray ray = model.Camera.CoordToRay(coord);
                    color += RenderRay(ray, sample++);
                }
            }
            return color / (nrSamplesX * nrSamplesY);
        }

        protected abstract Color3 RenderRay(Ray ray, int sample, int depth = 0);
    }
}
