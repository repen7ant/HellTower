using System.Drawing;

namespace HellTower.Model
{
    public class Platform
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; } = GameSettings.PlatformHeight;
        public RectangleF Bounds => new RectangleF(X, Y, Width, Height);
    }
}
