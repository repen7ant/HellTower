using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace HellTower.Model.Entity
{
    public class Slash
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; set; } = 120;
        public int Height { get; set; } = 100;
        public bool IsAttacking { get; private set; }
        public float AttackTimer { get; private set; }
        public float AttackCooldownTimer { get; private set; }
        public bool IsOnCooldown => AttackCooldownTimer > 0;
        public AttackDirection Direction { get; private set; }
        private HashSet<Enemy> hitEnemies = new HashSet<Enemy>();
        public RectangleF Bounds => new RectangleF(X, Y, Width, Height);

        public void StartAttack(AttackDirection direction)
        {
            if (!IsAttacking && !IsOnCooldown)
            {
                IsAttacking = true;
                AttackTimer = 0f;
                Direction = direction;
                hitEnemies.Clear();
            }
        }

        public bool RegisterHit(Enemy enemy)
        {
            if (hitEnemies.Contains(enemy))
                return false;
            hitEnemies.Add(enemy);
            return true;
        }

        public void Update(float deltaTime, Player player)
        {
            if (AttackCooldownTimer > 0)
                AttackCooldownTimer -= deltaTime;

            if (IsAttacking)
            {
                AttackTimer += deltaTime;
                switch (Direction)
                {
                    case AttackDirection.Up:
                        X = player.X + player.Width / 2 - Width / 2;
                        Y = player.Y - Height;
                        break;
                    case AttackDirection.Down:
                        X = player.X + player.Width / 2 - Width / 2;
                        Y = player.Y + player.Height;
                        break;
                    case AttackDirection.Left:
                        X = player.X - Width;
                        Y = player.Y + player.Height / 2 - Height / 2 - 15;
                        break;
                    case AttackDirection.Right:
                        X = player.X + player.Width;
                        Y = player.Y + player.Height / 2 - Height / 2 - 15;
                        break;
                }

                if (AttackTimer >= GameSettings.AttackDuration)
                {
                    IsAttacking = false;
                    AttackCooldownTimer = GameSettings.AttackCooldownDuration;
                }
            }
        }
    }

    public enum AttackDirection
    {
        Up,
        Down,
        Left,
        Right
    }
}