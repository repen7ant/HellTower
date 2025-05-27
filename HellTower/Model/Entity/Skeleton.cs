using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HellTower.Model.Entity
{
    public class Skeleton : Enemy
    {
        private readonly GameWorld _world;
        private readonly Random _random = new Random();
        private float moveTimer = 0;
        private float idleTimer = 0;
        private float attackCooldown = 0;
        private bool isAttacking = false;
        private float attackDuration = 0;
        private RectangleF attackHitbox;
        private bool isGrounded;
        public Platform Platform { get; set; }
        public override bool IsAttacking => isAttacking;
        public RectangleF AttackHitbox => attackHitbox;
        public float RunAnimationTimer { get; set; } = 0f;

        public float TimerToAttack { get; set; } = 0f;

        public Skeleton(GameWorld world)
        {
            _world = world;
            Width = 100;
            Height = 150;
        }

        public override void Update(Player player)
        {

            if (Math.Abs(VelocityX) > 0.1f)
            {
                RunAnimationTimer += 0.016f;
                if (RunAnimationTimer >= GameSettings.AnimationFrameTime)
                {
                    RunAnimationTimer = 0;
                    IsAnimationFrame1 = !IsAnimationFrame1;
                }

            }
            else
            {
                RunAnimationTimer = 0;
                IsAnimationFrame1 = true;
            }

            if (IsKnockback)
            {
                base.Update(player);
                return;
            }

            if (!isGrounded)
            {
                VelocityY += GameSettings.Gravity;
                Y += VelocityY;
            }
            else
                VelocityY = 0;

            isGrounded = false;
            Platform currentPlatform = FindPlatform();
            if (currentPlatform != null)
            {
                isGrounded = true;
                Y = currentPlatform.Y - Height;
            }

            if (attackCooldown > 0)
                attackCooldown -= 0.016f;

            if (isAttacking)
            {

                attackDuration -= 0.016f;
                if (attackDuration <= 0)
                {
                    isAttacking = false;
                    attackCooldown = GameSettings.SkeletonAttackCooldownTime;
                }
                return;
            }

            if (!isGrounded) return;

            float dx = player.X < X ? player.X + player.Width - X : player.X - (X + Width);
            float dy = player.Y - Y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);

            if (player.Y < Y + Height && distance < GameSettings.SkeletonDetectionRange)
            {
                if (distance < GameSettings.SkeletonAttackRange - 40f && TimerToAttack > 0)
                    VelocityX = 0;
                else
                {
                    VelocityX = Math.Sign(dx) * GameSettings.SkeletonMoveSpeed;
                    IsFacingRight = VelocityX > 0;
                }
                if ((VelocityX > 0 && X + Width >= currentPlatform.X + currentPlatform.Width) ||
                    (VelocityX < 0 && X <= currentPlatform.X))
                    VelocityX = 0;
                if (distance < GameSettings.SkeletonAttackRange && attackCooldown <= 0)
                {
                    VelocityX = 0;
                    if (TimerToAttack >= GameSettings.SkeletonTimeToAttack)
                        StartAttack();
                    TimerToAttack += 0.016f;
                }
            }
            else
            {
                if (moveTimer > 0)
                {
                    moveTimer -= 0.016f;
                    if ((VelocityX > 0 && X + Width >= currentPlatform.X + currentPlatform.Width) ||
                        (VelocityX < 0 && X <= currentPlatform.X))
                    {
                        VelocityX *= -1;
                        IsFacingRight = VelocityX > 0;
                    }
                }
                else if (idleTimer > 0)
                {
                    idleTimer -= 0.016f;
                    VelocityX = 0;
                }
                else
                {
                    if (_random.NextDouble() > 0.5)
                    {
                        moveTimer = GameSettings.SkeletonPatrolChangeTime;
                        VelocityX = (_random.NextDouble() > 0.5 ? 1 : -1) * GameSettings.SkeletonMoveSpeed;
                        IsFacingRight = VelocityX > 0;
                    }
                    else
                    {
                        idleTimer = GameSettings.SkeletonIdleDuration;
                        VelocityX = 0;
                    }
                }
            }
            X += VelocityX;
        }

        private void StartAttack()
        {
            isAttacking = true;
            attackDuration = GameSettings.SkeletonAttackActiveTime;
            TimerToAttack = 0;
            float hitboxWidth = 125;
            float hitboxHeight = 150;
            float hitboxX = IsFacingRight ? X + Width : X - hitboxWidth;
            attackHitbox = new RectangleF(
                hitboxX,
                Y,
                hitboxWidth,
                hitboxHeight);
        }

        private Platform FindPlatform()
        {
            foreach (var platform in _world.Platforms)
            {
                bool isOnPlatform = Y + Height >= platform.Y &&
                                  Y + Height <= platform.Y + 10 &&
                                  X + Width > platform.X &&
                                  X < platform.X + platform.Width;
                if (isOnPlatform)
                    return platform;
            }
            return null;
        }

    }
}