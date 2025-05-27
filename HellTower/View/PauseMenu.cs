using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HellTower.View
{
    public class PauseMenu : Menu
    {
        public PauseMenu() : base()
        {
            buttons.Add(new MenuButton("Continue", Rectangle.Empty, null));
            buttons.Add(new MenuButton("Options and Controls", Rectangle.Empty, null));
            buttons.Add(new MenuButton("Quit to Menu", Rectangle.Empty, null));
            buttons[0].IsSelected = true;
        }

        public override void Draw(Graphics g, int screenWidth, int screenHeight)
        {
            using (var darkBrush = new SolidBrush(Color.FromArgb(180, 0, 0, 0)))
            {
                g.FillRectangle(darkBrush, 0, 0, screenWidth, screenHeight);
            }

            var buttonWidth = 250;
            var buttonHeight = 50;
            int startY = screenHeight / 2 - 100;

            for (var i = 0; i < buttons.Count; i++)
            {
                buttons[i].Bounds = new Rectangle(
                    (screenWidth - buttonWidth) / 2,
                    startY + i * 70,
                    buttonWidth,
                    buttonHeight);
            }

            var titleFont = new Font("Stencil", 48, FontStyle.Bold);
            g.DrawString("PAUSED", titleFont, Brushes.White,
                (screenWidth - g.MeasureString("PAUSED", titleFont).Width) / 2,
                screenHeight / 4);

            foreach (var button in buttons)
                button.Draw(g);
        }
    }
}
