using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Pool1984
{
    class Texture
    {
        protected Color3[,] pixels;
        protected int width;
        protected int height;
        protected Func<Color3, Color3> transformation = c => c;

        public Texture(Color3[,] pixels, int width, int height)
        {
            this.pixels = pixels;
            this.width = width;
            this.height = height;
        }

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public Color3 this[int x, int y]
        {
            get { return transformation(pixels[(x % width + width) % width, (y % height + height) % height]); }
            set { pixels[(x % width + width) % width, (y % height + height) % height] = value; }
        }

        public void SetColorTransformation(Color3 color1, Color3 color2)
        {
            Color3 dColor = color2 - color1;
            transformation = (col) =>
            {
                return color1 + dColor * col.R; // Just take a component
            };
        }

        public Texture BakeColorTransformation()
        {
            var pixels = new Color3[width, height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    pixels[x, y] = transformation(this.pixels[x, y]);
                }
            return new Texture(pixels, width, height);
        }

        protected static Color3[,] ReadBitmap(Bitmap bitmap, Rectangle rect)
        {
            int width = rect.Width;
            int height = rect.Height;
            Color3[,] pixels = new Color3[width, height];

            BitmapData bmpDataRead = null;
            try
            {
                bmpDataRead = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                unsafe
                {
                    byte* adr = (byte*)bmpDataRead.Scan0; // + rect.Top * bmpDataRead.Stride + rect.Left * 3;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            int b = *adr++;
                            int g = *adr++;
                            int r = *adr++;
                            pixels[x, y] = Color3.FromColor(Color.FromArgb(r, g, b));
                        }
                        adr = adr + bmpDataRead.Stride - width * 3;
                    }
                }
                return pixels;
            }
            finally
            {
                if (bmpDataRead != null)
                {
                    try
                    {
                        bitmap.UnlockBits(bmpDataRead);

                    }
                    catch
                    {
                    }
                }
            }
        }

        public static Texture FromBitmap(Bitmap bitmap, Rectangle rect)
        {
            int width = rect.Width;
            int height = rect.Height;
            return new Texture(ReadBitmap(bitmap, rect), width, height);
        }

        public static Texture FromBitmap(Bitmap bitmap)
        {
            return FromBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
        }
    }
}
