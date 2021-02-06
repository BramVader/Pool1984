using Ajv.Raytracing;
using Ajv.VectorMath;
using System;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace Ajv.Pool1984
{
    class MyRaytracer: Raytracer
    {

        public MyRaytracer(int nrSamplesX, int nrSamplesY, int width, int height, Model model)
            : base(nrSamplesX, nrSamplesY, width, height, model)
        {
        }

        protected Vector2 Hammersley(int i, int numSamples)
        {
            uint b = (uint)i;
            b = (b << 16) | (b >> 16);
            b = ((b & 0x55555555u) << 1) | ((b & 0xAAAAAAAAu) >> 1);
            b = ((b & 0x33333333u) << 2) | ((b & 0xCCCCCCCCu) >> 2);
            b = ((b & 0x0F0F0F0Fu) << 4) | ((b & 0xF0F0F0F0u) >> 4);
            b = ((b & 0x00FF00FFu) << 8) | ((b & 0xFF00FF00u) >> 8);
            double radicalInverseVDC = b * 2.0 * 2.3283064365386963e-10 - 1.0;
            return new Vector2(i * 2.0 / numSamples - 1.0, radicalInverseVDC);
        }

        protected override Color3 RenderRay(Ray ray, int sample, int depth = 0)
        {
            double time = (sample + rnd.NextDouble()) / nrSamples;
            var model = this.model as Model;

            Color3 col = default;
            Intersection closest = default;
            foreach (var entity in model.Primitives)
            {
                var intsec = entity.GetClosestIntersection(ray, IntersectionMode.PositionAndNormal, time, maxDist: 100.0);
                if (intsec.Hit)
                {
                    if (intsec.Distance < closest.Distance || !closest.Hit)
                    {
                        closest = intsec;
                    }
                }
            }

            if (closest.Hit)
            {
                Primitive primitive = closest.Entity as Primitive;

                // Reflected felt is apparently not rendered 
                if (primitive is Plane && depth > 0)
                    return new Color3(0.0, 0.0, 0.0);

                Color3 diffuseColor = primitive.DiffuseColor;

                // Calculate mirror ray
                double a = -Vector3.Dot(closest.Normal, ray.Direction);
                Ray mirrorRay = new Ray { Origin = closest.Position, Direction = ray.Direction + 2.0 * a * closest.Normal };

                // Get texture
                if (primitive.Texture != null)
                {
                    if (primitive is Plane)
                    {
                        diffuseColor = primitive.Texture[
                            (int)(closest.Position.X * 100.0),
                            (int)(closest.Position.Y * 100.0)
                        ];
                    }
                    else
                    {
                        if (primitive is Ball ball)
                        {
                            Vector3 transformedNormal = ball.GetWorldToTextureTransformation(closest.Normal, time);
                            Vector2 p = primitive.GetTextureCoordinates(transformedNormal, time);
                            if (p.Length < 1.0)
                            {
                                Point q = new Point(
                                    (int)((p.X * 0.5 + 0.5) * primitive.Texture.Width),
                                    (int)((p.Y * 0.5 + 0.5) * primitive.Texture.Height)
                                );
                                diffuseColor = primitive.Texture[q.X, q.Y];
                            }
                            else
                            {
                                if (Math.Abs(transformedNormal.X) < 0.5)
                                {
                                    diffuseColor = ball.BandColor;
                                }
                            }
                        }
                    }
                }

                // Calculate lights
                foreach (var light in model.Lights.OfType<SpotLight>())
                {
                    var lightVec1 = light.Center - closest.Position;

                    var lightVec2 = lightVec1.Normalize();
                    Vector3 hor = Vector3.Cross(new Vector3(0.0, 1.0, 0.0), lightVec2).Normalize();
                    Vector3 ver = Vector3.Cross(lightVec2, hor).Normalize();

                    Vector2 v1 = new Vector2(rnd.NextDouble() * 2.0 - 1.0, rnd.NextDouble() * 2.0 - 1.0) * light.Radius1;
                    var lightVec3 = lightVec1 + hor * v1.X + ver * v1.Y;
                    double lightDist = lightVec3.Length;

                    Ray shadowRay = new Ray { Origin = closest.Position, Direction = lightVec3 / lightDist };

                    double shadow = 1.0;
                    foreach (var shadowEntity in model.Primitives.OfType<Ball>())
                    {
                        var intsec = shadowEntity.GetClosestIntersection(shadowRay, IntersectionMode.Hit, Intersection.MinDistance); //, lightDist + 0.01);
                        if (intsec.Hit)
                        {
                            shadow = 0.0;
                            break;
                        }
                    }

                    // Calculate specular highlight analytically
                    double specInt = 0.0;
                    if (primitive is Ball)
                    {
                        double k =
                            Vector3.Dot(light.Center - mirrorRay.Origin, lightVec2) /
                            Vector3.Dot(mirrorRay.Direction, lightVec2);
                        Vector3 v = mirrorRay.Origin + k * mirrorRay.Direction - light.Center;
                        specInt = k > Intersection.MinDistance && k < lightVec1.Length + light.Radius2 ? ((v.Length - light.Radius2) / (light.Radius1 - light.Radius2)).Limit(0.0, 1.0) : 0;
                    }

                    Ray lightRay = new Ray { Origin = closest.Position, Direction = lightVec2 };
                    double diffuseIntensity = Math.Max(0.0, Vector3.Dot(lightRay.Direction, closest.Normal)) * 0.333 * shadow;
                    double specularIntensity = specInt * shadow;
                    col += diffuseIntensity * light.Color * diffuseColor + specularIntensity * light.Color + model.AmbientColor;

                    if (depth < model.IterationDepth && primitive is Ball)
                        col += model.Reflection * RenderRay(mirrorRay, sample, depth + 1);
                }
            }
            else
            // If no object hit, check the environment map
            {
                col = model.CubeMap[ray.Direction];
            }
            return col;
        }

        /// <summary>
        /// Raytraces a picture line-by-line.
        /// After each finished line, the progress is reported, enabling the user interface to show the picture so far
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="ct"></param>
        public void Render(IProgress<Line> progress, CancellationToken ct)
        {
            for (int y = 0; y < height; y++)
            {
                int adr = 0;
                byte[] lineData = new byte[width * 3];
                for (int x = 0; x < width; x++)
                {
                    Color3 color = RenderPixel(new PointF(x, y));
                    var resultColor = color.ToColor();
                    lineData[adr++] = (resultColor.B);
                    lineData[adr++] = (resultColor.G);
                    lineData[adr++] = (resultColor.R);
                }
                if (y >= 0)
                {
                    progress.Report(new Line { LineData = lineData, Y = y });
                }
                ct.ThrowIfCancellationRequested();
            }
        }
    }
}
