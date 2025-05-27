using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HellTower.View
{
    public class MenuButton
    {
        public string Text { get; set; }
        public Rectangle Bounds { get; set; }
        public bool IsSelected { get; set; }
        public Action Action { get; set; }

        public MenuButton(string text, Rectangle bounds, Action action)
        {
            Text = text;
            Bounds = bounds;
            Action = action;
            IsSelected = false;
        }

        public void Draw(Graphics g)
        {
            var brush = IsSelected ? Brushes.White : Brushes.Gray;
            var font = new Font("Stencil", 24, FontStyle.Bold);
            g.DrawString(Text, font, brush, Bounds.X, Bounds.Y);
        }
    }
}
