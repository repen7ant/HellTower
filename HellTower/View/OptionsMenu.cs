using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HellTower.View
{
    public class OptionsMenu : Menu
    {
        public OptionsMenu() : base("Resources/Images/Background/menu_background.png")
        {
            buttons.Add(new MenuButton("Back", Rectangle.Empty, null));
        }

        public override void Draw(Graphics g, int screenWidth, int screenHeight)
        {
            base.Draw(g, screenWidth, screenHeight);

            var buttonWidth = 200;
            var buttonHeight = 50;

            buttons[0].Bounds = new Rectangle(
                (screenWidth - buttonWidth) / 2,
                screenHeight - 100,
                buttonWidth,
                buttonHeight);

            buttons[0].IsSelected = true;

            var titleFont = new Font("Stencil", 36, FontStyle.Bold);
            g.DrawString("OPTIONS AND CONTROLS", titleFont, Brushes.White,
                (screenWidth - g.MeasureString("OPTIONS AND CONTROLS", titleFont).Width) / 2,
                100);

            var controlsFont = new Font("Stencil", 18, FontStyle.Bold);
            string controlsText = "CONTROLS:\n\n" +
                                 "Move Left: A\n" +
                                 "Move Right: Dw\n" +
                                 "Jump: Space\n" +
                                 "Drop Down: S\n" +
                                 "Dash: Shift\n" +
                                 "Attack: J\n" +
                                 "Heal: Ctrl\n" +
                                 "Menu Navigation: W/S or Arrows\n" +
                                 "Select: Enter or Space\n" +
                                 "Pause: Esc";

            g.DrawString(controlsText, controlsFont, Brushes.White, (screenWidth - g.MeasureString("CONTROLS:", controlsFont).Width) / 2, 200);

            // Here you would add volume slider logic
        }
    }
}
