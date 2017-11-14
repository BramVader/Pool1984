using Pool1984.Exporters;
using Pool1984.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pool1984
{
    public partial class Form1 : Form
    {
        private static int textureSize = 512;
        private double ambient = 0.01;
        private double reflection = 0.3;

        private int nrSamplesX = 3;
        private int nrSamplesY = 3;

        private static Number[] numbers = new Number[]
        {
            new Number { TargetPosition = "Ball 1", PixelCenter = new PointF(124.72f, 175.53f), PixelSize = new SizeF(90.68f, 95.44f), Degrees = -24.00f, OrientStart = new PointF(127.38f, 201.46f), OrientEnd = new PointF(138.72f, 156.11f) },
            new Number { TargetPosition = "Ball 4a", PixelCenter = new PointF(641.06f, 154.74f), PixelSize = new SizeF(74.17f, 93.62f), Degrees = 41.30f, OrientStart = new PointF(653.94f, 144.58f), OrientEnd = new PointF(644.95f, 184.83f) },
            new Number { TargetPosition = "Ball 8a", PixelCenter = new PointF(486.89f, 205.26f), PixelSize = new SizeF(92.53f, 94.44f), Degrees = -4.80f, OrientStart = new PointF(498.23f, 189.74f), OrientEnd = new PointF(473.28f, 225.27f) },
            new Number { TargetPosition = "Ball 9c", PixelCenter = new PointF(291.46f, 214.52f), PixelSize = new SizeF(70.45f, 91.77f), Degrees = -53.00f, OrientStart = new PointF(297.54f, 198.44f), OrientEnd = new PointF(282.67f, 222.12f) },
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

        private static Dictionary<string, BallPosition> positions = new[]
        {
            new BallPosition { Name = "Ball 1", TargetBall = "Ball 1", PixelCenter = new PointF(111.78f, 166.22f), PixelSize = new SizeF(175.02f, 176.32f), Degrees = -179.50f },
            new BallPosition { Name = "Ball 9a", TargetBall = "Ball 9", PixelCenter = new PointF(338.73f, 296.04f), PixelSize = new SizeF(169.32f, 175.37f), Degrees = 0.00f },
            new BallPosition { Name = "Ball 9b", TargetBall = "Ball 9", PixelCenter = new PointF(342.34f, 278.31f), PixelSize = new SizeF(169.32f, 175.37f), Degrees = 0.00f },
            new BallPosition { Name = "Ball 9c", TargetBall = "Ball 9", PixelCenter = new PointF(325.05f, 246.97f), PixelSize = new SizeF(169.32f, 175.37f), Degrees = 0.00f },
            new BallPosition { Name = "Ball 8a", TargetBall = "Ball 8", PixelCenter = new PointF(495.39f, 191.44f), PixelSize = new SizeF(169.89f, 172.91f), Degrees = 0.00f },
            new BallPosition { Name = "Ball 8b", TargetBall = "Ball 8", PixelCenter = new PointF(513.25f, 178.75f), PixelSize = new SizeF(169.89f, 172.91f), Degrees = 0.00f },
            new BallPosition { Name = "Ball 4a", TargetBall = "Ball 4", PixelCenter = new PointF(676.31f, 123.41f), PixelSize = new SizeF(172.73f, 175.75f), Degrees = -179.70f },
            new BallPosition { Name = "Ball 4b", TargetBall = "Ball 4", PixelCenter = new PointF(689.79f, 118.62f), PixelSize = new SizeF(172.73f, 175.75f), Degrees = -179.70f },
            new BallPosition { Name = "Ball wa", TargetBall = "Ball w", PixelCenter = new PointF(295.17f, 550.50f), PixelSize = new SizeF(177.64f, 177.64f), Degrees = 0.00f },
            new BallPosition { Name = "Ball wb", TargetBall = "Ball w", PixelCenter = new PointF(287.61f, 548.14f), PixelSize = new SizeF(177.64f, 177.64f), Degrees = 0.00f },
        }.ToDictionary(it => it.Name);

        private static Light[] lights = new Light[]
        {
            new Light
            {
                Spots = new Light.Spot[]
                {
                    new Light.Spot { TargetPosition = "Ball 1", PixelCenter = new PointF(78.76f, 197.04f), PixelSize1 = new SizeF(3.75f, 5.14f), PixelSize2 = new SizeF(6.62f, 9.12f), Degrees = 40f },
                    new Light.Spot { TargetPosition = "Ball 4a", PixelCenter = new PointF(645.37f, 151.28f), PixelSize1 = new SizeF(4.38f, 5.85f), PixelSize2 = new SizeF(6.98f, 8.64f), Degrees = 60f },
                }
            },

            new Light
            {
                Spots = new Light.Spot[]
                {
                    new Light.Spot { TargetPosition = "Ball 1", PixelCenter = new PointF(89.39f, 168.44f), PixelSize1 = new SizeF(11.17f, 10.74f), PixelSize2 = new SizeF(13.96f, 13.86f), Degrees = 60f },
                    new Light.Spot { TargetPosition = "Ball 4a", PixelCenter = new PointF(655.59f, 125.47f), PixelSize1 = new SizeF(10.02f, 9.31f), PixelSize2 = new SizeF(12.83f, 12.11f), Degrees = 62.1f },
                }
            },

            new Light
            {
                Spots = new Light.Spot[]
                {
                    new Light.Spot { TargetPosition = "Ball 1", PixelCenter = new PointF(120.31f, 129.81f), PixelSize1 = new SizeF(7.31f, 8.84f), PixelSize2 = new SizeF(9.97f, 11.56f), Degrees = 75f },
                    new Light.Spot { TargetPosition = "Ball 4a", PixelCenter = new PointF(683.08f, 85.47f), PixelSize1 = new SizeF(8.22f, 10.21f), PixelSize2 = new SizeF(10.37f, 12.35f), Degrees = 80f },
                }
            }
        };

        // ColorRef areas are used for color calibration
        private static ColorRef[] colorRefs = new ColorRef[] {
            new ColorRef { PixelCenter = new PointF(573.58f, 456.55f), Radius = 56.42f }, // Lightest part of the cloth
            new ColorRef { PixelCenter = new PointF(609.13f, 214.47f), Radius = 24.94f }, // Darkest part of the cloth (ambient color)
            new ColorRef { PixelCenter = new PointF(86.20f, 287.82f), Radius = 35.91f }, // Cloth with shadow of lamp 1 (top)
            new ColorRef { PixelCenter = new PointF(398.50f, 575.20f), Radius = 20.52f }, // Cloth with shadow of lamp 2
            new ColorRef { PixelCenter = new PointF(444.56f, 449.02f), Radius = 30.99f }, // Cloth with shadow of lamp 3 (bottom)
            new ColorRef { PixelCenter = new PointF(105.66f, 119.70f), Radius = 20.41f }, // Lightest part of Ball 1
            new ColorRef { PixelCenter = new PointF(709.32f, 99.04f), Radius = 20.41f }, // Lightest part of Ball 4
            new ColorRef { PixelCenter = new PointF(304.50f, 545.56f), Radius = 20.41f }, // Lightest part of Ball w
            new ColorRef { PixelCenter = new PointF(621.44f, 73.33f), Radius = 18.90f }, // Reflection of cloth in Ball 4
            new ColorRef { PixelCenter = new PointF(111.02f, 169.15f), Radius = 12.47f }, // White part of the number texture
            new ColorRef { PixelCenter = new PointF(132f, 159.13f), Radius = 6f }, // Black part of the number texture
        };

        // From the paper:
        // Notice that the motion is not linear: the 9 ball changes direction abruptly in the middle of the
        // frame, the 8 ball moves only during the middle of the frame, and the 4 ball only starts to move 
        // near the end of the frame. The shadows on the table are sharper where the balls are closer to the 
        // table; this most apparent in the stationary 1 ball. The reflections of the billiard balls and
        // the room are motion blurred, as are the penumbras.
        private static Keyframe[] keyframes = new Keyframe[] {
            new Keyframe { StartPosition = positions["Ball 1"],  EndPosition = positions["Ball 1"],  StartTime = 0.0, EndTime = 1.0 },
            new Keyframe { StartPosition = positions["Ball 9a"], EndPosition = positions["Ball 9b"], StartTime = 0.0, EndTime = 0.5 },
            new Keyframe { StartPosition = positions["Ball 9b"], EndPosition = positions["Ball 9c"], StartTime = 0.5, EndTime = 1.0 },
            new Keyframe { StartPosition = positions["Ball 8a"], EndPosition = positions["Ball 8b"], StartTime = 0.3, EndTime = 0.7 },
            new Keyframe { StartPosition = positions["Ball 4a"], EndPosition = positions["Ball 4b"], StartTime = 0.5, EndTime = 1.0 },
            new Keyframe { StartPosition = positions["Ball wa"], EndPosition = positions["Ball wb"], StartTime = 0.0, EndTime = 1.0 }
        };

        private Bitmap picture;
        private Bitmap pictureDetail;

        private Bitmap renderBitmap;
        private CubeTexture cubeMap;

        private Model model;
        private Camera viewCamera;
        private Dictionary<string, Primitive> primitives;
        private Dictionary<string, Ball> balls;

        private Vector3[] pictureRect = new Vector3[4];
        private float pictureScale = 1f;
        private float pictureWidth, pictureHeight;

        private RectangleF pictureDetailRect = new RectangleF(650f, 56f, 96f, 77f);

        private Texture UnpackTexture(int offset)
        {
            return Texture.FromBitmap((Bitmap)CubeMapBox.Image, new Rectangle(offset * textureSize, 0, textureSize, textureSize));
        }

        public Form1()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            InitializeComponent();

            picture = RenderBox.Image as Bitmap;
            pictureWidth = picture.Width / pictureScale;
            pictureHeight = picture.Height / pictureScale;

            pictureDetail = RenderBox.ErrorImage as Bitmap;
            cubeMap = CubeTexture.FromBitmap(CubeMapBox.ErrorImage as Bitmap);

            CubeMapContextActiveCubeMap.SelectedIndex = 0;

            MeasureColorRefs();
            primitives = new Primitive[]
            {
                new Ball
                {
                    Name = "Ball 1",
                    DiffuseColor = colorRefs[5].Measured,
                    BandColor = colorRefs[5].Measured,
                    Texture = UnpackTexture(0),
                    Reflection = reflection
                },
                new Ball
                {
                    Name = "Ball 9",
                    DiffuseColor = Color3.FromColor(Color.FromArgb(234, 246, 163)),
                    BandColor = colorRefs[5].Measured,
                    Texture = UnpackTexture(3),
                    Reflection = reflection
                },
                new Ball
                {
                    Name = "Ball 8",
                    DiffuseColor = Color3.FromColor(Color.Black),
                    BandColor = Color3.FromColor(Color.Black),
                    Texture = UnpackTexture(2),
                    Reflection = reflection
                },
                new Ball
                {
                    Name = "Ball 4",
                    DiffuseColor = colorRefs[6].Measured,
                    BandColor = colorRefs[6].Measured,
                    Texture = UnpackTexture(1),
                    Reflection = reflection
                },
                new Ball
                {
                    Name = "Ball w",
                    DiffuseColor = Color3.FromColor(Color.FromArgb(234, 246, 163)),
                    BandColor = Color3.FromColor(Color.FromArgb(234, 246, 163)),
                    Reflection = reflection
                },
                new Plane
                {
                    Name = "Felt",
                    Center = new Vector3(0.0, 0.0, 0.0),
                    Normal = new Vector3(0.0, 0.0, 1.0),
                    DiffuseColor = colorRefs[0].Measured,
                    Texture = Texture.FromBitmap(Resources.Cloth)
                }
            }.ToDictionary(it => it.Name);
            balls = primitives.Values.OfType<Ball>().ToDictionary(it => it.Name);

            // Connect entities
            foreach (var number in numbers)
            {
                number.Position = positions[number.TargetPosition];
                balls[number.Position.TargetBall].Number = number;
            }

            foreach (var position in positions.Values)
            {
                position.Ball = balls[position.TargetBall];
            }

            // For obtaining original cube map texture
            positions["Ball 1"].Boxes = boxes[0];
            positions["Ball 1"].CubeMap = new Bitmap(textureSize * 4, textureSize, PixelFormat.Format24bppRgb);
            positions["Ball 4a"].Boxes = boxes[1];
            positions["Ball 4a"].CubeMap = new Bitmap(textureSize * 4, textureSize, PixelFormat.Format24bppRgb);

            // For obtaining original number texture
            balls["Ball 1"].SphereMap = new Bitmap(textureSize, textureSize, PixelFormat.Format24bppRgb);
            balls["Ball 9"].SphereMap = new Bitmap(textureSize, textureSize, PixelFormat.Format24bppRgb);
            balls["Ball 8"].SphereMap = new Bitmap(textureSize, textureSize, PixelFormat.Format24bppRgb);
            balls["Ball 4"].SphereMap = new Bitmap(textureSize, textureSize, PixelFormat.Format24bppRgb);

            renderBitmap = new Bitmap((int)pictureWidth, (int)pictureHeight, PixelFormat.Format24bppRgb);
            RenderBox.Image = null;
            RenderBox.ErrorImage = null;
            CubeMapBox.Image = null;
            CubeMapBox.ErrorImage = null;

            this.model = new Model
            {
                Camera = new Camera() { ApertureH = 7.4, ApertureV = 6.3 },
                CubeMap = cubeMap,
                AmbientColor = new Color3(ambient, ambient, ambient),
                Primitives = primitives.Values,
                Lights = lights
            };
            this.viewCamera = model.Camera.Clone();

            OffsetXSetter.Value = 0.0;
            OffsetYSetter.Value = 0.0;
            CamRotationSetter.Value = 35.2;
            CamDistSetter.Value = 33.7;

            ViewRotation1Setter.Value = -50.0;
            ViewRotation2Setter.Value = 100.0;
            ViewDistanceSetter.Value = 600.0;

            CalcScene();
            this.MouseWheel += Form1_MouseWheel;

            var exporters = Assembly.GetExecutingAssembly().GetTypes().Where(it => typeof(Exporter).IsAssignableFrom(it) && !it.IsAbstract);
            ExportersCombo.Format += (s, e) => { e.Value = (e.ListItem as Type).Name.Replace("Exporter", ""); };
            ExportersCombo.DataSource = exporters.ToList();

            //using (var ms = new MemoryStream())
            //{
            //    new ShaderToyExporter().Export(ms, model);
            //    string result = Encoding.UTF8.GetString(ms.ToArray());
            //}
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

        private void RenderBox_Paint(object sender, PaintEventArgs e)
        {
            var pens = new Pen[] { Pens.Red, Pens.Blue, Pens.Yellow, Pens.Purple, Pens.Red, Pens.Green, Pens.Yellow };
            for (int n = 0; n < pens.Length; n++)
                pens[n] = new Pen(pens[n].Color, 0.1f);

            Vector3 p1 = default(Vector3), p2 = default(Vector3);
            Camera renderCamera = ViewEnabledCheckbox.Checked ? viewCamera : this.model.Camera;

            double time = timeSetter.Value;

            if (!ViewEnabledCheckbox.Checked)
            {
                e.Graphics.TranslateTransform(RenderBox.Offset.X, RenderBox.Offset.Y);
                e.Graphics.ScaleTransform(RenderBox.Zoom, RenderBox.Zoom);

                // Draw either the rendered bitmap or the original + detail
                if (ViewRenderingCheckBox.Checked)
                {
                    e.Graphics.DrawImage(renderBitmap, 0f, 0f);
                }
                else
                {
                    e.Graphics.DrawImage(picture, 0f, 0f, pictureWidth, pictureHeight);
                    e.Graphics.DrawImage(pictureDetail, pictureDetailRect);
                }

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                if (ViewBoxesCheckbox.Checked)
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
                }

                if (ViewLightsCheckBox.Checked)
                {
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
                }

                if (ViewBallOutlineFlatCheckBox.Checked)
                {
                    // Draw measured ball outline
                    foreach (var position in positions.Values)
                    {
                        PointF pp1 = default(PointF);
                        foreach (var pp2 in position.Ellipse.GetOutline(90))
                        {
                            // Using measured ellipse
                            if (pp1 != default(PointF))
                                e.Graphics.DrawLine(pens[0], pp1, pp2);
                            pp1 = pp2;
                        }
                    }
                }

                if (ViewNumbersFlatCheckBox.Checked)
                {
                    // Draw numbers flat
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

            if (ViewGridCheckBox.Checked)
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
            }

            // Draw model.Camera
            if (ViewEnabledCheckbox.Checked)
            {
                var cp1 = CoordToPixel(renderCamera.VertexToCoord(model.Camera.From + 10.0 * (model.Camera.Look - model.Camera.Hor - model.Camera.Ver).Normalize()));
                var cp2 = CoordToPixel(renderCamera.VertexToCoord(model.Camera.From + 10.0 * (model.Camera.Look + model.Camera.Hor - model.Camera.Ver).Normalize()));
                var cp3 = CoordToPixel(renderCamera.VertexToCoord(model.Camera.From + 10.0 * (model.Camera.Look + model.Camera.Hor + model.Camera.Ver).Normalize()));
                var cp4 = CoordToPixel(renderCamera.VertexToCoord(model.Camera.From + 10.0 * (model.Camera.Look - model.Camera.Hor + model.Camera.Ver).Normalize()));
                var cp5 = CoordToPixel(renderCamera.VertexToCoord(model.Camera.From));
                e.Graphics.DrawLine(pens[2], cp1, cp2);
                e.Graphics.DrawLine(pens[2], cp2, cp3);
                e.Graphics.DrawLine(pens[2], cp3, cp4);
                e.Graphics.DrawLine(pens[2], cp4, cp1);
                e.Graphics.DrawLine(pens[2], cp1, cp5);
                e.Graphics.DrawLine(pens[2], cp2, cp5);
                e.Graphics.DrawLine(pens[2], cp3, cp5);
                e.Graphics.DrawLine(pens[2], cp4, cp5);
            }

            // Draw balls
            if (ViewBallsCheckBox.Checked)
            {
                using (var pen = new Pen(Color.Orange, 0.1f))
                {
                    foreach (var ball in primitives.Values.OfType<Ball>())
                    {
                        for (int n = -90; n < 90; n += 10)
                        {
                            double radius = Math.Cos(n * Math.PI / 180.0);
                            p1 = default(Vector3);
                            for (int m = 0; m <= 90; m++)
                            {
                                p2 = new Vector3(
                                   radius * Math.Cos(m * Math.PI / 45.0),
                                   radius * Math.Sin(m * Math.PI / 45.0),
                                   Math.Sin(n * Math.PI / 180.0));
                                p2 = ball.GetTextureToWorldTransformation(p2, time) + ball.GetCenter(time);
                                if (m > 0)
                                    e.Graphics.DrawLine(pen, CoordToPixel(renderCamera.VertexToCoord(p1)), CoordToPixel(renderCamera.VertexToCoord(p2)));
                                p1 = p2;
                            }
                        }

                        for (int m = 0; m < 16; m++)
                        {
                            p1 = default(Vector3);
                            for (int n = -90; n <= 90; n += 10)
                            {
                                p2 = new Vector3(
                                    Math.Cos(m * Math.PI / 8.0) * Math.Cos(n * Math.PI / 180.0),
                                    Math.Sin(m * Math.PI / 8.0) * Math.Cos(n * Math.PI / 180.0),
                                    Math.Sin(n * Math.PI / 180.0));
                                p2 = ball.GetTextureToWorldTransformation(p2, time) + ball.GetCenter(time);
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
                    foreach (var position in positions.Values)
                    {
                        Vector3 va = model.Camera.From - position.Center;
                        Vector3 vb = Vector3.Cross(va, model.Camera.Hor).Normalize();
                        Vector3 vc = Vector3.Cross(vb, va).Normalize();
                        for (int n = 0; n <= 90; n++)
                        {
                            double f1 = Math.Cos(n * Math.PI / 45.0);
                            double f2 = Math.Sin(n * Math.PI / 45.0);
                            p2 = position.Center + f1 * vb + f2 * vc;
                            if (n > 0)
                                e.Graphics.DrawLine(pen, CoordToPixel(renderCamera.VertexToCoord(p1)), CoordToPixel(renderCamera.VertexToCoord(p2)));
                            p1 = p2;
                        }
                        string st = position.Name;
                        PointF center = CoordToPixel(renderCamera.VertexToCoord(position.Center));
                        SizeF size = e.Graphics.MeasureString(st, Font);
                        e.Graphics.DrawString(st, Font, Brushes.Black, new PointF(center.X - size.Width * 0.5f, center.Y - size.Height * 0.5f));
                    }
                }
            }

            if (ViewLightsCheckBox.Checked)
            {
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
            }

            if (ViewShadowOutlinesCheckBox.Checked)
            {

                // Draw shadow outlines
                for (int lightNr = 0; lightNr < lights.Length; lightNr++)
                {
                    var light = lights[lightNr];
                    var felt = primitives["Felt"];
                    using (var pen = new Pen(Color.FromArgb(100, pens[lightNr].Color), 0.5f))
                    {
                        foreach (var ball in primitives.Values.OfType<Ball>())
                        {
                            Vector3 va = light.Center - ball.GetCenter(time);
                            Vector3 vb = Vector3.Cross(va, new Vector3(1.0, 0.0, 0.0)).Normalize();
                            Vector3 vc = Vector3.Cross(vb, va).Normalize();
                            for (int n = 0; n < 90; n++)
                            {
                                double f1 = Math.Cos(n * Math.PI / 45.0);
                                double f2 = Math.Sin(n * Math.PI / 45.0);
                                Ray ray = new Ray { Origin = light.Center, Direction = (ball.GetCenter(time) + f1 * vb + f2 * vc - light.Center).Normalize() };
                                var intsec = felt.GetClosestIntersection(ray, IntersectionMode.Position, 0.0);
                                p2 = intsec.Position;
                                if (n > 0)
                                    e.Graphics.DrawLine(pen, CoordToPixel(renderCamera.VertexToCoord(p1)), CoordToPixel(renderCamera.VertexToCoord(p2)));
                                p1 = p2;
                            }
                        }
                    }
                }
            }


            if (ViewNumbers3DCheckBox.Checked)
            {
                // Draw numbers
                foreach (var ball in balls.Values)
                {
                    var number = ball.Number;
                    if (number != null)
                    {
                        PointF pp1 = default(PointF);
                        for (int n = 0; n <= 90; n++)
                        {
                            double angle1 = Ball.TextureAngle * Math.Cos(n * Math.PI / 45.0);
                            double angle2 = Ball.TextureAngle * Math.Sin(n * Math.PI / 45.0);
                            Vector3 v = new Vector3(
                                Math.Cos(angle1) * Math.Cos(angle2),
                                Math.Sin(angle1) * Math.Cos(angle2),
                                Math.Sin(angle2));
                            v = ball.GetTextureToWorldTransformation(v, time) + ball.GetCenter(time);
                            PointF pp2 = CoordToPixel(renderCamera.VertexToCoord(v));
                            if (n > 0)
                                e.Graphics.DrawLine(pens[4], pp1, pp2);
                            pp1 = pp2;
                        }
                        foreach (var arrowPoint in new[] {
                            new PointF(-0.2f, 0.4f),
                            new PointF(0.0f, 0.5f),
                            new PointF(0.0f, -0.5f),
                            new PointF(0.0f, 0.5f),
                            new PointF(0.2f, 0.4f)
                        }.Select((point, index) => new { point, index }))
                        {
                            double angle1 = Ball.TextureAngle * arrowPoint.point.X;
                            double angle2 = Ball.TextureAngle * arrowPoint.point.Y;
                            Vector3 v = new Vector3(
                                Math.Cos(angle1) * Math.Cos(angle2),
                                Math.Sin(angle1) * Math.Cos(angle2),
                                Math.Sin(angle2));
                            v = ball.GetTextureToWorldTransformation(v, time) + ball.GetCenter(time);
                            PointF pp2 = CoordToPixel(renderCamera.VertexToCoord(v));
                            if (arrowPoint.index != 0 && arrowPoint.index != 3)
                                e.Graphics.DrawLine(pens[4], pp1, pp2);
                            pp1 = pp2;
                        }
                    }
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

            PointF cubemapOffset = new PointF(textureSize + 30f, 0f);

            BallPosition selectedPosition = positions[CubeMapContextActiveCubeMap.SelectedItem.ToString()];
            Ball selectedBall = selectedPosition.Ball;

            if (selectedBall.SphereMap != null)
                e.Graphics.DrawImage(selectedBall.SphereMap, 0f, 0f);
            if (selectedPosition.CubeMap != null)
                e.Graphics.DrawImage(selectedPosition.CubeMap, cubemapOffset);

            // Draw lines from balls to reflected boxes
            if (ViewBoxesCheckbox.Checked)
            {
                foreach (var position in positions.Values)
                {
                    if (position.Boxes != null)
                    {
                        for (int boxNr = 0; boxNr < position.Boxes.Length; boxNr++)
                        {
                            using (var pen = new Pen(Color.FromArgb(150, pens[boxNr].Color), 1.0f))
                            {
                                PointF p1 = default(PointF), p2 = default(PointF);
                                for (int cornerNr = 0; cornerNr <= 4; cornerNr++)
                                {
                                    // Ray through a reflection (on the ball) of the corner of a box, seen from the model.Camera
                                    var ray = this.model.Camera.CoordToRay(PixelToCoord(position.Boxes[boxNr][cornerNr % 4]));

                                    var intsec = position.GetClosestIntersection(ray, IntersectionMode.PositionAndNormal);
                                    if (intsec.Hit)
                                    {
                                        // Calculate mirror vector;
                                        double a = -Vector3.Dot(intsec.Normal, ray.Direction);
                                        Vector3 dir = ray.Direction + 2.0 * a * intsec.Normal;

                                        double cs = Math.Cos(position.CubeMapOffset * Math.PI / 180.0);
                                        double sn = Math.Sin(position.CubeMapOffset * Math.PI / 180.0);
                                        Vector3 transformed = new Vector3(
                                            dir.X * cs + dir.Y * sn,
                                            dir.Y * cs - dir.X * sn,
                                            dir.Z
                                        );
                                        var p = CubeTexture.Project(dir, out int plane);
                                        p2 = new PointF(
                                            (float)(p.X + (plane - 1) * textureSize + cubemapOffset.X),
                                            (float)(p.Y + cubemapOffset.Y)
                                        );
                                    }
                                    if (cornerNr > 0)
                                        e.Graphics.DrawLine(pen, p1, p2);
                                    p1 = p2;
                                }
                            }
                        }
                    }
                }
            }
            for (int n = 1; n < 4; n++)
            {
                e.Graphics.DrawLine(Pens.DarkGray, cubemapOffset.X + n * textureSize, 0f, cubemapOffset.X + n * textureSize, textureSize);
            }

            e.Graphics.ResetTransform();
        }

        private ZoomablePictureBox hoverPictureBox = null;

        private void CubeMapBox_MouseHover(object sender, EventArgs e)
        {
            hoverPictureBox = CubeMapBox;
        }

        private void RenderBox_MouseHover(object sender, EventArgs e)
        {
            hoverPictureBox = RenderBox;
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (hoverPictureBox != null)
            {
                if (e.Delta < 0) hoverPictureBox.ZoomOut(e.Location);
                if (e.Delta > 0) hoverPictureBox.ZoomIn(e.Location);
            }
        }

        // Camera events

        private void CamRotationSetter_ValueChanged(object sender, EventArgs e)
        {
            double offsetx = OffsetXSetter.Value;
            double offsety = OffsetYSetter.Value;
            double dist = CamDistSetter.Value;

            model.Camera.Angle = CamRotationSetter.Value;
            double cs = Math.Cos(model.Camera.Angle * Math.PI / 180.0);
            double sn = Math.Cos(model.Camera.Angle * Math.PI / 180.0);
            model.Camera.From = new Vector3(offsetx * cs + offsety * sn, offsety * cs - offsetx * sn, 1).Normalize() * dist;

            CalcScene();
            RenderBox.Invalidate();
            CubeMapBox.Invalidate();
        }

        private void CamDistSetter_ValueChanged(object sender, EventArgs e)
        {
            model.Camera.From = model.Camera.At - Vector3.Normalize(model.Camera.Look) * CamDistSetter.Value;
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
            if (model.Camera.ApertureH < 1.0) model.Camera.ApertureH = 1.0;
            if (model.Camera.ApertureV < 1.0) model.Camera.ApertureV = 1.0;
            for (int iter1 = 0; iter1 < 4; iter1++)
            {
                foreach (var position in positions.Values)
                {
                    Vector2 q1 = PixelToCoord(position.PixelCenter);
                    Vector2 q2 = q1;
                    for (int iter2 = 0; iter2 < 10; iter2++)
                    {
                        var ray = model.Camera.CoordToRay(q2);
                        double f = (1.0 - ray.Origin.Z) / ray.Direction.Z;
                        position.Center = ray.Origin + ray.Direction * f;
                        double minx = 1E12, maxx = -1E12, miny = 1E12, maxy = -1E12;

                        // Measure the graphical extents of the rendered ball
                        Vector3 va = model.Camera.From - position.Center;
                        Vector3 vb = Vector3.Cross(va, model.Camera.Hor).Normalize();
                        Vector3 vc = Vector3.Cross(vb, va).Normalize();
                        for (int n = 0; n < 90; n++)
                        {
                            double f1 = Math.Cos(n * Math.PI / 45.0);
                            double f2 = Math.Sin(n * Math.PI / 45.0);
                            Vector2 px = model.Camera.VertexToCoord(position.Center + f1 * vb + f2 * vc);
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
                foreach (var position in positions.Where(it => it.Key == "Ball 1" || it.Key == "Ball 4a").Select(it => it.Value))
                {
                    double minx1 = 1E12, maxx1 = -1E12, miny1 = 1E12, maxy1 = -1E12;
                    double minx2 = 1E12, maxx2 = -1E12, miny2 = 1E12, maxy2 = -1E12;

                    // Measure the graphical extents of the measured ellipse
                    foreach (var point in position.Ellipse.GetOutline(90))
                    {
                        Vector2 v = PixelToCoord(point);
                        if (v.X < minx1) minx1 = v.X;

                        if (v.X > maxx1) maxx1 = v.X;
                        if (v.Y < miny1) miny1 = v.Y;
                        if (v.Y > maxy1) maxy1 = v.Y;
                    }

                    // Measure the graphical extents of the rendered ball
                    Vector3 va = model.Camera.From - position.Center;
                    Vector3 vb = Vector3.Cross(va, model.Camera.Hor).Normalize();
                    Vector3 vc = Vector3.Cross(vb, va).Normalize();
                    for (int n = 0; n < 90; n++)
                    {
                        double f1 = Math.Cos(n * Math.PI / 45.0);
                        double f2 = Math.Sin(n * Math.PI / 45.0);
                        Vector2 v = model.Camera.VertexToCoord(position.Center + f1 * vb + f2 * vc);
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
                    model.Camera.ApertureH = model.Camera.ApertureH * facH / sum;
                    model.Camera.ApertureV = model.Camera.ApertureV * facV / sum;
                }
            }
            ApertureHLabel.Text = model.Camera.ApertureH.ToString("0.00°");
            ApertureVLabel.Text = model.Camera.ApertureV.ToString("0.00°");

            // Calculate world coordinates of the bitmap
            for (int n = 0; n < 4; n++)
            {
                Vector2 p1 = PixelToCoord(new PointF(
                    n == 0 || n == 3 ? 0f : pictureWidth,
                    n == 0 || n == 1 ? 0f : pictureHeight
                ));
                var ray = model.Camera.CoordToRay(p1);
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
                    BallPosition position = positions[spot.TargetPosition];
                    Vector3 sum = new Vector3();
                    foreach (var point in spot.InnerEllipse.GetOutline(16))
                    {
                        Vector2 v2 = PixelToCoord(point);
                        var intsec = position.GetClosestIntersection(model.Camera.CoordToRay(v2), IntersectionMode.PositionAndNormal);
                        if (intsec.Hit)
                        {
                            sum += intsec.Normal;
                        }
                    }
                    Ray reflectedRay = new Ray();
                    Vector3 normal = sum.Normalize();   // The average of all normals found in the loop
                    reflectedRay.Origin = position.Center + normal;
                    Vector3 direction = (reflectedRay.Origin - model.Camera.From).Normalize();
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
                    BallPosition position = positions[spot.TargetPosition];
                    for (int m = 0; m < 2; m++)
                    {
                        Ellipse ellipse = m == 0 ? spot.InnerEllipse : spot.OuterEllipse;
                        double radiusSum = 0.0;
                        double sum = 0.0;
                        foreach (var point in ellipse.GetOutline(16))
                        {
                            var intsec = position.GetClosestIntersection(model.Camera.CoordToRay(PixelToCoord(point)), IntersectionMode.PositionAndNormal);
                            if (intsec.Hit)
                            {
                                Ray reflectedRay = new Ray
                                {
                                    Origin = intsec.Position
                                };
                                Vector3 direction = (reflectedRay.Origin - model.Camera.From).Normalize();
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
                    var position = number.Position;

                    Ray ray = default(Ray);
                    Intersection intsec = default(Intersection);

                    // Calculate center vertex of texture
                    Vector3 sum = default(Vector3);
                    foreach (var point in number.Ellipse.GetOutline(16))
                    {
                        // Calculate ray going through the ellipse points
                        Vector2 px = PixelToCoord(point);
                        ray = model.Camera.CoordToRay(px);

                        // Calculate intersection with ball
                        intsec = position.GetClosestIntersection(ray, IntersectionMode.PositionAndNormal);
                        if (intsec.Hit)
                        {
                            sum += intsec.Normal;
                        }
                        else
                        {
                        }
                    }
                    Vector3 center = sum.Normalize();

                    // Calculate transformation matrix of texture
                    // - Rotation about Z and Y axes
                    Vector3 textureOrientation = new Vector3
                    {
                        Z = -Math.Atan2(center.Y, center.X)
                    };
                    var rot = Matrix4.RotateZ(textureOrientation.Z);
                    var v1 = center * rot;
                    textureOrientation.Y = Math.Atan2(v1.Z, v1.X);
                    rot = rot * Matrix4.RotateY(textureOrientation.Y);

                    // -Orientation of the texture by using the captured OrientStart and OrientEnd
                    Ray rayStart = model.Camera.CoordToRay(PixelToCoord(number.OrientStart));
                    var intsecStart = position.GetClosestIntersection(rayStart, IntersectionMode.PositionAndNormal);
                    Ray rayEnd = model.Camera.CoordToRay(PixelToCoord(number.OrientEnd));
                    var intsecEnd = position.GetClosestIntersection(rayEnd, IntersectionMode.PositionAndNormal);
                    var a = intsecEnd.Normal * rot - intsecStart.Normal * rot;
                    textureOrientation.X = Math.Atan2(a.Y, a.Z) + Math.PI;

                    // - World <-> Texture transformation matrices
                    position.TextureOrientation = textureOrientation;
                    position.WorldToTexture = Matrix4.Rotate(textureOrientation);
                    position.TextureToWorld = Matrix4.AffineInvert(position.WorldToTexture);
                }
            }

            // Apply keyframes
            foreach (var primitive in primitives.Values.OfType<Ball>())
            {
                primitive.ApplyKeyframes(keyframes);
            }

            CalibrateColors();
        }

        private void CalibrateColors()
        {
            var raytracer = new Raytracer(2, 2, renderBitmap.Width, renderBitmap.Height, model);

            // Calibrate felt color
            var felt = primitives["Felt"];
            for (int n = 0; n < 5; n++)
            {
                var actual = raytracer.RenderPixel(colorRefs[0].PixelCenter);
                felt.DiffuseColor = felt.DiffuseColor + colorRefs[0].Measured - actual;
            }

            // Calibrate ball 1 color
            var ball = (Ball)primitives["Ball 1"];
            for (int n = 0; n < 5; n++)
            {
                var actual = raytracer.RenderPixel(colorRefs[5].PixelCenter);
                ball.BandColor = ball.DiffuseColor = ball.DiffuseColor + colorRefs[5].Measured - actual;
            }

            // Calibrate ball 4 color
            ball = (Ball)primitives["Ball 4"];
            for (int n = 0; n < 5; n++)
            {
                var actual = raytracer.RenderPixel(colorRefs[6].PixelCenter);
                ball.BandColor = ball.DiffuseColor = ball.DiffuseColor + colorRefs[6].Measured - actual;
            }

            // Calibrate white ball color
            ball = (Ball)primitives["Ball w"];
            for (int n = 0; n < 5; n++)
            {
                var actual = raytracer.RenderPixel(colorRefs[7].PixelCenter);
                ball.BandColor = ball.DiffuseColor = ball.DiffuseColor + colorRefs[7].Measured - actual;
            }

            // Calibrate reflection
            //for (int n = 0; n < 5; n++)
            //{
            //    var actual = RenderPixel(PixelToCoord(colorRefs[8].PixelCenter));
            //    reflection = reflection + colorRefs[8].Measured.G - actual.G;
            //}
            // Calibrate number texture white

            Color3 numberTextureWhiteColor = Color3.FromColor(Color.White);
            Color3 numberTextureBlackColor = Color3.FromColor(Color.Black);
            for (int n = 0; n < 5; n++)
            {
                var actual = raytracer.RenderPixel(colorRefs[9].PixelCenter);
                numberTextureWhiteColor = numberTextureWhiteColor + colorRefs[9].Measured - actual;
                primitives["Ball 1"].Texture.SetColorTransformation(numberTextureBlackColor, numberTextureWhiteColor);
            }
            for (int n = 0; n < 5; n++)
            {
                var actual = raytracer.RenderPixel(colorRefs[10].PixelCenter);
                numberTextureBlackColor = numberTextureBlackColor + colorRefs[10].Measured - actual;
                primitives["Ball 1"].Texture.SetColorTransformation(numberTextureBlackColor, numberTextureWhiteColor);
            }

            primitives["Ball 1"].Texture.SetColorTransformation(numberTextureBlackColor, numberTextureWhiteColor);
            primitives["Ball 9"].Texture.SetColorTransformation(numberTextureBlackColor, numberTextureWhiteColor);
            primitives["Ball 8"].Texture.SetColorTransformation(numberTextureBlackColor, numberTextureWhiteColor);
            primitives["Ball 4"].Texture.SetColorTransformation(numberTextureBlackColor, numberTextureWhiteColor);

            ball = (Ball)primitives["Ball 9"];
            ball.BandColor = primitives["Ball 1"].DiffuseColor;
            ball.DiffuseColor = primitives["Ball w"].DiffuseColor;
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
                                count++;
                            }
                        }
                    }
                }
                colorRef.Measured = sum / count;
            }
            picture.UnlockBits(bmpDataRead);
        }

        // Cube maps
        private void CalculateCubeMaps(double precision)
        {
            foreach (var position in positions.Values)
            {
                Bitmap bmp = position.CubeMap;
                if (bmp != null)
                {
                    var ball = position.Ball;

                    // Writing to cube map
                    var bmpDataWrite = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                    // Reading from original picture
                    var bmpDataReadOriginal = picture.LockBits(new Rectangle(0, 0, picture.Width, picture.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                    // Reading from original detail picture
                    var bmpDataReadOriginalDetail = pictureDetail.LockBits(new Rectangle(0, 0, pictureDetail.Width, pictureDetail.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                    // Find the graphical box containing the sphere
                    double minx = 1E12, maxx = -1E12, miny = 1E12, maxy = -1E12;
                    Vector3 va = model.Camera.From - position.Center;
                    Vector3 vb = Vector3.Cross(va, model.Camera.Hor).Normalize();
                    Vector3 vc = Vector3.Cross(vb, va).Normalize();
                    for (int n = 0; n < 90; n++)
                    {
                        double f1 = Math.Cos(n * Math.PI / 45.0);
                        double f2 = Math.Sin(n * Math.PI / 45.0);
                        Vector2 px = model.Camera.VertexToCoord(position.Center + f1 * vb + f2 * vc);
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
                            Ray ray = model.Camera.CoordToRay(new Vector2(x, y));
                            var intsec = position.GetClosestIntersection(ray, IntersectionMode.PositionAndNormal);
                            if (intsec.Hit)
                            {
                                // Calculate mirror vector
                                double a = -Vector3.Dot(intsec.Normal, ray.Direction);
                                Vector3 direction = ray.Direction + 2.0 * a * intsec.Normal;
                                Vector2 p = CubeTexture.Project(direction, out int plane);
                                if (plane > 0 && plane < 5)     // we render just the 4 sides
                                {
                                    Point q = new Point(
                                        (int)((p.X * 0.5 + 0.5) * textureSize).Limit(0, textureSize - 1) + (plane - 1) * textureSize,
                                        (int)((p.Y * 0.5 + 0.5) * textureSize).Limit(0, textureSize - 1)
                                    );
                                    unsafe
                                    {
                                        var pixelCoord = CoordToPixel(new Vector2(x, y));
                                        PointF detailCoord = new PointF(
                                            (pixelCoord.X - pictureDetailRect.X) * pictureDetail.Width / pictureDetailRect.Width,
                                            (pixelCoord.Y - pictureDetailRect.Y) * pictureDetail.Height / pictureDetailRect.Height
                                        );

                                        byte* adrReadOriginal;
                                        if (detailCoord.X >= 0 && detailCoord.X < pictureDetail.Width &&
                                            detailCoord.Y >= 0 && detailCoord.Y < pictureDetail.Height)
                                        {
                                            adrReadOriginal = (byte*)(bmpDataReadOriginalDetail.Scan0 + (int)detailCoord.Y * bmpDataReadOriginalDetail.Stride + (int)detailCoord.X * 3);
                                        }
                                        else
                                        {
                                            // Reading from original picture
                                            adrReadOriginal = (byte*)(bmpDataReadOriginal.Scan0 + (int)(pixelCoord.Y * pictureScale) * bmpDataReadOriginal.Stride + (int)(pixelCoord.X * pictureScale) * 3);
                                        }

                                        // Writing to cube map
                                        byte* adrWrite = (byte*)(bmpDataWrite.Scan0 + q.Y * bmpDataWrite.Stride + q.X * 3);

                                        *adrWrite++ = *adrReadOriginal++;
                                        *adrWrite++ = *adrReadOriginal++;
                                        *adrWrite++ = *adrReadOriginal++;
                                    }
                                }
                            }
                        }

                    picture.UnlockBits(bmpDataReadOriginal);
                    pictureDetail.UnlockBits(bmpDataReadOriginalDetail);
                    bmp.UnlockBits(bmpDataWrite);

                }
            }
            CubeMapBox.Invalidate();
        }

        // Sphere maps
        private void CalculateSphereMaps()
        {
            foreach (Number number in numbers)
            {
                BallPosition position = number.Position;
                Ball ball = position.Ball;
                Bitmap bmp = ball.SphereMap;
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
                            double angle1 = 2.0 * x * Ball.TextureAngle / bmp.Width - Ball.TextureAngle;
                            double angle2 = Ball.TextureAngle - 2.0 * y * Ball.TextureAngle / bmp.Height;
                            Vector3 v = new Vector3(
                                Math.Cos(angle1) * Math.Cos(angle2),
                                Math.Sin(angle1) * Math.Cos(angle2),
                                Math.Sin(angle2));
                            v = v * position.TextureToWorld;
                            unsafe
                            {
                                var pixelCoord = CoordToPixel(model.Camera.VertexToCoord(position.Center + v));
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

        private void ViewOptionsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            RenderBox.Invalidate();
            if (sender == ViewBoxesCheckbox)
                CubeMapBox.Invalidate();
        }

        private void ViewEnabledCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            RenderBox.Invalidate();
        }

        private void TimeSetter_ValueChanged(object sender, EventArgs e)
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
            CalculateCubeMaps(0.5f);
        }

        private void CubemapsRecalcFineMenuItem_Click(object sender, EventArgs e)
        {
            CalculateCubeMaps(0.2f);
        }

        private void CubemapsCopyMenuItem_Click(object sender, EventArgs e)
        {
            if (CubeMapContextActiveCubeMap.SelectedIndex > -1)
            {
                BallPosition position = positions[CubeMapContextActiveCubeMap.SelectedItem.ToString()];
                if (position.CubeMap != null)
                    Clipboard.SetImage(position.CubeMap);
            }
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
                ViewBallOutlineFlatCheckBox.Checked = false;
                ViewRenderingCheckBox.Checked = true;
                int width = renderBitmap.Width;
                int height = renderBitmap.Height;
                var raytracer = new Raytracer(nrSamplesX, nrSamplesY, renderBitmap.Width, renderBitmap.Height, model);
                IProgress<Raytracer.Line> progress = new Progress<Raytracer.Line>(line =>
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
                    await Task.Run(() => raytracer.Render(progress, cts.Token));
                }
                catch (OperationCanceledException)
                {
                }
            }
        }

        private void SaveRenderingButton_Click(object sender, EventArgs e)
        {
            if (SaveRenderingDialog.ShowDialog() == DialogResult.OK)
            {
                using (var s = new FileStream(SaveRenderingDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    renderBitmap.Save(s, ImageFormat.Png);
                }
            }
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            if (ExportersCombo.SelectedItem is Type exporterType)
            {
                var exporter = (Exporter)Activator.CreateInstance(exporterType);
                ExportDialog.DefaultExt = exporter.GetFileDialogFilter().Split('|')[1];
                ExportDialog.Filter = exporter.GetFileDialogFilter();
                if (ExportDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var stream = new FileStream(ExportDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        exporter.Export(stream, model);
                    }
                }
            }
        }

        private void RenderBox_MouseClick(object sender, MouseEventArgs e)
        {
            Point p = new Point(
                (int)((e.X - RenderBox.Offset.X) / RenderBox.Zoom),
                (int)((e.Y - RenderBox.Offset.Y) / RenderBox.Zoom)
            );
            if (p.X >= 0 && p.X <= renderBitmap.Width && p.Y >= 0 && p.Y <= renderBitmap.Height)
            {
                var raytracer = new Raytracer(nrSamplesX, nrSamplesY, renderBitmap.Width, renderBitmap.Height, model);
                Color3 col = raytracer.RenderPixel(p);
                RenderButton.BackColor = col.ToColor();
            }
            else
            {
                RenderButton.BackColor = SystemColors.Control;
            }
        }
    }
}
