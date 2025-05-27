using System.Drawing;

namespace HellTower.Model.Entity
{
    public abstract class Enemy
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Health { get; set; }
        public bool IsAnimationFrame1 { get; set; } = true;
        public bool IsFacingRight { get; set; }
        public virtual bool IsAttacking { get; set; }
        public float KnockbackTimer { get; set; }
        public const float KnockbackDuration = 0.2f;
        public bool IsKnockback => KnockbackTimer > 0;
        public float KnockbackVelocityX { get; set; }
        public float KnockbackVelocityY { get; set; }

        public RectangleF Bounds => new RectangleF(X, Y, Width, Height);

        public virtual void Update(Player player)
        {
            if (IsKnockback)
            {
                KnockbackTimer -= 0.016f;
                float slowdown = KnockbackTimer / KnockbackDuration;
                X += KnockbackVelocityX * slowdown;
                Y += KnockbackVelocityY * slowdown;
                return;
            }
        }

        public void ApplyKnockback(float forceX, float forceY)
        {
            KnockbackVelocityX = forceX;
            KnockbackVelocityY = forceY;
            KnockbackTimer = KnockbackDuration;
            VelocityX = 0;
            VelocityY = 0;
        }
    }
}
