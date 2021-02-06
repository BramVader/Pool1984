using System;
using System.Drawing;
using System.Drawing.Imaging;
using Ajv.VectorMath;

namespace Ajv.Pool1984
{
    class CubeTexture: Texture
    {
        public CubeTexture(Color3[,] pixels, int width, int height)
            : base(pixels, width, height)
        {
        }

        public Color3 this[Vector3 dir]
        {
            get
            {
                Vector2 p = Project(dir, out int plane);
                if (plane > 0 && plane < 5)     // Top & Bottom planes ignored for now (no interesting stuff here)
                {
                    int px = (int)((p.X * 0.5 + 0.5 + (plane - 1)) * width / 4).Limit(0, width - 1);
                    int py = (int)((0.5 - p.Y * 0.5) * height).Limit(0, height - 1);
                    return pixels[px, py];
                }
                return default;
            }
        }

        public static Vector2 Project(Vector3 dir, out int plane)
        {
            plane =
                Math.Abs(dir.X) > Math.Abs(dir.Y) && Math.Abs(dir.X) > Math.Abs(dir.Z) ? (dir.X > 0 ? 2 : 4) :
                Math.Abs(dir.Y) > Math.Abs(dir.Z) ? (dir.Y > 0 ? 3 : 1) : (dir.Z > 0 ? 0 : 5);

            double k;
            switch (plane)
            {
                case 0:
                    k = 1.0 / dir.Z;
                    return new Vector2(dir.X * k, dir.Y * k);
                case 1:
                    k = -1.0 / dir.Y;
                    return new Vector2(dir.X * k, dir.Z * k);
                case 2:
                    k = 1.0 / dir.X;
                    return new Vector2(dir.Y * k, dir.Z * k);
                case 3:
                    k = 1.0 / dir.Y;
                    return new Vector2(-dir.X * k, dir.Z * k);
                case 4:
                    k = -1.0 / dir.X;
                    return new Vector2(-dir.Y * k, dir.Z * k);
                default:    // 5
                    k = -1.0 / dir.Z;
                    return new Vector2(dir.X * k, -dir.Y * k);
            }
        }

        public new static CubeTexture FromBitmap(Bitmap bitmap, Rectangle rect = default)
        {
            if (rect == default)
                rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            int width = rect.Width;
            int height = rect.Height;
            return new CubeTexture(ReadBitmap(bitmap, rect), width, height);
        }
    }
}
