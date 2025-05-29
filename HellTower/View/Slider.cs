using System;
using System.Drawing;
using System.Windows.Forms;

namespace HellTower.View
{
    public class Slider : Control
    {
        private int min = 0;
        private int max = 100;
        private int value = 100;
        private bool dragging = false;
        private int thumbRadius = 10;

        public event Action<int> ValueChanged;

        public int Minimum
        {
            get => min;
            set { min = value; Invalidate(); }
        }

        public int Maximum
        {
            get => max;
            set { max = value; Invalidate(); }
        }

        public int Value
        {
            get => value;
            set
            {
                int clamped = Math.Max(min, Math.Min(max, value));
                if (this.value != clamped)
                {
                    this.value = clamped;
                    ValueChanged?.Invoke(this.value);
                    Invalidate();
                }
            }
        }

    public Slider()
    {
        this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                      ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                      ControlStyles.SupportsTransparentBackColor, true);
        this.BackColor = Color.Transparent;
        this.Height = 30;
        this.Width = 300;
        this.TabStop = false;
    }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int trackY = this.Height / 2;
            int left = thumbRadius;
            int right = this.Width - thumbRadius;

            using (var trackPen = new Pen(Color.Gray, 4))
                g.DrawLine(trackPen, left, trackY, right, trackY);

            int thumbX = ValueToX(Value);
            using (var fillPen = new Pen(Color.DarkRed, 4))
                g.DrawLine(fillPen, left, trackY, thumbX, trackY);

            using (var brush = new SolidBrush(Color.White))
                g.FillEllipse(brush, thumbX - thumbRadius, trackY - thumbRadius, thumbRadius * 2, thumbRadius * 2);
            using (var outline = new Pen(Color.DodgerBlue, 2))
                g.DrawEllipse(outline, thumbX - thumbRadius, trackY - thumbRadius, thumbRadius * 2, thumbRadius * 2);

            string vstr = Value.ToString();
            var font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
            var size = g.MeasureString(vstr, font);
            g.DrawString(vstr, font, Brushes.Black, thumbX - size.Width / 2, trackY - thumbRadius - size.Height - 2);
        }

        private int ValueToX(int val)
        {
            float percent = (float)(val - min) / (max - min);
            int left = thumbRadius;
            int right = this.Width - thumbRadius;
            return left + (int)((right - left) * percent);
        }

        private int XToValue(int x)
        {
            int left = thumbRadius;
            int right = this.Width - thumbRadius;
            float percent = (float)(x - left) / (right - left);
            int val = min + (int)(percent * (max - min));
            return Math.Max(min, Math.Min(max, val));
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            int thumbX = ValueToX(Value);
            int dy = e.Y - (this.Height / 2);
            if (Math.Abs(e.X - thumbX) <= thumbRadius + 2 && Math.Abs(dy) <= thumbRadius + 2)
            {
                dragging = true;
                this.Capture = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (dragging)
                Value = XToValue(e.X);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (dragging)
            {
                dragging = false;
                this.Capture = false;
            }
        }
    }
}
