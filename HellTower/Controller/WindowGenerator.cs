using HellTower.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HellTower.Controller
{
    public class WindowGenerator
    {
        private readonly GameWorld _world;
        private readonly Random _random = new Random();
        private int nextWindowY;

        public WindowGenerator(GameWorld world)
        {
            _world = world;
        }

        public void GenerateInitialWindow()
        {
            var firstWindow = new Window
            {
                X = 0,
                Y = GameSettings.ScreenHeight + 100
            };
            _world.Windows.Add(firstWindow);
            nextWindowY = firstWindow.Y - GetNextWindowDistance();
        }

        public void GenerateWindows()
        {
            while (nextWindowY > _world.CameraY - GameSettings.ScreenHeight)
            {
                var newWindow = new Window
                {
                    X = _random.Next(30, GameSettings.ScreenWidth - Window.Width - 30),
                    Y = nextWindowY
                };
                if (!WindowOverlapsPlatform(newWindow))
                {
                    _world.Windows.Add(newWindow);
                    nextWindowY -= GetNextWindowDistance();
                }
                else
                    nextWindowY -= 60;
            }
            _world.Windows.RemoveAll(w => w.Y > _world.Player.Y + GameSettings.ScreenHeight);
        }

        private int GetNextWindowDistance() => _random.Next(GameSettings.PlatformMinVerticalDistance, GameSettings.PlatformMaxVerticalDistance);

        private bool WindowOverlapsPlatform(Window window)
        {
            var windowRect = new Rectangle(window.X, window.Y, Window.Width, Window.Height);
            foreach (var platform in _world.Platforms)
            {
                var platformRect = new Rectangle(platform.X, platform.Y, platform.Width, platform.Height);
                if (windowRect.IntersectsWith(platformRect))
                    return true;
            }
            return false;
        }
    }
}
