using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HellTower.View
{
    public class Menu
    {
        public readonly List<MenuButton> buttons = new List<MenuButton>();
        protected int selectedIndex = 0;
        protected readonly Image background;

        public Menu(string backgroundImagePath = null) // Делаем параметр необязательным
        {
            if (backgroundImagePath != null)
                background = Image.FromFile(backgroundImagePath);
        }

        public virtual void Draw(Graphics g, int screenWidth, int screenHeight)
        {
            if (background != null)
                g.DrawImage(background, 0, 0, screenWidth, screenHeight);
            foreach (var button in buttons)
                button.Draw(g);
        }

        public void SelectNext()
        {
            buttons[selectedIndex].IsSelected = false;
            selectedIndex = (selectedIndex + 1) % buttons.Count;
            buttons[selectedIndex].IsSelected = true;
        }

        public void SetFirstSelected()
        {
            buttons[selectedIndex].IsSelected = false;
            selectedIndex = 0;
            buttons[selectedIndex].IsSelected = true;

        }

        public void SelectPrevious()
        {
            buttons[selectedIndex].IsSelected = false;
            selectedIndex = (selectedIndex - 1 + buttons.Count) % buttons.Count;
            buttons[selectedIndex].IsSelected = true;
        }

        public void ExecuteSelected() => buttons[selectedIndex].Action?.Invoke();
    }
}
