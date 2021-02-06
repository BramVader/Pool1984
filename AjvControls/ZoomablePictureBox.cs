using System.Drawing;
using System.Windows.Forms;

namespace Ajv.Controls
{
    public partial class ZoomablePictureBox : PictureBox
    {
        private float zoom = 1.0f;
        private PointF offset = default;

        private PointF oldOffset;
        private Point oldMousePos;
        private bool dragging;

        public ZoomablePictureBox()
        {
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            dragging = true;
            oldMousePos = e.Location;
            oldOffset = offset;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (dragging)
            {
                offset = new PointF(
                    oldOffset.X + (e.Location.X - oldMousePos.X),
                    oldOffset.Y + (e.Location.Y - oldMousePos.Y)
                );
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            dragging = false;
        }

        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; }
        }

        public PointF Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        public void ZoomIn(Point center)
        {
            float lastZoom = zoom;
            zoom /= 0.8f;
            PointF zoomCenter = center;
            offset = new PointF(
                zoomCenter.X - (zoomCenter.X - offset.X) * zoom / lastZoom,
                zoomCenter.Y - (zoomCenter.Y - offset.Y) * zoom / lastZoom
            );
            Invalidate();
        }

        public void ZoomOut(Point center)
        {
            float lastZoom = zoom;
            zoom *= 0.8f;
            PointF zoomCenter = center;
            offset = new PointF(
                zoomCenter.X - (zoomCenter.X - offset.X) * zoom / lastZoom,
                zoomCenter.Y - (zoomCenter.Y - offset.Y) * zoom / lastZoom
            );
            Invalidate();
        }
    }
}
