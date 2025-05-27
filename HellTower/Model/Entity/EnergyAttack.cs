using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HellTower.Model.Entity
{
    public class EnergyAttack
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; set; } = 390;
        public int Height { get; set; } = 150;
        public float Speed { get; set; } = 40f;
        public bool IsActive { get; set; } = true;
        public bool IsFacingRight { get; set; }
        public HashSet<Enemy> HitEnemies { get; } = new HashSet<Enemy>();
        public RectangleF Bounds => new RectangleF(X, Y, Width, Height);

        public void Update()
        {
            X += IsFacingRight ? Speed : -Speed;
            if (X > GameSettings.ScreenWidth || X + Width < 0)
                IsActive = false;
        }

        public bool TryHitEnemy(Enemy enemy)
        {
            if (HitEnemies.Contains(enemy))
                return false;
            if (Bounds.IntersectsWith(enemy.Bounds))
            {
                HitEnemies.Add(enemy);
                return true;
            }
            return false;
        }
    }
}
