using System;
using System.Drawing;

namespace HellTower.Model.Entity
{
    public class Bat : Enemy
    {
        private float patrolTimer = 0;
        private float animationTimer = 0;
        public bool isAnimationFrameUp = true;

        private Random _random = new Random();
        public Bat()
        {
            Width = 50;
            Height = 37;
        }

        public override void Update(Player player)
        {
            if (IsKnockback)
            {
                base.Update(player);
                return;
            }

            if (VelocityX != 0)
                IsFacingRight = VelocityX > 0;

            animationTimer += 0.016f;
            if (animationTimer > GameSettings.AnimationFrameTime)
            {
                animationTimer = 0;
                isAnimationFrameUp = !isAnimationFrameUp;
            }

            float dx = player.X - X;
            float dy = player.Y - Y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);

            if (distance < GameSettings.BatDetectionRange && distance > GameSettings.BatMinDistance)
            {
                float nx = dx / distance;
                float ny = dy / distance;
                VelocityX = nx * GameSettings.BatSpeed;
                VelocityY = ny * GameSettings.BatSpeed;
            }
            else if (distance <= GameSettings.BatMinDistance)
            {
                VelocityX = 0;
                VelocityY = 0;
            }
            else
            {
                patrolTimer += 0.016f;
                if (patrolTimer > GameSettings.BatPatrolChangeTime)
                {
                    patrolTimer = 0;
                    VelocityX = (float)(_random.NextDouble() - 0.5) * GameSettings.BatPatrolSpeed;
                    VelocityY = (float)(_random.NextDouble() - 0.5) * GameSettings.BatPatrolSpeed;
                }
            }
            X += VelocityX;
            Y += VelocityY;
        }
    }
}
