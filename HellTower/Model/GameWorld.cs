using HellTower.Model.Entity;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace HellTower.Model
{
    public class GameWorld
    {
        public Player Player { get; set; }
        public List<Platform> Platforms { get; set; } = new List<Platform>();
        public List<Window> Windows { get; set; } = new List<Window>();
        public List<Enemy> Enemies { get; set; } = new List<Enemy>();
        public List<EnergyAttack> EnergyAttacks { get; set; } = new List<EnergyAttack>();
        public float CameraY { get; set; }
        public bool IsGameOver { get; set; }
        public double Score { get; set; }
        public double Height { get; set; }

        public void Reset()
        {
            Player = new Player();
            Platforms.Clear();
            Windows.Clear();
            Enemies.Clear();
            EnergyAttacks.Clear();
            Score = 0;
            Height = 0;
            IsGameOver = false;
            CameraY = 0;
        }

        public void Update()
        {
            Player.PrevY = Player.Y;

            Player.Update();

            Player.IsGrounded = false;

            if (Player.IsAttacking)
            {
                var enemiesToCheck = Enemies.ToList();
                foreach (var enemy in enemiesToCheck)
                {
                    if (Player.Slash.Bounds.IntersectsWith(enemy.Bounds) &&
                        Player.Slash.RegisterHit(enemy))
                    {
                        float forceX = 0, forceY = 0;
                        float enemyForce = GameSettings.EnemyKnockbackForce;
                        switch (Player.Slash.Direction)
                        {
                            case AttackDirection.Up:
                                forceY = -enemyForce;
                                break;
                            case AttackDirection.Down:
                                forceY = enemyForce;
                                break;
                            case AttackDirection.Left:
                                forceX = -enemyForce;
                                break;
                            case AttackDirection.Right:
                                forceX = enemyForce;
                                break;
                        }
                        if (enemy is Bat) 
                            enemy.ApplyKnockback(forceX, forceY);
                        else
                            enemy.ApplyKnockback(forceX*0.4f, 0);

                        float playerForceX = 0, playerForceY = 0;

                        switch (Player.Slash.Direction)
                        {
                            case AttackDirection.Up:
                                playerForceY = GameSettings.AttackKnockbackForce * 0.25f;
                                break;
                            case AttackDirection.Down:
                                playerForceY = -GameSettings.AttackKnockbackForce * (Player.IsGrounded ? 0.5f : 1.5f);
                                break;
                            case AttackDirection.Left:
                                playerForceX = GameSettings.AttackKnockbackForce * 0.4f;
                                break;
                            case AttackDirection.Right:
                                playerForceX = -GameSettings.AttackKnockbackForce * 0.4f;
                                break;
                        }

                        Player.ApplyKnockback(playerForceX, playerForceY);

                        if (enemy is Bat bat)
                        {
                            bat.Health--;
                            if (bat.Health <= 0)
                            {
                                Enemies.Remove(bat);
                                Score += (int)(100*(1+Height/100));
                                if (Player.Energy < 100)
                                    Player.Energy += 5;
                            }
                        }
                        if (enemy is Skeleton skeleton)
                        {
                            skeleton.Health--;
                            if (skeleton.Health <= 0)
                            {
                                Enemies.Remove(skeleton);
                                Score += 200;
                                if (Player.Energy < 100)
                                    Player.Energy += 10;
                            }
                        }
                    }
                }
            }

            foreach (var energyAttack in EnergyAttacks.ToList())
            {
                energyAttack.Update();
                if (!energyAttack.IsActive)
                {
                    EnergyAttacks.Remove(energyAttack);
                    continue;
                }
                foreach (var enemy in Enemies.ToList())
                {
                    if (energyAttack.TryHitEnemy(enemy))
                    {
                        if (enemy is Bat bat)
                        {
                            bat.Health -= 3;
                            if (bat.Health <= 0)
                            {
                                Enemies.Remove(bat);
                                Score += (int)(100 * (1 + Height / 100));
                                if (Player.Energy < 100)
                                    Player.Energy += 5;
                            }
                        }
                        else if (enemy is Skeleton skeleton)
                        {
                            skeleton.ApplyKnockback(energyAttack.IsFacingRight ? GameSettings.EnemyKnockbackForce : -GameSettings.EnemyKnockbackForce, 0);
                            skeleton.Health -= 3;
                            if (skeleton.Health <= 0)
                            {
                                Enemies.Remove(skeleton);
                                Score += 200;
                                if (Player.Energy < 100)
                                    Player.Energy += 10;
                            }
                        }
                    }
                }
            }

            foreach (var platform in Platforms)
            {
                if (Player.CheckPlatformCollision(platform))
                {
                    Player.IsGrounded = true;
                    Player.VelocityY = 0;
                    Player.Y = platform.Y - Player.Height;
                    if (Player.IsKnockback)
                        Player.KnockbackVelocityY = 0;
                    break;
                }
                if (Player.IsKnockback && Player.VelocityY > 0 && 
                    Player.Y + Player.Height > platform.Y &&
                    Player.Y + Player.Height < platform.Y + 35 && 
                    Player.X + Player.Width > platform.X - 10 &&
                    Player.X < platform.X + platform.Width + 10)
                {
                    Player.IsGrounded = true;
                    Player.VelocityY = 0;
                    Player.KnockbackVelocityY = 0;
                    Player.Y = platform.Y - Player.Height;
                    break;
                }
            }

            foreach (var platform in Platforms)
            {
                if (Player.CheckSideCollision(platform))
                {
                    if (Player.VelocityX > 0 && Player.X + Player.Width > platform.X && Player.X < platform.X)
                    {
                        Player.X = platform.X - Player.Width;
                        Player.VelocityX = 0;
                    }
                    else if (Player.VelocityX < 0 && Player.X < platform.X + platform.Width && Player.X + Player.Width > platform.X + platform.Width)
                    {
                        Player.X = platform.X + platform.Width;
                        Player.VelocityX = 0;
                    }
                }
            }

            foreach (var enemy in Enemies)
            {
                enemy.Update(Player);
                if (enemy is Skeleton skeleton && skeleton.IsAttacking)
                    if (skeleton.AttackHitbox.IntersectsWith(Player.Bounds))
                        Player.TakeDamage();
                if (enemy.Bounds.IntersectsWith(Player.Bounds))
                    Player.TakeDamage();
            }

            if (Player.Y > CameraY + GameSettings.ScreenHeight || Player.Health == 0)
                IsGameOver = true;

            if (Player.Y < CameraY + GameSettings.ScreenHeight / 8)
                CameraY = Player.Y - GameSettings.ScreenHeight / 8;

            Height = (int)(GameSettings.ScreenHeight - Player.Y)/100;
            Platforms.RemoveAll(p => p.Y > Player.Y + GameSettings.ScreenHeight);
            Enemies.RemoveAll(e => e.Y > CameraY + GameSettings.ScreenHeight * 1.5f);
        }
    }
}
