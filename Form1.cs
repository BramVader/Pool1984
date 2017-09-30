using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace Pool1984
{
    public partial class Form1 : Form
    {
        private static int textureSize = 512;
        private Color3 ambientColor = Color3.FromColor(Color.FromArgb(20, 20, 20));
        private Color3 numberTextureBlackColor = Color3.FromColor(Color.Black);
        private Color3 numberTextureWhiteColor = Color3.FromColor(Color.Ivory);
        private double reflection = 0.2;

        private static Number[] numbers = new Number[]
        {
            // Ball 1
            new Number { PixelCenter = new PointF(124.72f, 175.53f), PixelSize = new SizeF(90.68f, 95.44f), Degrees = -24f,  OrientStart = new PointF(138.7196445106f, 156.106349604506f),  OrientEnd = new PointF(127.381059997338f, 201.460667501665f) },
            // Ball 4
            new Number { PixelCenter = new PointF(641.06f, 154.74f), PixelSize = new SizeF(74.17f, 93.62f), Degrees = 41.3f, OrientStart = new PointF(653.944924793219f, 144.578793805644f), OrientEnd = new PointF(644.949647746031f, 184.830750939373f) },
            // Ball 8a
            new Number { PixelCenter = new PointF(520.97f, 181.08f), PixelSize = new SizeF(98.21f, 93.10f), Degrees = 96f, OrientStart = new PointF(510.133877883348f, 199.192951606807f), OrientEnd = new PointF(534.889787403969f, 163.287449938223f) },		
            // Ball 8b
            new Number { PixelCenter = new PointF(486.89f, 205.26f), PixelSize = new SizeF(92.53f, 94.44f), Degrees = -4.8f, OrientStart = new PointF(498.228364144423f, 189.744135378232f), OrientEnd = new PointF(473.283478215247f, 225.271684397674f) },
        };

        private static PointF[][][] boxes = new PointF[][][]
        {
            // Boxes on Ball 1
            new PointF[][]
            {
                new PointF[4] { new PointF (133.74f, 205.89f), new PointF(137.83f, 202.42f), new PointF(140.17f, 205.05f), new PointF(136.18f, 208.63f) },
                new PointF[4] { new PointF (138.60f, 201.71f), new PointF(149.29f, 188.97f), new PointF(152.57f, 191.44f), new PointF(140.92f, 204.33f) },
                new PointF[4] { new PointF (149.88f, 188.04f), new PointF(152.03f, 184.38f), new PointF(156.29f, 186.40f), new PointF(153.24f, 190.57f) },
                new PointF[4] { new PointF (136.02f, 209.78f), new PointF(157.08f, 186.78f), new PointF(162.12f, 189.17f), new PointF(141.01f, 215.42f) },
                new PointF[4] { new PointF (152.64f, 183.24f), new PointF(160.00f, 159.49f), new PointF(161.71f, 159.48f), new PointF(154.21f, 183.98f) },
                new PointF[4] { new PointF (155.20f, 184.46f), new PointF(162.70f, 159.47f), new PointF(171.40f, 159.37f), new PointF(162.72f, 188.03f) },
                new PointF[4] { new PointF (120.43f, 215.63f), new PointF(128.50f, 211.51f), new PointF(132.29f, 218.01f), new PointF(124.24f, 222.78f) },
                new PointF[4] { new PointF (92.00f, 111.85f), new PointF(72.65f, 125.87f), new PointF(64.69f, 117.36f), new PointF(87.98f, 102.65f) },
            },

            // Boxes on Ball 4
            new PointF[][]
            {
                new PointF[4] { new PointF (699.88f, 164.61f), new PointF(703.88f, 161.80f), new PointF(705.76f, 165.08f), new PointF(700.91f, 168.02f) },
                new PointF[4] { new PointF (704.85f, 161.06f), new PointF(717.41f, 148.06f), new PointF(720.37f, 151.27f), new PointF(706.79f, 164.37f) },
                new PointF[4] { new PointF (718.24f, 146.90f), new PointF(721.06f, 142.46f), new PointF(724.38f, 145.27f), new PointF(721.25f, 150.07f) },
                new PointF[4] { new PointF (701.23f, 169.09f), new PointF(725.42f, 146.16f), new PointF(730.39f, 150.36f), new PointF(703.40f, 176.25f) },
                new PointF[4] { new PointF (721.86f, 141.05f), new PointF(729.50f, 116.64f), new PointF(731.43f, 116.70f), new PointF(723.65f, 142.57f) },
                new PointF[4] { new PointF (724.45f, 143.25f), new PointF(732.25f, 116.72f), new PointF(738.73f, 116.92f), new PointF(731.18f, 148.95f) },
                new PointF[4] { new PointF (685.45f, 173.13f), new PointF(693.84f, 170.15f), new PointF(694.65f, 177.28f), new PointF(686.09f, 180.39f) },
                new PointF[4] { new PointF (655.93f, 71.53f), new PointF(640.17f, 86.63f), new PointF(629.79f, 80.02f), new PointF(649.91f, 63.27f) },
            },
        };

        private static Dictionary<string, Ball> balls = new[]
        {
            new Ball { Name = "Ball 1", PixelCenter = new PointF(111.78f, 166.22f), PixelSize = new SizeF(175.02f, 176.32f), Degrees = -179.50f },
            new Ball { Name = "Ball 9a", PixelCenter = new PointF(326.64f, 247.57f), PixelSize = new SizeF(173.11f, 176.89f), Degrees = -179.50f },
            new Ball { Name = "Ball 9b", PixelCenter = new PointF(337.32f, 267.88f), PixelSize = new SizeF(173.11f, 176.89f), Degrees = -179.50f },
            new Ball { Name = "Ball 9c", PixelCenter = new PointF(340.62f, 297.17f), PixelSize = new SizeF(173.11f, 176.89f), Degrees = -179.50f },
            new Ball { Name = "Ball 8a", PixelCenter = new PointF(513.15f, 187.86f), PixelSize = new SizeF(171.81f, 176.67f), Degrees = 167.00f },
            new Ball { Name = "Ball 8b", PixelCenter = new PointF(495.39f, 193.06f), PixelSize = new SizeF(171.81f, 176.67f), Degrees = 167.00f },
            new Ball { Name = "Ball 4", PixelCenter = new PointF(675.32f, 122.76f), PixelSize = new SizeF(171.81f, 176.67f), Degrees = 167.00f },
            new Ball { Name = "Ball w", PixelCenter = new PointF(296.49f, 550.67f), PixelSize = new SizeF(177.64f, 177.64f), Degrees = -19.00f }
        }.ToDictionary(it => it.Name);

        private static Light[] lights = new Light[]
        {
            new Light
            {
                Spots = new Light.Spot[]
                {
                    new Light.Spot { Target = "Ball 1", PixelCenter = new PointF(78.7676348023617f, 197.045802607026f), PixelSize1 = new SizeF(3.75987462459764f, 5.14884893927504f), PixelSize2 = new SizeF(6.62400107264758f, 9.12188718706622f), Degrees = 40f },
                    new Light.Spot { Target = "Ball 4", PixelCenter = new PointF(645.372198995559f, 151.289343091178f), PixelSize1 = new SizeF(4.38803220663234f, 5.85486448787416f), PixelSize2 = new SizeF(6.98456806016931f, 8.64037551205804f), Degrees = 60f },
                }
            },

            new Light
            {
                Spots = new Light.Spot[]
                {
                    new Light.Spot { Target = "Ball 1", PixelCenter = new PointF(89.3994475476303f, 168.448015409621f), PixelSize1 = new SizeF(11.1730411793682f, 10.7433040518897f), PixelSize2 = new SizeF(13.9683802147044f, 13.8644370285125f), Degrees = 60f },
                    new Light.Spot { Target = "Ball 4", PixelCenter = new PointF(655.591287264545f, 125.473287391466f), PixelSize1 = new SizeF(10.0267102850775f, 9.31539894342744f), PixelSize2 = new SizeF(12.8326319992927f, 12.1194296474193f), Degrees = 62.1f },
                }
            },

            new Light
            {
                Spots = new Light.Spot[]
                {
                    new Light.Spot { Target = "Ball 1", PixelCenter = new PointF(120.313342317405f, 129.814829472171f), PixelSize1 = new SizeF(7.31716653922499f, 8.84106836875298f), PixelSize2 = new SizeF(9.9715291737796f, 11.56346130053f), Degrees = 75f },
                    new Light.Spot { Target = "Ball 4", PixelCenter = new PointF(683.08659880357f, 85.4726687694167f), PixelSize1 = new SizeF(8.2223635362004f, 10.2194616801775f), PixelSize2 = new SizeF(10.3714032542806f, 12.3594295796251f), Degrees = 80f },
                }
            }
        };

        // ColorRef areas are used for color calibration
        private static ColorRef[] colorRefs = new ColorRef[] {
            new ColorRef { PixelCenter = new PointF(573.58f, 456.55f), Radius = 56.42f }, // Lightest part of the cloth
            new ColorRef { PixelCenter = new PointF(577.27f, 226.89f), Radius = 24.94f }, // Darkest part of the cloth (ambient color)
            new ColorRef { PixelCenter = new PointF(86.20f, 287.82f), Radius = 35.91f }, // Cloth with shadow of lamp 1 (highest)
            new ColorRef { PixelCenter = new PointF(398.50f, 575.20f), Radius = 20.52f }, // Cloth with shadow of lamp 2
            new ColorRef { PixelCenter = new PointF(444.56f, 449.02f), Radius = 30.99f }, // Cloth with shadow of lamp 3 (lowest)
            new ColorRef { PixelCenter = new PointF(83.24f, 134.94f), Radius = 20.41f }, // Lightest part of Ball 1
            new ColorRef { PixelCenter = new PointF(709.32f, 99.04f), Radius = 20.41f }, // Lightest part of Ball 4
            new ColorRef { PixelCenter = new PointF(304.50f, 545.56f), Radius = 20.41f }, // Lightest part of Ball w
            new ColorRef { PixelCenter = new PointF(621.44f, 73.33f), Radius = 18.90f }, // Reflection of cloth in Ball 4
            new ColorRef { PixelCenter = new PointF(111.02f, 169.15f), Radius = 12.47f }, // White part of the number texture
            new ColorRef { PixelCenter = new PointF(132f, 159.13f), Radius = 6f }, // Black part of the number texture
        };

        private Bitmap picture;
        private Bitmap renderBitmap;
        private Bitmap previewBitmap;
        private Camera camera;
        private Camera viewCamera;
        private Entity[] entities;

        private Vector3[] pictureRect = new Vector3[4];
        private float pictureScale = 4f;
        private float pictureWidth, pictureHeight;
        private RectangleF previewRect = new RectangleF(523f, 106f, 118f, 90f);

        private void CopyTexture(int offset, Ball ball)
        {
            var bitmap = new Bitmap(textureSize, textureSize, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(CubeMapBox.Image, new Point(-offset * textureSize, 0));
            }
            ball.Texture = bitmap;
        }

        public Form1()
        {
            InitializeComponent();

            picture = RenderBox.Image as Bitmap;
            pictureWidth = picture.Width / pictureScale;
            pictureHeight = picture.Height / pictureScale;

            MeasureColorRefs();
            double ambient = Math.Min(Math.Min(
                colorRefs[1].Measured.R,
                colorRefs[1].Measured.G),
                colorRefs[1].Measured.B
            );
            ambient = 0.01;
            ambientColor = new Color3(ambient, ambient, ambient);

            CubeMapContextActiveCubeMap.SelectedIndex = 0;

            balls["Ball 1"].CubeMap = new Bitmap(256 * 6, 256, PixelFormat.Format24bppRgb);
            balls["Ball 1"].SphereMap = new Bitmap(256, 256, PixelFormat.Format24bppRgb);
            balls["Ball 1"].DiffuseColor = colorRefs[5].Measured - ambientColor;
            balls["Ball 1"].Number = numbers[0];
            balls["Ball 1"].Boxes = boxes[0];

            balls["Ball 4"].CubeMap = new Bitmap(256 * 6, 256, PixelFormat.Format24bppRgb);
            balls["Ball 4"].SphereMap = new Bitmap(256, 256, PixelFormat.Format24bppRgb);
            balls["Ball 4"].DiffuseColor = colorRefs[6].Measured - ambientColor;
            balls["Ball 4"].Number = numbers[1];
            balls["Ball 4"].Boxes = boxes[1];

            balls["Ball 8a"].Number = numbers[2];
            balls["Ball 8b"].Number = numbers[3];
            balls["Ball 8b"].SphereMap = new Bitmap(256, 256, PixelFormat.Format24bppRgb);

            balls["Ball w"].DiffuseColor = Color3.FromColor(Color.FromArgb(234, 246, 163));

            renderBitmap = new Bitmap((int)pictureWidth, (int)pictureHeight, PixelFormat.Format24bppRgb);

            CopyTexture(0, balls["Ball 1"]);
            CopyTexture(1, balls["Ball 4"]);
            CopyTexture(2, balls["Ball 8a"]);
            balls["Ball 8b"].Texture = balls["Ball 8a"].Texture;
            CopyTexture(3, balls["Ball 9a"]);
            balls["Ball 9b"].Texture = balls["Ball 9a"].Texture;
            balls["Ball 9c"].Texture = balls["Ball 9c"].Texture;

            RenderBox.Image = null;
            CubeMapBox.Image = null;

            var felt = new Plane
            {
                Center = new Vector3(0.0, 0.0, 0.0),
                Normal = new Vector3(0.0, 0.0, 1.0),
                DiffuseColor = colorRefs[0].Measured - ambientColor
            };

            entities = balls.Values.Concat(new Entity[] { felt }).ToArray();

            this.MouseWheel += Form1_MouseWheel;

            this.camera = new Camera() { ApertureH = 7.4, ApertureV = 6.3 };
            this.viewCamera = this.camera.Clone();

            OffsetXSetter.Value = 0.0;
            OffsetYSetter.Value = 0.0;
            CamRotationSetter.Value = 35.2;
            CamDistSetter.Value = 33.7;

            ViewRotation1Setter.Value = -50.0;
            ViewRotation2Setter.Value = 100.0;
            ViewDistanceSetter.Value = 600.0;

            CalcScene();
        }

        private PointF CoordToPixel(Vector2 coord)
        {
            return new PointF(
                (float)(coord.X + 1.0) * pictureWidth * 0.5f,
                (float)(1.0 - coord.Y) * pictureHeight * 0.5f
            );
        }

        private Vector2 PixelToCoord(PointF pixel)
        {
            return new Vector2(
                pixel.X * 2.0 / pictureWidth - 1.0,
                1.0 - pixel.Y * 2.0 / pictureHeight
            );
        }

        private Point DirToCubeMap(Vector3 dir, out int plane)
        {
            plane =
                Math.Abs(dir.X) > Math.Abs(dir.Y) && Math.Abs(dir.X) > Math.Abs(dir.Z) ? (dir.X > 0 ? 2 : 4) :
                Math.Abs(dir.Y) > Math.Abs(dir.Z) ? (dir.Y > 0 ? 3 : 1) : (dir.Z > 0 ? 0 : 5);

            double planeX, planeY, k;
            switch (plane)
            {
                case 0:
                    k = 1.0 / dir.Z;
                    planeX = dir.X * k;
                    planeY = dir.Y * k;
                    break;
                case 1:
                    k = -1.0 / dir.Y;
                    planeX = dir.X * k;
                    planeY = dir.Z * k;
                    break;
                case 2:
                    k = 1.0 / dir.X;
                    planeX = dir.Y * k;
                    planeY = dir.Z * k;
                    break;
                case 3:
                    k = 1.0 / dir.Y;
                    planeX = -dir.X * k;
                    planeY = dir.Z * k;
                    break;
                case 4:
                    k = -1.0 / dir.X;
                    planeX = -dir.Y * k;
                    planeY = dir.Z * k;
                    break;
                default:    // 5
                    k = -1.0 / dir.Z;
                    planeX = dir.X * k;
                    planeY = -dir.Y * k;
                    break;
            }
            int px = (int)(planeX * 128.0 + 128.0);
            int py = (int)(128.0 - planeY * 128.0);
            if (px < 0) px = 0;
            if (py < 0) py = 0;
            if (px > 255) px = 255;
            if (py > 255) py = 255;
            return new Point(px, py);
        }

        private void RenderBox_Paint(object sender, PaintEventArgs e)
        {
            var pens = new Pen[] { Pens.Red, Pens.Green, Pens.Yellow, Pens.Purple, Pens.Red, Pens.Green, Pens.Yellow };
            for (int n = 0; n < pens.Length; n++)
                pens[n] = new Pen(pens[n].Color, 0.1f);

            Vector3 p1 = default(Vector3), p2 = default(Vector3);

            Camera renderCamera = ViewEnabledCheckbox.Checked ? viewCamera : this.camera;

            if (!ViewEnabledCheckbox.Checked)
            {
                e.Graphics.TranslateTransform(RenderBox.Offset.X, RenderBox.Offset.Y);
                e.Graphics.ScaleTransform(RenderBox.Zoom, RenderBox.Zoom);

                e.Graphics.DrawImage(picture, 0f, 0f, pictureWidth, pictureHeight);
                if (ViewRenderingCheckBox.Checked)
                {
                    e.Graphics.DrawImage(renderBitmap, 0f, 0f);
                }

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                if (ViewWireframeCheckBox.Checked)
                {
                    // Draw boxes
                    for (int ballNr = 0; ballNr < boxes.Length; ballNr++)
                    {
                        for (int boxNr = 0; boxNr < boxes[ballNr].Length; boxNr++)
                        {
                            for (int cornerNr = 0; cornerNr < 4; cornerNr++)
                            {
                                e.Graphics.DrawEllipse(pens[cornerNr], boxes[ballNr][boxNr][cornerNr].X - 0.1f, boxes[ballNr][boxNr][cornerNr].Y - 0.1f, 0.2f, 0.2f);
                            }
                        }
                    }

                    // Draw lights
                    var saveTransform = e.Graphics.Transform;
                    for (int lightNr = 0; lightNr < lights.Length; lightNr++)
                    {
                        var light = lights[lightNr];
                        for (int ballNr = 0; ballNr < light.Spots.Length; ballNr++)
                        {
                            var spot = light.Spots[ballNr];
                            e.Graphics.TranslateTransform(spot.PixelCenter.X, spot.PixelCenter.Y);
                            e.Graphics.RotateTransform(-spot.Degrees);
                            e.Graphics.DrawEllipse(pens[lightNr], -spot.PixelSize1.Width * 0.5f, -spot.PixelSize1.Height * 0.5f, spot.PixelSize1.Width, spot.PixelSize1.Height);
                            e.Graphics.DrawEllipse(pens[lightNr], -spot.PixelSize2.Width * 0.5f, -spot.PixelSize2.Height * 0.5f, spot.PixelSize2.Width, spot.PixelSize2.Height);
                            e.Graphics.Transform = saveTransform;
                        }
                    }

                    // Draw measured ball outline
                    foreach (var ball in balls.Values)
                    {
                        PointF pp1 = default(PointF);
                        foreach (var pp2 in ball.Ellipse.GetOutline(90))
                        {
                            // Using measured ellipse
                            if (pp1 != default(PointF))
                                e.Graphics.DrawLine(pens[0], pp1, pp2);
                            pp1 = pp2;
                        }
                    }

                    // Draw numbers
                    foreach (var ball in balls.Values)
                    {
                        var number = ball.Number;
                        if (number != null)
                        {
                            PointF pp1 = default(PointF);
                            foreach (var pp2 in number.Ellipse.GetOutline(32))
                            {
                                if (pp1 != default(PointF))
                                    e.Graphics.DrawLine(pens[0], pp1, pp2);
                                pp1 = pp2;
                            }
                            e.Graphics.DrawLine(pens[0], number.OrientStart, number.OrientEnd);
                        }
                    }
                }

                // Draw ColorRefs
                if (ViewColorRefsCheckBox.Checked)
                {
                    for (int colorRefNr = 0; colorRefNr < colorRefs.Length; colorRefNr++)
                    {
                        var colorRef = colorRefs[colorRefNr];
                        using (var brush = new SolidBrush(colorRef.Measured.ToColor()))
                        {
                            e.Graphics.FillEllipse(brush,
                                colorRef.PixelCenter.X - colorRef.Radius * 0.5f,
                                colorRef.PixelCenter.Y - colorRef.Radius * 0.5f,
                                colorRef.Radius,
                                colorRef.Radius
                            );
                        }
                        e.Graphics.DrawEllipse(Pens.LightCyan,
                            colorRef.PixelCenter.X - colorRef.Radius * 0.5f,
                            colorRef.PixelCenter.Y - colorRef.Radius * 0.5f,
                            colorRef.Radius,
                            colorRef.Radius
                        );
                        var size = e.Graphics.MeasureString(colorRefNr.ToString(), this.Font);
                        e.Graphics.DrawString(colorRefNr.ToString(), this.Font, Brushes.LightCyan, colorRef.PixelCenter.X - size.Width * 0.5f, colorRef.PixelCenter.Y - size.Height * 0.5f);
                    }
                }
            }

            if (ViewWireframeCheckBox.Checked)
            {
                // Draw border
                for (int n = 0; n < 4; n++)
                {
                    p1 = pictureRect[n % 4];
                    p2 = pictureRect[(n + 1) % 4];
                    e.Graphics.DrawLine(Pens.Red, CoordToPixel(renderCamera.VertexToCoord(p1)), CoordToPixel(renderCamera.VertexToCoord(p2)));
                }

                // Draw grid
                using (var pen = new Pen(Color.FromArgb(50, Color.Yellow)))
                {
                    for (int x = -20; x < 20; x++)
                        for (int y = -20; y < 21; y++)
                        {
                            p1 = new Vector3(x, y, 0.0);
                            p2 = new Vector3(x + 1, y, 0.0);
                            if (renderCamera.IsVertexVisible(p1) && renderCamera.IsVertexVisible(p2))
                                e.Graphics.DrawLine(pen, CoordToPixel(renderCamera.VertexToCoord(p1)), CoordToPixel(renderCamera.VertexToCoord(p2)));
                            p1 = new Vector3(y, x, 0.0);
                            p2 = new Vector3(y, x + 1, 0.0);
                            if (renderCamera.IsVertexVisible(p1) && renderCamera.IsVertexVisible(p2))
                                e.Graphics.DrawLine(pen, CoordToPixel(renderCamera.VertexToCoord(p1)), CoordToPixel(renderCamera.VertexToCoord(p2)));
                        }
                }

                // Draw balls
                using (var pen = new Pen(Color.Orange, 0.1f))
                {
                    foreach (var ball in balls.Values)
                    {
                        for (int n = -90; n < 90; n += 10)
                        {
                            double radius = Math.Cos(n * Math.PI / 180.0);
                            p1 = default(Vector3);
                            for (int m = 0; m < 90; m++)
                            {
                                p2 = ball.Center + new Vector3(
                                   radius * Math.Cos(m * Math.PI / 45.0),
                                   radius * Math.Sin(m * Math.PI / 45.0),
                                   Math.Sin(n * Math.PI / 180.0));
                                if (m > 0)
                                    e.Graphics.DrawLine(pen, CoordToPixel(renderCamera.VertexToCoord(p1)), CoordToPixel(renderCamera.VertexToCoord(p2)));
                                p1 = p2;
                            }
                        }

                        for (int m = 0; m < 16; m++)
                        {
                            p1 = default(Vector3);
                            for (int n = -90; n < 90; n += 10)
                            {
                                p2 = ball.Center + new Vector3(
                                    Math.Cos(m * Math.PI / 8.0) * Math.Cos(n * Math.PI / 180.0),
                                    Math.Sin(m * Math.PI / 8.0) * Math.Cos(n * Math.PI / 180.0),
                                    Math.Sin(n * Math.PI / 180.0));
                                if (n > -90)
                                    e.Graphics.DrawLine(pen, CoordToPixel(renderCamera.VertexToCoord(p1)), CoordToPixel(renderCamera.VertexToCoord(p2)));
                                p1 = p2;
                            }
                        }

                    }
                }

                // Draw ball outline
                using (var pen = new Pen(Color.LightCyan, 0.4f))
                {
                    foreach (var ball in balls.Values)
                    {
                        Vector3 va = camera.From - ball.Center;
                        Vector3 vb = Vector3.Cross(va, camera.Hor).Normalize();
                        Vector3 vc = Vector3.Cross(vb, va).Normalize();
                        for (int n = 0; n < 90; n++)
                        {
                            double f1 = Math.Cos(n * Math.PI / 45.0);
                            double f2 = Math.Sin(n * Math.PI / 45.0);
                            p2 = ball.Center + f1 * vb + f2 * vc;
                            if (n > 0)
                                e.Graphics.DrawLine(pen, CoordToPixel(renderCamera.VertexToCoord(p1)), CoordToPixel(renderCamera.VertexToCoord(p2)));
                            p1 = p2;
                        }
                    }
                }

                // Draw lines from balls to reflected lights
                for (int lightNr = 0; lightNr < lights.Length; lightNr++)
                {
                    var light = lights[lightNr];
                    using (var pen = new Pen(Color.FromArgb(200, pens[lightNr].Color), 0.5f))
                    {
                        foreach (var spot in light.Spots)
                        {
                            try
                            {
                                for (int n = 0; n < 100; n++)
                                {
                                    p1 = spot.ReflectedRay.Origin + spot.ReflectedRay.Direction * n * 1.0;
                                    p2 = spot.ReflectedRay.Origin + spot.ReflectedRay.Direction * (n + 1) * 1.0;
                                    if (renderCamera.IsVertexVisible(p1) && renderCamera.IsVertexVisible(p2))
                                        e.Graphics.DrawLine(pen,
                                                CoordToPixel(renderCamera.VertexToCoord(p1)),
                                                CoordToPixel(renderCamera.VertexToCoord(p2))
                                        );
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                }

                // Draw lights
                for (int lightNr = 0; lightNr < lights.Length; lightNr++)
                {
                    var light = lights[lightNr];
                    PointF lightPix1, lightPix2;
                    if (renderCamera.IsVertexVisible(light.Center))
                    {
                        lightPix1 = CoordToPixel(renderCamera.VertexToCoord(light.Center));
                        e.Graphics.DrawEllipse(pens[lightNr], lightPix1.X - 2f, lightPix1.Y - 2f, 4f, 4f);

                        if (renderCamera.IsVertexVisible(lights[(lightNr + 1) % lights.Length].Center))
                        {
                            lightPix1 = CoordToPixel(renderCamera.VertexToCoord(lights[lightNr].Center));
                            lightPix2 = CoordToPixel(renderCamera.VertexToCoord(lights[(lightNr + 1) % lights.Length].Center));
                            e.Graphics.DrawLine(pens[6], lightPix1, lightPix2);
                        }
                    }

                    if (ViewEnabledCheckbox.Checked)
                    {
                        foreach (var spot in light.Spots)
                        {
                            Vector3 va = light.Center - spot.ReflectedRay.Origin;
                            Vector3 vb = Vector3.Cross(va, new Vector3(1, 0, 0)).Normalize();
                            Vector3 vc = Vector3.Cross(vb, va).Normalize();
                            for (int m = 0; m < 2; m++)
                            {
                                double radius = m == 0 ? light.Radius1 : light.Radius2;
                                PointF pp1 = default(PointF);
                                for (int n = 0; n < 90; n++)
                                {
                                    double f1 = radius * Math.Cos(n * Math.PI / 45.0);
                                    double f2 = radius * Math.Sin(n * Math.PI / 45.0);
                                    Vector3 v = light.Center + f1 * vb + f2 * vc;
                                    PointF pp2 = CoordToPixel(renderCamera.VertexToCoord(v));
                                    if (n > 0)
                                        e.Graphics.DrawLine(pens[lightNr], pp1, pp2);
                                    pp1 = pp2;
                                }
                            }
                        }
                    }
                }


                // Draw numbers
                foreach (var ball in balls.Values)
                {
                    var number = ball.Number;
                    if (number != null)
                    {
                        PointF pp1 = default(PointF);
                        for (int n = 0; n <= 90; n++)
                        {
                            double angle1 = 0.5 * (ball.MinAngle1 + ball.MaxAngle1) + 0.5 * (ball.MaxAngle1 - ball.MinAngle1) * Math.Cos(n * Math.PI / 45.0);
                            double angle2 = 0.5 * (ball.MinAngle2 + ball.MaxAngle2) + 0.5 * (ball.MaxAngle2 - ball.MinAngle2) * Math.Sin(n * Math.PI / 45.0);
                            Vector3 v = new Vector3(
                                Math.Cos(angle1) * Math.Cos(angle2),
                                Math.Sin(angle1) * Math.Cos(angle2),
                                Math.Sin(angle2));
                            v = v * ball.TextureToWorld + ball.Center;
                            PointF pp2 = CoordToPixel(renderCamera.VertexToCoord(v));
                            if (n > 0)
                                e.Graphics.DrawLine(pens[4], pp1, pp2);
                            pp1 = pp2;
                        }
                    }
                }

                // Draw camera
                if (ViewEnabledCheckbox.Checked)
                {
                    var cp1 = CoordToPixel(renderCamera.VertexToCoord(camera.From + 10.0 * (camera.Look - camera.Hor - camera.Ver).Normalize()));
                    var cp2 = CoordToPixel(renderCamera.VertexToCoord(camera.From + 10.0 * (camera.Look + camera.Hor - camera.Ver).Normalize()));
                    var cp3 = CoordToPixel(renderCamera.VertexToCoord(camera.From + 10.0 * (camera.Look + camera.Hor + camera.Ver).Normalize()));
                    var cp4 = CoordToPixel(renderCamera.VertexToCoord(camera.From + 10.0 * (camera.Look - camera.Hor + camera.Ver).Normalize()));
                    var cp5 = CoordToPixel(renderCamera.VertexToCoord(camera.From));
                    e.Graphics.DrawLine(pens[2], cp1, cp2);
                    e.Graphics.DrawLine(pens[2], cp2, cp3);
                    e.Graphics.DrawLine(pens[2], cp3, cp4);
                    e.Graphics.DrawLine(pens[2], cp4, cp1);
                    e.Graphics.DrawLine(pens[2], cp1, cp5);
                    e.Graphics.DrawLine(pens[2], cp2, cp5);
                    e.Graphics.DrawLine(pens[2], cp3, cp5);
                    e.Graphics.DrawLine(pens[2], cp4, cp5);
                }
            }

            if (!ViewEnabledCheckbox.Checked)
            {
                e.Graphics.ResetTransform();
            }

            for (int n = 0; n < pens.Length; n++)
                pens[n].Dispose();
        }

        private void CubeMapBox_Paint(object sender, PaintEventArgs e)
        {
            var pens = new Pen[] { Pens.Red, Pens.Green, Pens.Yellow, Pens.Purple, Pens.PowderBlue, Pens.Cyan, Pens.Wheat, Pens.Azure };

            e.Graphics.TranslateTransform(CubeMapBox.Offset.X, CubeMapBox.Offset.Y);
            e.Graphics.ScaleTransform(CubeMapBox.Zoom, CubeMapBox.Zoom);

            Ball selectedBall = balls[CubeMapContextActiveCubeMap.SelectedItem.ToString()];
            if (selectedBall.SphereMap != null)
                e.Graphics.DrawImage(selectedBall.SphereMap, 0f, 0f);
            if (selectedBall.CubeMap != null)
                e.Graphics.DrawImage(selectedBall.CubeMap, 300f, 0f);

            // Draw lines from balls to reflected boxes
            foreach (var ball in balls.Values)
            {
                if (ball.Boxes != null)
                {
                    for (int boxNr = 0; boxNr < ball.Boxes.Length; boxNr++)
                    {
                        using (var pen = new Pen(Color.FromArgb(150, pens[boxNr].Color), 1.0f))
                        {
                            Point p1 = default(Point), p2 = default(Point);
                            for (int cornerNr = 0; cornerNr <= 4; cornerNr++)
                            {
                                // Ray through a reflection (on the ball) of the corner of a box, seen from the camera
                                var ray = this.camera.CoordToRay(PixelToCoord(ball.Boxes[boxNr][cornerNr % 4]));

                                var intsec = ball.GetClosestIntersection(ray, IntersectionMode.PositionAndNormal);
                                if (intsec.Hit)
                                {
                                    // Calculate mirror vector;
                                    double a = -Vector3.Dot(intsec.Normal, ray.Direction);
                                    Vector3 dir = ray.Direction + 2.0 * a * intsec.Normal;

                                    int plane;
                                    p2 = DirToCubeMap(dir, out plane);
                                    p2.X = p2.X + plane * 256 + 300;
                                }
                                if (cornerNr > 0)
                                    e.Graphics.DrawLine(pen, p1, p2);
                                p1 = p2;
                            }
                        }
                    }
                }
            }

            e.Graphics.ResetTransform();
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            var control = this.GetControlAt(e.Location) as ZoomablePictureBox;
            if (control != null)
            {
                Point p1 = this.PointToClient(default(Point));
                Point p2 = control.PointToClient(default(Point));
                Point p3 = new Point(e.X + p2.X - p1.X, e.Y + p2.Y - p1.Y);

                if (e.Delta < 0) control.ZoomOut(p3);
                if (e.Delta > 0) control.ZoomIn(p3);
            }
        }

        // Camera events

        private void CamRotationSetter_ValueChanged(object sender, EventArgs e)
        {
            double offsetx = OffsetXSetter.Value;
            double offsety = OffsetYSetter.Value;
            double dist = CamDistSetter.Value;

            camera.Angle = CamRotationSetter.Value;
            double cs = Math.Cos(camera.Angle * Math.PI / 180.0);
            double sn = Math.Cos(camera.Angle * Math.PI / 180.0);
            camera.From = new Vector3(offsetx * cs + offsety * sn, offsety * cs - offsetx * sn, 1).Normalize() * dist;

            CalcScene();
            RenderBox.Invalidate();
            CubeMapBox.Invalidate();
        }

        private void CamDistSetter_ValueChanged(object sender, EventArgs e)
        {
            camera.From = camera.At - Vector3.Normalize(camera.Look) * CamDistSetter.Value;
            CalcScene();
            RenderBox.Invalidate();
            CubeMapBox.Invalidate();
        }

        // View controls

        private void ViewSetter_ValueChanged(object sender, EventArgs e)
        {
            double ang1 = ViewRotation1Setter.Value * Math.PI / 180.0;
            double ang2 = ViewRotation2Setter.Value * Math.PI / 180.0;
            double dist = ViewDistanceSetter.Value;

            viewCamera.Angle = ViewRotation2Setter.Value;
            viewCamera.From = new Vector3(
                Math.Cos(ang2) * Math.Sin(ang1) * dist,
                Math.Sin(ang2) * Math.Sin(ang1) * dist,
                Math.Cos(ang1) * dist
            );
            RenderBox.Invalidate();
        }

        // Calculations
        private void CalcScene()
        {
            if (camera.ApertureH < 1.0) camera.ApertureH = 1.0;
            if (camera.ApertureV < 1.0) camera.ApertureV = 1.0;
            for (int iter1 = 0; iter1 < 4; iter1++)
            {
                foreach (var ball in balls.Values)
                {
                    Vector2 q1 = PixelToCoord(ball.PixelCenter);
                    Vector2 q2 = q1;
                    for (int iter2 = 0; iter2 < 10; iter2++)
                    {
                        var ray = camera.CoordToRay(q2);
                        double f = (1.0 - ray.Origin.Z) / ray.Direction.Z;
                        ball.Center = ray.Origin + ray.Direction * f;
                        double minx = 1E12, maxx = -1E12, miny = 1E12, maxy = -1E12;

                        // Measure the graphical extents of the rendered ball
                        Vector3 va = camera.From - ball.Center;
                        Vector3 vb = Vector3.Cross(va, camera.Hor).Normalize();
                        Vector3 vc = Vector3.Cross(vb, va).Normalize();
                        for (int n = 0; n < 90; n++)
                        {
                            double f1 = Math.Cos(n * Math.PI / 45.0);
                            double f2 = Math.Sin(n * Math.PI / 45.0);
                            Vector2 px = camera.VertexToCoord(ball.Center + f1 * vb + f2 * vc);
                            if (px.X < minx) minx = px.X;
                            if (px.X > maxx) maxx = px.X;
                            if (px.Y < miny) miny = px.Y;
                            if (px.Y > maxy) maxy = px.Y;
                        }
                        Vector2 q3 = new Vector2((minx + maxx) * 0.5f, (miny + maxy) * 0.5f);
                        q2 = new Vector2(q2.X + q1.X - q3.X, q2.Y + q1.Y - q3.Y);
                    }
                }

                // Calibrate ApertureH and ApertureV using Ball 1 and Ball 4
                double facH = 0.0;
                double facV = 0.0;
                double sum = 0.0;
                foreach (var ball in balls.Where(it => it.Key == "Ball 1" || it.Key == "Ball 4").Select(it => it.Value))
                {
                    double minx1 = 1E12, maxx1 = -1E12, miny1 = 1E12, maxy1 = -1E12;
                    double minx2 = 1E12, maxx2 = -1E12, miny2 = 1E12, maxy2 = -1E12;

                    // Measure the graphical extents of the measured ellipse
                    foreach (var point in ball.Ellipse.GetOutline(90))
                    {
                        Vector2 v = PixelToCoord(point);
                        if (v.X < minx1) minx1 = v.X;
                        if (v.X > maxx1) maxx1 = v.X;
                        if (v.Y < miny1) miny1 = v.Y;
                        if (v.Y > maxy1) maxy1 = v.Y;
                    }

                    // Measure the graphical extents of the rendered ball
                    Vector3 va = camera.From - ball.Center;
                    Vector3 vb = Vector3.Cross(va, camera.Hor).Normalize();
                    Vector3 vc = Vector3.Cross(vb, va).Normalize();
                    for (int n = 0; n < 90; n++)
                    {
                        double f1 = Math.Cos(n * Math.PI / 45.0);
                        double f2 = Math.Sin(n * Math.PI / 45.0);
                        Vector2 v = camera.VertexToCoord(ball.Center + f1 * vb + f2 * vc);
                        if (v.X < minx2) minx2 = v.X;
                        if (v.X > maxx2) maxx2 = v.X;
                        if (v.Y < miny2) miny2 = v.Y;
                        if (v.Y > maxy2) maxy2 = v.Y;
                    }
                    facH += (maxx2 - minx2) / (maxx1 - minx1);
                    facV += (maxy2 - miny2) / (maxy1 - miny1);
                    sum += 1.0;
                }
                if (sum != 0.0)
                {
                    camera.ApertureH = camera.ApertureH * facH / sum;
                    camera.ApertureV = camera.ApertureV * facV / sum;
                }
            }
            ApertureHLabel.Text = camera.ApertureH.ToString("0.00°");
            ApertureVLabel.Text = camera.ApertureV.ToString("0.00°");

            // Calculate world coordinates of the bitmap
            for (int n = 0; n < 4; n++)
            {
                Vector2 p1 = PixelToCoord(new PointF(
                    n == 0 || n == 3 ? 0f : pictureWidth,
                    n == 0 || n == 1 ? 0f : pictureHeight
                ));
                var ray = camera.CoordToRay(p1);
                double f = (0.0 - ray.Origin.Z) / ray.Direction.Z;
                pictureRect[n] = ray.Origin + ray.Direction * f;
            }

            // Calculate lights
            // Determines the world coordinates of the 3 'lights' (reflections) on balls "1", "4" and "w"
            for (int lightNr = 0; lightNr < lights.Length; lightNr++)
            {
                var light = lights[lightNr];
                foreach (var spot in light.Spots)
                {
                    Ball ball = balls[spot.Target];
                    Vector3 sum = new Vector3();
                    foreach (var point in spot.InnerEllipse.GetOutline(16))
                    {
                        Vector2 v2 = PixelToCoord(point);
                        var intsec = ball.GetClosestIntersection(camera.CoordToRay(v2), IntersectionMode.PositionAndNormal);
                        if (intsec.Hit)
                        {
                            sum += intsec.Normal;
                        }
                    }
                    Ray reflectedRay = new Ray();
                    Vector3 normal = sum.Normalize();   // The average of all normals found in the loop
                    reflectedRay.Origin = ball.Center + normal;
                    Vector3 direction = (reflectedRay.Origin - camera.From).Normalize();
                    double a = -Vector3.Dot(normal, direction);
                    reflectedRay.Direction = direction + 2.0 * a * normal;
                    spot.ReflectedRay = reflectedRay;
                }
                light.Center = Ray.Closest(light.Spots.Select(it => it.ReflectedRay).ToArray());
            }

            // Calculate light spots
            // Determines the Radius1 and Radius2 of each light
            for (int lightNr = 0; lightNr < lights.Length; lightNr++)
            {
                var light = lights[lightNr];
                foreach (var spot in light.Spots)
                {
                    Ball ball = balls[spot.Target];
                    for (int m = 0; m < 2; m++)
                    {
                        Ellipse ellipse = m == 0 ? spot.InnerEllipse : spot.OuterEllipse;
                        double radiusSum = 0.0;
                        double sum = 0.0;
                        foreach (var point in ellipse.GetOutline(16))
                        {
                            var intsec = ball.GetClosestIntersection(camera.CoordToRay(PixelToCoord(point)), IntersectionMode.PositionAndNormal);
                            if (intsec.Hit)
                            {
                                Ray reflectedRay = new Ray
                                {
                                    Origin = intsec.Position
                                };
                                Vector3 direction = (reflectedRay.Origin - camera.From).Normalize();
                                double a = -Vector3.Dot(intsec.Normal, direction);
                                reflectedRay.Direction = direction + 2.0 * a * intsec.Normal;

                                Vector3 correctedDirection = (light.Center - spot.ReflectedRay.Origin).Normalize();
                                double k =
                                    Vector3.Dot(light.Center - reflectedRay.Origin, reflectedRay.Direction) /
                                    Vector3.Dot(reflectedRay.Direction, correctedDirection);
                                Vector3 v = reflectedRay.Origin + k * reflectedRay.Direction - light.Center;

                                radiusSum += v.Length;
                                sum += 1.0;
                            }
                        }
                        if (sum != 0.0)
                        {
                            if (m == 0)
                                spot.Radius1 = radiusSum / sum;
                            else
                                spot.Radius2 = radiusSum / sum;
                        }
                    }
                }
                light.Radius1 = light.Spots.Average(it => it.Radius1);
                light.Radius2 = light.Spots.Average(it => it.Radius2);
            }

            // Calculate center of ball Number texture
            foreach (var ball in balls.Values)
            {
                var number = ball.Number;
                if (number != null)
                {
                    Ray ray = default(Ray);
                    Intersection intsec = default(Intersection);

                    // Calculate center vertex of texture
                    Vector3 sum = default(Vector3);
                    foreach (var point in number.Ellipse.GetOutline(16))
                    {
                        // Calculate ray going through the ellipse points
                        Vector2 px = PixelToCoord(point);
                        ray = camera.CoordToRay(px);

                        // Calculate intersection with ball
                        intsec = ball.GetClosestIntersection(ray, IntersectionMode.PositionAndNormal);
                        sum += intsec.Normal;
                    }
                    Vector3 center = sum.Normalize();

                    // Calculate transformation matrix of texture
                    // - Rotation about Z and Y axes
                    var rot = Matrix4.RotateZ(-Math.Atan2(center.Y, center.X));
                    var v1 = center * rot;
                    rot = rot * Matrix4.RotateY(Math.Atan2(v1.Z, v1.X));

                    // - Orientation of the texture by using the captured OrientStart and OrientEnd
                    Ray rayStart = camera.CoordToRay(PixelToCoord(number.OrientStart));
                    var intsecStart = ball.GetClosestIntersection(rayStart, IntersectionMode.PositionAndNormal);
                    Ray rayEnd = camera.CoordToRay(PixelToCoord(number.OrientEnd));
                    var intsecEnd = ball.GetClosestIntersection(rayEnd, IntersectionMode.PositionAndNormal);
                    var a = intsecEnd.Normal * rot - intsecStart.Normal * rot;
                    double rrot = Math.Atan2(a.Y, a.Z) + Math.PI;

                    // - World <-> Texture transformation matrices
                    ball.WorldToTexture = rot * Matrix4.RotateX(rrot);
                    ball.TextureToWorld = Matrix4.AffineInvert(ball.WorldToTexture);

                    // Calculate angle range of texture
                    double minAngle1 = 1E12, maxAngle1 = -1E12;
                    double minAngle2 = 1E12, maxAngle2 = -1E12;
                    foreach (var point in number.Ellipse.GetOutline(90))
                    {
                        // Calculate ray going through the ellipse points
                        Vector2 px = PixelToCoord(point);
                        ray = camera.CoordToRay(px);

                        // Calculate intersection with ball
                        intsec = ball.GetClosestIntersection(ray, IntersectionMode.PositionAndNormal);
                        Vector3 transformedNormal = intsec.Normal * ball.WorldToTexture;

                        // After transformation, texture center should sit at angle1 = 0.0° and angle2 = 0.0°
                        double angle1 = Math.Atan2(transformedNormal.Y, transformedNormal.X);
                        double angle2 = Math.Atan2(transformedNormal.Z, new Vector2(transformedNormal.X, transformedNormal.Y).Length);
                        if (angle1 < minAngle1) minAngle1 = angle1;
                        if (angle1 > maxAngle1) maxAngle1 = angle1;
                        if (angle2 < minAngle2) minAngle2 = angle2;
                        if (angle2 > maxAngle2) maxAngle2 = angle2;
                        sum += intsec.Normal;
                    }
                    ball.MinAngle1 = minAngle1;
                    ball.MaxAngle1 = maxAngle1;
                    ball.MinAngle2 = minAngle2;
                    ball.MaxAngle2 = maxAngle2;
                }
            }
            CalibrateColors();
        }

        private void CalibrateColors()
        {

            // Calibrate felt color
            var felt = entities.OfType<Plane>().First();
            for (int n = 0; n < 5; n++)
            {
                var actual = RenderPixel(PixelToCoord(colorRefs[0].PixelCenter));
                felt.DiffuseColor = felt.DiffuseColor + colorRefs[0].Measured - actual;
            }
            // Calibrate ball 1 color
            var ball = balls["Ball 1"];
            for (int n = 0; n < 5; n++)
            {
                var actual = RenderPixel(PixelToCoord(colorRefs[5].PixelCenter));
                ball.DiffuseColor = ball.DiffuseColor + colorRefs[5].Measured - actual;
            }
            // Calibrate ball 4 color
            ball = balls["Ball 4"];
            for (int n = 0; n < 5; n++)
            {
                var actual = RenderPixel(PixelToCoord(colorRefs[6].PixelCenter));
                ball.DiffuseColor = ball.DiffuseColor + colorRefs[6].Measured - actual;
            }
            // Calibrate white ball color
            ball = balls["Ball w"];
            for (int n = 0; n < 5; n++)
            {
                var actual = RenderPixel(PixelToCoord(colorRefs[7].PixelCenter));
                ball.DiffuseColor = ball.DiffuseColor + colorRefs[7].Measured - actual;
            }
            // Calibrate reflection
            //for (int n = 0; n < 5; n++)
            //{
            //    var actual = RenderPixel(PixelToCoord(colorRefs[8].PixelCenter));
            //    reflection = reflection + colorRefs[8].Measured.G - actual.G;
            //}
            // Calibrate number texture white
            for (int n = 0; n < 5; n++)
            {
                var actual = RenderPixel(PixelToCoord(colorRefs[9].PixelCenter));
                numberTextureWhiteColor = numberTextureWhiteColor + colorRefs[9].Measured - actual;
            }
            for (int n = 0; n < 5; n++)
            {
                var actual = RenderPixel(PixelToCoord(colorRefs[10].PixelCenter));
                numberTextureBlackColor = numberTextureBlackColor + colorRefs[10].Measured - actual;
            }
        }

        private void MeasureColorRefs()
        {
            Color3[] measured = new Color3[colorRefs.Length];

            // Reading from original picture
            var bmpDataRead = picture.LockBits(new Rectangle(0, 0, picture.Width, picture.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            for (int colorRefNr = 0; colorRefNr < colorRefs.Length; colorRefNr++)
            {
                var colorRef = colorRefs[colorRefNr];
                Color3 sum = new Color3();
                int count = 0;
                float radiusSq = colorRef.Radius * colorRef.Radius;
                unsafe
                {
                    for (int y = -(int)(colorRef.Radius); y < (int)(colorRef.Radius); y++)
                    {
                        byte* adrOffs = (byte*)(bmpDataRead.Scan0 + (int)((y + colorRef.PixelCenter.Y) * pictureScale) * bmpDataRead.Stride);
                        for (int x = -(int)(colorRef.Radius); x < (int)(colorRef.Radius); x++)
                        {
                            if (x * x + y * y < radiusSq)
                            {
                                byte* adrRead = adrOffs + (int)((x + colorRef.PixelCenter.X) * pictureScale) * 3;
                                byte b = *adrRead++;
                                byte g = *adrRead++;
                                byte r = *adrRead++;
                                sum += Color3.FromColor(Color.FromArgb(r, g, b));
                                if (colorRefNr == 10) Debug.Print($"{(int)(x + colorRef.PixelCenter.X):000}, {(int)(y + colorRef.PixelCenter.Y):000}: {r:000} {g:000} {b:000}");
                                count++;
                            }
                        }
                    }
                }
                colorRef.Measured = sum / count;
            }
            picture.UnlockBits(bmpDataRead);

            // Override these values, because we're using the high resolution inset which is not color accurate
            colorRefs[6].Measured = new Color3 (0.51963661140131789, 0.24298281474752118, 0.501691271103037);
            colorRefs[8].Measured = new Color3 (0.45770615095353273, 0.29091950935625455, 0.47271913331542786);
        }

        private void ViewEnabledCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            RenderBox.Invalidate();
        }

        // Cube maps
        private void CalculateCubeMaps(double precision)
        {
            foreach (var ball in balls.Values)
            {
                Bitmap bmp = ball.CubeMap;
                if (bmp != null)
                {
                    // Writing to cube map
                    var bmpDataWrite = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                    // Reading from original picture
                    var bmpDataReadOriginal = picture.LockBits(new Rectangle(0, 0, picture.Width, picture.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                    // Reading from rendered picture
                    var bmpDataReadRendered = renderBitmap.LockBits(new Rectangle(0, 0, renderBitmap.Width, renderBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                    // Find the graphical box containing the sphere
                    double minx = 1E12, maxx = -1E12, miny = 1E12, maxy = -1E12;
                    Vector3 va = camera.From - ball.Center;
                    Vector3 vb = Vector3.Cross(va, camera.Hor).Normalize();
                    Vector3 vc = Vector3.Cross(vb, va).Normalize();
                    for (int n = 0; n < 90; n++)
                    {
                        double f1 = Math.Cos(n * Math.PI / 45.0);
                        double f2 = Math.Sin(n * Math.PI / 45.0);
                        Vector2 px = camera.VertexToCoord(ball.Center + f1 * vb + f2 * vc);
                        if (px.X < minx) minx = px.X;
                        if (px.X > maxx) maxx = px.X;
                        if (px.Y < miny) miny = px.Y;
                        if (px.Y > maxy) maxy = px.Y;
                    }
                    Vector2 p1 = new Vector2(minx, miny);
                    Vector2 p2 = new Vector2(maxx, maxy);

                    // Ray trace the ball
                    double stepx = precision / pictureWidth * 0.5;
                    double stepy = precision / pictureHeight * 0.5;
                    for (double y = p1.Y; y <= p2.Y; y += stepy)
                        for (double x = p1.X; x <= p2.X; x += stepx)
                        {
                            Ray ray = camera.CoordToRay(new Vector2(x, y));
                            var intsec = ball.GetClosestIntersection(ray, IntersectionMode.PositionAndNormal);
                            if (intsec.Hit)
                            {
                                double a = -Vector3.Dot(intsec.Normal, ray.Direction);

                                // Conversion of direction vector to texture coords
                                Vector3 direction = ray.Direction + 2.0 * a * intsec.Normal;

                                int plane;
                                Point p = DirToCubeMap(direction, out plane);

                                unsafe
                                {
                                    var pixelCoord = CoordToPixel(new Vector2(x, y));
                                    // Reading from original picture
                                    byte* adrReadOriginal = (byte*)(bmpDataReadOriginal.Scan0 + (int)(pixelCoord.Y * pictureScale) * bmpDataReadOriginal.Stride + (int)(pixelCoord.X * pictureScale) * 3);
                                    // Reading from rendered picture
                                    byte* adrReadRendered = (byte*)(bmpDataReadRendered.Scan0 + (int)pixelCoord.Y * bmpDataReadRendered.Stride + (int)pixelCoord.X * 3);
                                    // Writing to cube map
                                    byte* adrWrite = (byte*)(bmpDataWrite.Scan0 + p.Y * bmpDataWrite.Stride + (p.X + plane * 256) * 3);

                                    int b1 = (*adrReadOriginal++ - *adrReadRendered++).Limit(0, 255);
                                    int g1 = (*adrReadOriginal++ - *adrReadRendered++).Limit(0, 255);
                                    int r1 = (*adrReadOriginal++ - *adrReadRendered++).Limit(0, 255);

                                    *adrWrite++ = (byte)b1;
                                    *adrWrite++ = (byte)g1;
                                    *adrWrite++ = (byte)r1;
                                }
                            }
                        }

                    renderBitmap.UnlockBits(bmpDataReadRendered);
                    picture.UnlockBits(bmpDataReadOriginal);
                    bmp.UnlockBits(bmpDataWrite);

                }
            }
            CubeMapBox.Invalidate();
        }

        // Sphere maps
        private void CalculateSphereMaps()
        {
            foreach (Ball ball in balls.Values)
            {
                Bitmap bmp = ball.SphereMap;
                Number number = ball.Number;
                if (number != null && bmp != null)
                {
                    // Writing to sphere map
                    var bmpDataWrite = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                    // Reading from original picture
                    var bmpDataRead = picture.LockBits(new Rectangle(0, 0, picture.Width, picture.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                    for (int y = 0; y < bmp.Height; y++)
                    {
                        for (int x = 0; x < bmp.Width; x++)
                        {
                            double angle1 = x * (ball.MaxAngle1 - ball.MinAngle1) / bmp.Width + ball.MinAngle1;
                            double angle2 = y * (ball.MinAngle2 - ball.MaxAngle2) / bmp.Height + ball.MaxAngle2;
                            Vector3 v = new Vector3(
                                Math.Cos(angle1) * Math.Cos(angle2),
                                Math.Sin(angle1) * Math.Cos(angle2),
                                Math.Sin(angle2));
                            v = v * ball.TextureToWorld;
                            unsafe
                            {
                                var pixelCoord = CoordToPixel(camera.VertexToCoord(ball.Center + v));
                                // Reading from original picture
                                byte* adrRead = (byte*)(bmpDataRead.Scan0 + (int)(pixelCoord.Y * pictureScale) * bmpDataRead.Stride + (int)(pixelCoord.X * pictureScale) * 3);
                                // Writing to sphere map
                                byte* adrWrite = (byte*)(bmpDataWrite.Scan0 + y * bmpDataWrite.Stride + x * 3);

                                *adrWrite++ = *adrRead++;
                                *adrWrite++ = *adrRead++;
                                *adrWrite++ = *adrRead++;
                            }
                        }
                    }
                    picture.UnlockBits(bmpDataRead);
                    bmp.UnlockBits(bmpDataWrite);
                }
            }
            CubeMapBox.Invalidate();
        }

        private void CubeMapContextActiveCubeMap_SelectedIndexChanged(object sender, EventArgs e)
        {
            CubeMapBox.Invalidate();
        }

        private void ViewRenderingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            RenderBox.Invalidate();
        }

        private void ViewWireframeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            RenderBox.Invalidate();
        }

        private void ViewColorRefsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            RenderBox.Invalidate();
        }

        private void SpheremapsRecalculateMenuItem_Click(object sender, EventArgs e)
        {
            CalculateSphereMaps();
        }

        private void SpheremapsCopyMenuItem_Click(object sender, EventArgs e)
        {
            if (CubeMapContextActiveCubeMap.SelectedIndex > -1)
            {
                Ball ball = balls[CubeMapContextActiveCubeMap.SelectedItem.ToString()];
                if (ball.SphereMap != null)
                    Clipboard.SetImage(ball.SphereMap);
            }
        }

        private void CubemapsRecalcCoarseMenuItem_Click(object sender, EventArgs e)
        {
            CalculateCubeMaps(2f);
        }

        private void CubemapsRecalcFineMenuItem_Click(object sender, EventArgs e)
        {
            CalculateCubeMaps(0.5f);
        }

        private void CubemapsCopyMenuItem_Click(object sender, EventArgs e)
        {
            if (CubeMapContextActiveCubeMap.SelectedIndex > -1)
            {
                Ball ball = balls[CubeMapContextActiveCubeMap.SelectedItem.ToString()];
                if (ball.CubeMap != null)
                    Clipboard.SetImage(ball.CubeMap);
            }
        }



        // Rendering 
        private class Line
        {
            public byte[] LineData { get; set; }

            public int Y { get; set; }
        }

        private CancellationTokenSource cts = null;

        private async void RenderButton_Click(object sender, EventArgs e)
        {
            if (cts != null)
            {
                // Stop a pending rendering
                cts.Cancel();
                cts = null;
            }
            else
            {
                // Start a new rendering
                ViewWireframeCheckBox.Checked = false;
                ViewRenderingCheckBox.Checked = true;
                IProgress<Line> progress = new Progress<Line>(line =>
                {
                    BitmapData bmpDataWrite = null;
                    try
                    {
                        bmpDataWrite = renderBitmap.LockBits(new Rectangle(0, line.Y, renderBitmap.Width, 1), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                        unsafe
                        {
                            byte* adrWrite = (byte*)(bmpDataWrite.Scan0);
                            for (int n = 0; n < line.LineData.Length; n++)
                            {
                                *adrWrite++ = line.LineData[n];
                            }
                        }
                    }
                    finally
                    {
                        renderBitmap.UnlockBits(bmpDataWrite);
                        RenderBox.Invalidate();
                    }
                });
                cts = new CancellationTokenSource();
                try
                {
                    using (var g = Graphics.FromImage(renderBitmap))
                    {
                        g.Clear(Color.Gray);
                    }
                    await Task.Run(() => Render(progress, cts.Token));
                }
                catch (OperationCanceledException)
                {
                }
            }
        }

        private void RenderBox_MouseClick(object sender, MouseEventArgs e)
        {
            Point p = new Point(
                (int)((e.X - RenderBox.Offset.X) / RenderBox.Zoom),
                (int)((e.Y - RenderBox.Offset.Y) / RenderBox.Zoom)
            );
            Vector2 coord = new Vector2(p.X * 2f / renderBitmap.Width - 1f, 1f - p.Y * 2f / renderBitmap.Height);
            if (coord.X >= -1.0 && coord.X <= 1.0 && coord.Y >= -1.0 && coord.Y <= 1.0)
            {
                Color3 col = RenderPixel(coord);
                RenderButton.BackColor = col.ToColor();
            }
            else
            {
                RenderButton.BackColor = SystemColors.Control;
            }
        }

        Vector2 Hammersley(int i, int numSamples)
        {
            uint b = (uint)i;
            b = (b << 16) | (b >> 16);
            b = ((b & 0x55555555u) << 1) | ((b & 0xAAAAAAAAu) >> 1);
            b = ((b & 0x33333333u) << 2) | ((b & 0xCCCCCCCCu) >> 2);
            b = ((b & 0x0F0F0F0Fu) << 4) | ((b & 0xF0F0F0F0u) >> 4);
            b = ((b & 0x00FF00FFu) << 8) | ((b & 0xFF00FF00u) >> 8);
            double radicalInverseVDC = b * 2.3283064365386963e-10;
            return new Vector2((double)i / numSamples, radicalInverseVDC);
        }

        private Color3 RenderPixel(Vector2 coord)
        {
            Ray ray = camera.CoordToRay(coord);
            return RenderRay(ray);
        }

        private Color3 RenderRay(Ray ray, int depth = 0)
        {
            Color3 col = default(Color3);
            Intersection closest = default(Intersection);
            foreach (var entity in entities)
            {
                var intsec = entity.GetClosestIntersection(ray, IntersectionMode.PositionAndNormal);
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
                Entity entity = closest.Entity;

                // Hack to have reflected the cloth not to much
                Color3 diffuseColor =
                    (entity is Plane) && depth > 0 ?
                    entity.DiffuseColor * 1.0 :
                    entity.DiffuseColor;

                // Calculate mirror ray
                double a = -Vector3.Dot(closest.Normal, ray.Direction);
                Ray mirrorRay = new Ray { Origin = closest.Position, Direction = ray.Direction + 2.0 * a * closest.Normal };

                // Get texture
                if (entity.Texture != null && entity.WorldToTexture.Valid)
                {
                    Vector2 p = entity.GetTextureCoordinates(closest);

                    if (p.Length < 1.0)
                    {
                        Point q = new Point((int)((p.X * 0.5 + 0.5) * textureSize), (int)((p.Y * 0.5 + 0.5) * textureSize));
                        double f = entity.Texture.GetPixel(q.X, q.Y).R / 255.0;
                        diffuseColor = numberTextureBlackColor + (numberTextureWhiteColor - numberTextureBlackColor) * f;
                    }
                }

                // Calculate lights
                Random rnd = new Random();
                for (int lightNr = 0; lightNr < lights.Length; lightNr++)
                {
                    var light = lights[lightNr];
                    int shadowHit = 0;
                    var lightVec1 = light.Center - closest.Position;

                    var lightVec2 = lightVec1.Normalize();
                    Vector3 hor = Vector3.Cross(new Vector3(0.0, 1.0, 0.0), lightVec2);
                    Vector3 ver = Vector3.Cross(lightVec2, hor);

                    int shadowSamples = 3;
                    for (int n = 0; n < shadowSamples; n++)
                    {
                        Vector2 v1 = Hammersley(n, shadowSamples) * light.Radius2 - new Vector2(light.Radius2 * 0.5, light.Radius2 * 0.5);

                        var lightVec3 = lightVec1 + hor * v1.X + ver * v1.Y;
                        double lightDist = lightVec3.Length;

                        Ray shadowRay = new Ray { Origin = closest.Position, Direction = lightVec3 / lightDist };

                        foreach (var shadowEntity in entities)
                        {
                            var intsec = shadowEntity.GetClosestIntersection(shadowRay, IntersectionMode.Hit, Intersection.MinDistance, lightDist);
                            if (intsec.Hit)
                            {
                                shadowHit++;
                                break;
                            }
                        }
                    }
                    double shadow = 1.0 - shadowHit / 16;

                    // Calculate specular highlight analyitically
                    double k =
                        Vector3.Dot(light.Center - mirrorRay.Origin, lightVec2) /
                        Vector3.Dot(mirrorRay.Direction, lightVec2);
                    Vector3 v = mirrorRay.Origin + k * mirrorRay.Direction - light.Center;
                    double specInt = k > Intersection.MinDistance && k < lightVec1.Length + light.Radius2 ? ((v.Length - light.Radius2) / (light.Radius1 - light.Radius2)).Limit(0.0, 1.0) : 0;

                    Ray lightRay = new Ray { Origin = closest.Position, Direction = lightVec2 };
                    double diffuseIntensity = Math.Max(0.0, Vector3.Dot(lightRay.Direction, closest.Normal)) * 0.333 * shadow;
                    double specularIntensity = specInt * shadow;
                    col += diffuseIntensity * light.Color * diffuseColor + specularIntensity * light.Color + ambientColor;

                    if (depth < 3 && !(entity is Plane))
                        col += reflection * RenderRay(mirrorRay, depth + 1);
                }
            }
            return col;
        }

        private void Render(IProgress<Line> progress, CancellationToken ct)
        {
            int width = renderBitmap.Width;
            int height = renderBitmap.Height;
            Camera camera = this.camera.Clone();
            Color3[] prevCornerData = new Color3[width];
            var halfPixel = new Vector2(1.0 / width, -1.0 / height);

            // -1: Start with an extra line for correct antialiasing
            for (int y = -1; y < height; y++)
            {
                int adr = 0;
                byte[] lineData = new byte[width * 3];
                Color3 topLeftColor = new Color3();
                Vector2 coord = new Vector2(-1 * 2.0 / width - 1.0, 1.0 - y * 2.0 / height);
                Color3 bottomLeftColor = RenderPixel(coord);
                for (int x = 0; x < width; x++)
                {
                    coord = new Vector2(x * 2.0 / width - 1.0, 1.0 - y * 2.0 / height);
                    Color3 centerColor = RenderPixel(coord);
                    Color3 bottomRightColor = RenderPixel(coord + halfPixel);
                    Color3 topRightColor = prevCornerData[x];

                    var resultColor = new Color3(
                        (centerColor.R * 2.0 + topLeftColor.R + topRightColor.R + bottomLeftColor.R + bottomRightColor.R) / 6.0,
                        (centerColor.G * 2.0 + topLeftColor.G + topRightColor.G + bottomLeftColor.G + bottomRightColor.G) / 6.0,
                        (centerColor.B * 2.0 + topLeftColor.B + topRightColor.B + bottomLeftColor.B + bottomRightColor.B) / 6.0
                    ).ToColor();

                    lineData[adr++] = (resultColor.B);
                    lineData[adr++] = (resultColor.G);
                    lineData[adr++] = (resultColor.R);

                    prevCornerData[x] = bottomRightColor;
                    topLeftColor = topRightColor;
                    bottomLeftColor = bottomRightColor;
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
