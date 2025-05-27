using HellTower.Model.Entity;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HellTower.Model.Entity
{
    public class Player
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; set; } = 50;
        public int Height { get; set; } = 75;
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public int Health { get; set; } = 3;
        public bool IsGrounded { get; set; }
        public bool IsDashing { get; set; }
        public bool IsHealing { get; set; }
        public float HealTimer { get; private set; } = 0f;
        public bool IsHealingComplete { get; private set; } = false;
        public bool IsFacingRight { get; set; }
        public bool IsInvincible { get; set; }
        public float invincibilityTimer { get; private set; } = 0f;
        public int Energy { get; set; } = 0;
        public float PrevY { get; set; }
        public bool IsHoldingJump { get; set; }
        public float JumpHoldTime { get; private set; }
        public Slash Slash { get; } = new Slash();
        public float KnockbackTimer { get; set; }
        public bool IsAttacking => Slash.IsAttacking;
        public bool IsKnockback => KnockbackTimer > 0;
        public float KnockbackVelocityX { get; set; }
        public float KnockbackVelocityY { get; set; }
        private float dashTimer = 0f;
        private float dashSpeed = 0f;
        public float DashCooldownTimer { get; private set; } = 0f;
        public bool CanDash => DashCooldownTimer <= 0f;
        public float RunAnimationTimer { get; set; } = 0f;
        public bool IsRunningAnimationFrame1 { get; set; } = true;
        public float EnergyAttackCooldownTimer { get; set; }
        public bool CanUseEnergyAttack => EnergyAttackCooldownTimer <= 0 && Energy >= 20;

        public RectangleF Bounds => new RectangleF(X, Y, Width, Height);

        public void MoveLeft() => VelocityX = IsHealing ? 0 : -GameSettings.PlayerSpeed;

        public void MoveRight() => VelocityX = IsHealing ? 0 : GameSettings.PlayerSpeed;

        public void StopMoving() => VelocityX = 0;

        public void Jump()
        {
            if (IsGrounded && !IsHealing)
            {
                VelocityY = GameSettings.InitialJumpForce;
                IsGrounded = false;
                IsHoldingJump = true;
                JumpHoldTime = 0f;
            }
        }

        public void UpdateJumpHold(float deltaTime)
        {
            if (IsHoldingJump)
            {
                JumpHoldTime += deltaTime;
                if (JumpHoldTime <= GameSettings.MaxHoldTime && VelocityY < 0)
                    VelocityY += GameSettings.HoldJumpForce * deltaTime / GameSettings.MaxHoldTime;
                else
                    IsHoldingJump = false;
            }
        }

        public void Dash(int direction)
        {
            if (!IsDashing && direction != 0 && !IsAttacking)
            {
                IsDashing = true;
                dashTimer = GameSettings.DashDuration;
                dashSpeed = direction * (GameSettings.DashDistance / GameSettings.DashDuration);
                DashCooldownTimer = GameSettings.DashCooldownDuration;
                VelocityY = 0; 
            }
        }
        public void TakeDamage()
        {
            if (!IsInvincible & !IsDashing)
            {
                Health--;
                IsInvincible = true;
                invincibilityTimer = GameSettings.InvincibilityDuration;
            }
        }

        public void StartHealing()
        {
            if (Energy >= 40 && Health < 3 && !IsHealing && !IsHealingComplete && !IsDashing && !IsKnockback)
            {
                IsHealing = true;
                HealTimer = GameSettings.HealDuration;
                IsHealingComplete = false;
            }
        }

        public void InterruptHealing()
        {
            if (IsHealing)
            {
                IsHealing = false;
                HealTimer = 0f;
                IsHealingComplete = false;
            }
        }

        public void Attack(AttackDirection direction)
        {
            if (!IsAttacking && !IsKnockback && !Slash.IsOnCooldown && !IsHealing && !IsDashing)
                Slash.StartAttack(direction);
        }
        public void Update()
        {
            PrevY = Y;

            if (!IsDashing)
            {
                X += VelocityX;
                Y += VelocityY;
            }

            UpdateJumpHold(0.016f);

            Slash.Update(0.016f, this);

            IsFacingRight = VelocityX > 0 || (VelocityX == 0 && IsFacingRight);

            if (X < 0)
            {
                X = 0;
                VelocityX = 0;
            }
            else if (X + Width > GameSettings.ScreenWidth)
            {
                X = GameSettings.ScreenWidth - Width;
                VelocityX = 0;
            }

            bool ignoreGravity = (IsKnockback && Slash.IsAttacking && Slash.Direction == AttackDirection.Down) || IsDashing;
            if (!ignoreGravity && !IsGrounded)
            {
                VelocityY += GameSettings.Gravity;
                if (VelocityY > 35)
                    VelocityY = 35;
            }

            if (DashCooldownTimer > 0f)
            {
                DashCooldownTimer -= 0.016f;
                if (DashCooldownTimer < 0f) 
                    DashCooldownTimer = 0f;
            }

            if (EnergyAttackCooldownTimer > 0)
                EnergyAttackCooldownTimer -= 0.016f;

            if (IsDashing)
            {
                X += dashSpeed * 0.07f;
                dashTimer -= 0.016f;
                if (X < 0)
                {
                    X = 0;
                    dashTimer = 0;
                }
                else if (X + Width > GameSettings.ScreenWidth)
                {
                    X = GameSettings.ScreenWidth - Width;
                    dashTimer = 0;
                }
                if (dashTimer <= 0)
                {
                    IsDashing = false;
                    dashSpeed = 0;
                }
                return; 
            }

            if (IsHealing)
            {
                HealTimer -= 0.016f;
                VelocityX *= 0.5f;
                if (HealTimer <= 0f)
                {
                    IsHealing = false;
                    IsHealingComplete = true;
                    Energy -= 40;
                    Health++;
                }
            }
            else
                IsHealingComplete = false;

            if (IsHealing && (IsKnockback || IsDashing))
                InterruptHealing();

            if (IsInvincible)
            {
                invincibilityTimer -= 0.016f;
                if (invincibilityTimer <= 0f)
                {
                    IsInvincible = false;
                    invincibilityTimer = 0f;
                }
            }

            if (KnockbackTimer > 0)
                KnockbackTimer -= 0.016f;
            else if (IsKnockback)
            {
                VelocityX = 0;
                VelocityY = 0;
            }

            if (IsKnockback)
            {
                KnockbackTimer -= 0.016f;
                float slowdown = KnockbackTimer / GameSettings.KnockbackDuration;
                X += KnockbackVelocityX * slowdown;
                Y += KnockbackVelocityY * slowdown;
                return;
            }

            if (Math.Abs(VelocityX) > 0.1f) 
            {
                RunAnimationTimer += 0.016f;
                if (RunAnimationTimer >= GameSettings.AnimationFrameTime)
                {
                    RunAnimationTimer = 0;
                    IsRunningAnimationFrame1 = !IsRunningAnimationFrame1;
                }
            }
            else
            {
                RunAnimationTimer = 0;
                IsRunningAnimationFrame1 = true;
            }
        }

        public void ApplyKnockback(float forceX, float forceY)
        {
            if (forceY > 0) 
                return;
            KnockbackVelocityX = forceX;
            KnockbackVelocityY = forceY;
            KnockbackTimer = GameSettings.KnockbackDuration;
            VelocityX = 0;
            VelocityY = 0;
        }

        public void DropDown()
        {
            if (IsGrounded)
            {
                Y += 5;
                IsGrounded = false;
            }
        }

        public bool CheckPlatformCollision(Platform platform)
        {
            const int skin = 2;
            bool isFalling = VelocityY > 0;
            bool wasAbove = PrevY + Height <= platform.Y;
            bool withinHorizontalBounds = X + Width > platform.X && X < platform.X + platform.Width;
            bool nowOnPlatform = Y + Height >= platform.Y && Y + Height <= platform.Y + platform.Height + skin;
            return isFalling && wasAbove && withinHorizontalBounds && nowOnPlatform;
        }

        public bool CheckSideCollision(Platform platform)
        {
            return (X < platform.X + platform.Width &&
                    X + Width > platform.X &&
                    Y < platform.Y + platform.Height &&
                    Y + Height > platform.Y);
        }
    }
}