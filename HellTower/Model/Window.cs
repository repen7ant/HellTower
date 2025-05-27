using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HellTower.Model
{
    public class Window
    {
        public int X { get; set; }
        public int Y { get; set; }
        public static int Width = 85;
        public static int Height  = 120;
        public int BatsSpawned { get; set; } = 0;
        public float LastSpawnTime { get; set; } = 0;
    }
}
