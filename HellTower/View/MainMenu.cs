using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HellTower.View
{
    public class MainMenu : Menu
    {
        public MainMenu() : base("Resources/Images/Background/menu_background.png")
        {
            buttons.Add(new MenuButton("Play", Rectangle.Empty, null));
            buttons.Add(new MenuButton("Options and Controls", Rectangle.Empty, null));
            buttons.Add(new MenuButton("Exit", Rectangle.Empty, null));

            buttons[0].IsSelected = true;
        }

        public override void Draw(Graphics g, int screenWidth, int screenHeight)
        {
            base.Draw(g, screenWidth, screenHeight);

            var buttonWidth = 200;
            var buttonHeight = 50;
            int startY = screenHeight / 2 - 100;

            for (var i = 0; i < buttons.Count; i++)
            {
                buttons[i].Bounds = new Rectangle(
                    (screenWidth - buttonWidth) / 2 - 100,
                    startY + i * 70,
                    buttonWidth,
                    buttonHeight);
            }

            var titleFont = new Font("Chiller", 80, FontStyle.Bold);
            g.DrawString("HELL TOWER", titleFont, Brushes.AntiqueWhite,
                (screenWidth - g.MeasureString("HELL TOWER", titleFont).Width) / 2,
                screenHeight / 4);
        }
    }
}
