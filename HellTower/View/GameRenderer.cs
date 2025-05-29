using HellTower.Model;
using HellTower.Model.Entity;
using System;
using System.Drawing;
using System.Linq;

namespace HellTower.View
{
    public class GameRenderer
    {
        private readonly GameWorld _world;

        public GameRenderer(GameWorld world)
        {
            _world = world;
        }

        private readonly Image background = Image.FromFile("Resources/Images/Background/brick_background.png");
        private readonly Image window = Image.FromFile("Resources/Images/Background/window.png");
        private readonly Image platformBlock = Image.FromFile("Resources/Images/Background/platform_block.png");

        private readonly Image skeletonRunRight1 = Image.FromFile("Resources/Images/Entities/Skeleton/skeleton_moving_right_1.png");
        private readonly Image skeletonRunRight2 = Image.FromFile("Resources/Images/Entities/Skeleton/skeleton_moving_right_2.png");
        private readonly Image skeletonRunLeft1 = Image.FromFile("Resources/Images/Entities/Skeleton/skeleton_moving_left_1.png");
        private readonly Image skeletonRunLeft2 = Image.FromFile("Resources/Images/Entities/Skeleton/skeleton_moving_left_2.png");
        private readonly Image skeletonIdleLeft = Image.FromFile("Resources/Images/Entities/Skeleton/skeleton_idle_left.png");
        private readonly Image skeletonIdleRight = Image.FromFile("Resources/Images/Entities/Skeleton/skeleton_idle_right.png");
        private readonly Image skeletonAttackRight = Image.FromFile("Resources/Images/Entities/Skeleton/skeleton_attack_right.png");
        private readonly Image skeletonAttackLeft = Image.FromFile("Resources/Images/Entities/Skeleton/skeleton_attack_left.png");
        private readonly Image skeletonRunRight1Damage = Image.FromFile("Resources/Images/Entities/Skeleton/skeleton_moving_right_1_damage.png");
        private readonly Image skeletonRunRight2Damage = Image.FromFile("Resources/Images/Entities/Skeleton/skeleton_moving_right_2_damage.png");
        private readonly Image skeletonRunLeft1Damage = Image.FromFile("Resources/Images/Entities/Skeleton/skeleton_moving_left_1_damage.png");
        private readonly Image skeletonRunLeft2Damage = Image.FromFile("Resources/Images/Entities/Skeleton/skeleton_moving_left_2_damage.png");
        private readonly Image skeletonIdleLeftDamage = Image.FromFile("Resources/Images/Entities/Skeleton/skeleton_idle_left_damage.png");
        private readonly Image skeletonIdleRightDamage = Image.FromFile("Resources/Images/Entities/Skeleton/skeleton_idle_right_damage.png");
        private readonly Image skeletonAttackLeftDamage = Image.FromFile("Resources/Images/Entities/Skeleton/skeleton_attack_left_damage.png");
        private readonly Image skeletonAttackRightDamage = Image.FromFile("Resources/Images/Entities/Skeleton/skeleton_attack_right_damage.png");

        private readonly Image batLeftUp = Image.FromFile("Resources/Images/Entities/Bat/bat_left_up.png");
        private readonly Image batLeftDown = Image.FromFile("Resources/Images/Entities/Bat/bat_left_down.png");
        private readonly Image batRightUp = Image.FromFile("Resources/Images/Entities/Bat/bat_right_up.png");
        private readonly Image batRightDown = Image.FromFile("Resources/Images/Entities/Bat/bat_right_down.png");
        private readonly Image batLeftUpDamage = Image.FromFile("Resources/Images/Entities/Bat/bat_left_up_damage.png");
        private readonly Image batLeftDownDamage = Image.FromFile("Resources/Images/Entities/Bat/bat_left_down_damage.png");
        private readonly Image batRightUpDamage = Image.FromFile("Resources/Images/Entities/Bat/bat_right_up_damage.png");
        private readonly Image batRightDownDamage = Image.FromFile("Resources/Images/Entities/Bat/bat_right_down_damage.png");

        private readonly Image playerHealingLeft = Image.FromFile("Resources/Images/Entities/Player/player_healing_left.png");
        private readonly Image playerHealingRight = Image.FromFile("Resources/Images/Entities/Player/player_healing_right.png");
        private readonly Image playerIdleLeft = Image.FromFile("Resources/Images/Entities/Player/player_idle_left.png");
        private readonly Image playerIdleRight = Image.FromFile("Resources/Images/Entities/Player/player_idle_right.png");
        private readonly Image playerRunRight1 = Image.FromFile("Resources/Images/Entities/Player/player_run_right_1.png");
        private readonly Image playerRunRight2 = Image.FromFile("Resources/Images/Entities/Player/player_run_right_2.png");
        private readonly Image playerRunLeft1 = Image.FromFile("Resources/Images/Entities/Player/player_run_left_1.png");
        private readonly Image playerRunLeft2 = Image.FromFile("Resources/Images/Entities/Player/player_run_left_2.png");
        private readonly Image playerJumpLeft = Image.FromFile("Resources/Images/Entities/Player/player_jump_left.png");
        private readonly Image playerJumpRight = Image.FromFile("Resources/Images/Entities/Player/player_jump_right.png");
        private readonly Image playerDashLeft = Image.FromFile("Resources/Images/Entities/Player/player_dash_left.png");
        private readonly Image playerDashRight = Image.FromFile("Resources/Images/Entities/Player/player_dash_right.png");
        private readonly Image playerAttackLeft = Image.FromFile("Resources/Images/Entities/Player/player_attack_left.png");
        private readonly Image playerAttackRight = Image.FromFile("Resources/Images/Entities/Player/player_attack_right.png");
        private readonly Image slash = Image.FromFile("Resources/Images/Entities/Player/slash.png");
        private readonly Image energyAttackRight = Image.FromFile("Resources/Images/Entities/Player/energyAttack_right.png");
        private readonly Image energyAttackLeft = Image.FromFile("Resources/Images/Entities/Player/energyAttack_left.png");

        private readonly Image energy100 = Image.FromFile("Resources/Images/GUI/energy_100.png");
        private readonly Image energy90 = Image.FromFile("Resources/Images/GUI/energy_90.png");
        private readonly Image energy80 = Image.FromFile("Resources/Images/GUI/energy_80.png");
        private readonly Image energy70 = Image.FromFile("Resources/Images/GUI/energy_70.png");
        private readonly Image energy60 = Image.FromFile("Resources/Images/GUI/energy_60.png");
        private readonly Image energy50 = Image.FromFile("Resources/Images/GUI/energy_50.png");
        private readonly Image energy40 = Image.FromFile("Resources/Images/GUI/energy_40.png");
        private readonly Image energy30 = Image.FromFile("Resources/Images/GUI/energy_30.png");
        private readonly Image energy20 = Image.FromFile("Resources/Images/GUI/energy_20.png");
        private readonly Image energy10 = Image.FromFile("Resources/Images/GUI/energy_10.png");
        private readonly Image energy0 = Image.FromFile("Resources/Images/GUI/energy_0.png");
        private readonly Image heartFull = Image.FromFile("Resources/Images/GUI/heart_full.png");
        private readonly Image heartEmpty = Image.FromFile("Resources/Images/GUI/heart_empty.png");

    public void Render(Graphics g, int screenWidth, int screenHeight)
    {
        int bgWidth = background.Width;
        int bgHeight = background.Height;

        g.Clear(Color.Black);

        var cameraOffset = -_world.CameraY;

        var offsetY = (cameraOffset % bgHeight);
        for (var y = offsetY - bgHeight; y < screenHeight + bgHeight; y += bgHeight)
            for (var x = 0; x < screenWidth + bgWidth; x += bgWidth - 2)
                g.DrawImage(background, x, y, bgWidth, bgHeight);

        foreach (var platform in _world.Platforms)
        {
            if (platform.Y + cameraOffset + platform.Height > 0 &&
                platform.Y + cameraOffset < screenHeight + GameSettings.PlatformHeight)
            {
                var tileWidth = 63; 
                var tileCount = platform.Width / tileWidth;
                for (var i = 0; i < tileCount; i++)
                {
                    g.DrawImage(platformBlock,
                        platform.X + i * tileWidth,
                        platform.Y + cameraOffset,
                        tileWidth,
                        platform.Height);
                }
            }
        }

        foreach (var window in _world.Windows)
        {
            if (window.Y + cameraOffset + Window.Height > 0 &&
                window.Y + cameraOffset < screenHeight + Window.Height)
            {
                g.DrawImage(this.window,
                    window.X,
                    window.Y + cameraOffset,
                    Window.Width,
                    Window.Height);
            }
        }

        if (_world.Player.IsAttacking)
        {
            var slash = _world.Player.Slash;
            var rotation = 0f;

            switch (slash.Direction)
            {
                case AttackDirection.Up:
                    rotation = 270f;
                    break;
                case AttackDirection.Down:
                    rotation = 90f;
                    break;
                case AttackDirection.Left:
                    rotation = 180f;
                    break;
                case AttackDirection.Right:
                    rotation = 0f;
                    break;
            }

            var state = g.Save();

            g.TranslateTransform(
                slash.X + slash.Width / 2,
                slash.Y + slash.Height / 2 + cameraOffset);

            g.RotateTransform(rotation);

            g.DrawImage(
                this.slash,
                -slash.Width / 2,
                -slash.Height / 2,
                slash.Width,
                slash.Height);

            g.Restore(state);
        }

        if (_world.Player.IsInvincible && (int)(_world.Player.invincibilityTimer * 20) % 2 == 0)
        {

        }
        else
        {
            Image playerImage = _world.Player.IsFacingRight ? playerIdleRight : playerIdleLeft;
            if (_world.Player.IsHealing)
                playerImage = _world.Player.IsFacingRight ? playerHealingRight : playerHealingLeft;
            else if (_world.Player.Slash.IsAttacking)
                playerImage = _world.Player.IsFacingRight ? playerAttackRight : playerAttackLeft;
            else if (!_world.Player.IsGrounded && (_world.Player.IsHoldingJump || _world.Player.VelocityY != 0))
                playerImage = _world.Player.IsFacingRight ? playerJumpRight : playerJumpLeft;
            else if (_world.Player.IsDashing)
                playerImage = _world.Player.IsFacingRight ? playerDashRight : playerDashLeft;
            else if (_world.Player.VelocityX > 0.1f)
            {
                playerImage = _world.Player.IsRunningAnimationFrame1 ?
                    playerRunRight1 : playerRunRight2;
            }
            else if (_world.Player.VelocityX < -0.1f)
            {
                playerImage = _world.Player.IsRunningAnimationFrame1 ?
                    playerRunLeft1 : playerRunLeft2;
            }
            if (_world.Player.Slash.IsAttacking)
            {
                var X = _world.Player.IsFacingRight ? _world.Player.X - 20: _world.Player.X - 15;
                g.DrawImage(playerImage,
                        X,
                        _world.Player.Y + cameraOffset,
                        playerImage.Width,
                        playerImage.Height);
            }
            else
            {
                g.DrawImage(playerImage,
                        _world.Player.X,
                        _world.Player.Y + cameraOffset,
                        playerImage.Width,
                        playerImage.Height);
            }
        }

        foreach (var energyAttack in _world.EnergyAttacks)
        {
            if (energyAttack.Y + cameraOffset + energyAttack.Height > 0 &&
                energyAttack.Y + cameraOffset < screenHeight)
            {
                g.DrawImage(energyAttack.IsFacingRight ? energyAttackRight : energyAttackLeft,
                        energyAttack.X,
                        energyAttack.Y + cameraOffset,
                        energyAttackRight.Width,
                        energyAttackRight.Height);
                }
        }


        foreach (var enemy in _world.Enemies)
        {
                if (enemy.Y + cameraOffset + enemy.Height > 0 &&
                    enemy.Y + cameraOffset < screenHeight)
                {
                    if (enemy is Bat bat)
                    {
                        Image batImage;

                        if (bat.IsFacingRight)
                        {
                            batImage = bat.isAnimationFrameUp ? (enemy.IsKnockback ? batRightUpDamage : batRightUp) :
                                (enemy.IsKnockback ? batRightDownDamage : batRightDown);
                        }
                        else
                        {
                            batImage = bat.isAnimationFrameUp ? (enemy.IsKnockback ? batLeftUpDamage : batLeftUp) :
                                (enemy.IsKnockback ? batLeftDownDamage : batLeftDown);
                        }

                        g.DrawImage(batImage,
                            bat.X,
                            bat.Y + cameraOffset,
                            bat.Width,
                            bat.Height);
                    }

                    if (enemy is Skeleton skeleton)
                    {
                        Image skeletonImage;
                        var xOffset = 0;
                        string textureBaseName = "";

                        if (Math.Abs(enemy.VelocityX) > 0.1f)
                        {
                            textureBaseName = enemy.IsAnimationFrame1 ?
                                (enemy.VelocityX > 0 ? "SkeletonRunRight1" : "SkeletonRunLeft1") :
                                (enemy.VelocityX > 0 ? "SkeletonRunRight2" : "SkeletonRunLeft2");
                            xOffset = enemy.IsFacingRight ?
                                (enemy.IsAnimationFrame1 ? -74 : -64) :
                                0;
                        }
                        else if (enemy.IsAttacking)
                        {
                            textureBaseName = enemy.IsFacingRight ? "SkeletonAttackRight" : "SkeletonAttackLeft";
                            xOffset = enemy.IsFacingRight ? 0 : -130;
                        }
                        else
                        {
                            textureBaseName = enemy.IsFacingRight ? "SkeletonIdleRight" : "SkeletonIdleLeft";
                            xOffset = enemy.IsFacingRight ? -55 : 0;
                        }

                        if (enemy.IsKnockback)
                            textureBaseName += "Damage";

                        skeletonImage = GetSkeletonImageByName(textureBaseName);
                        g.DrawImage(
                            skeletonImage,
                            enemy.X + xOffset,
                            enemy.Y + cameraOffset,
                            skeletonImage.Width,
                            skeletonImage.Height
                        );
                    }
                }        
        }

        var distance = 20;
        for (var i = 0; i < 3; i++)
        {
            var heartImage = i < _world.Player.Health ? heartFull : heartEmpty;
            g.DrawImage(heartImage, 150 + distance, 20);
            distance += 80;
        }

        Image energyImage;
        var energyLevel = _world.Player.Energy;
        if (energyLevel == 100)
            energyImage = energy100;
        else if (energyLevel >= 90)
            energyImage = energy90;
        else if (energyLevel >= 80)
            energyImage = energy80;
        else if (energyLevel >= 70)
            energyImage = energy70;
        else if (energyLevel >= 60)
            energyImage = energy60;
        else if (energyLevel >= 50)
            energyImage = energy50;
        else if (energyLevel >= 40)
            energyImage = energy40;
        else if (energyLevel >= 30)
            energyImage = energy30;
        else if (energyLevel >= 20)
            energyImage = energy20;
        else if (energyLevel >= 10)
            energyImage = energy10;
        else
            energyImage = energy0;

        g.DrawImage(energyImage, 10, 10, 150, 150);

        g.DrawString($"Score: {_world.Score}",
                new Font("Stencil", 20),
                Brushes.White,
                20, 170);

        g.DrawString($"Height: {_world.Height}m",
            new Font("Stencil", 20),
            Brushes.White,
            20, 215);

            if (_world.IsGameOver)
            {
                string gameOverText = "GAME OVER";
                var gameOverFont = new Font("Stencil", 48, FontStyle.Bold);
                SizeF gameOverSize = g.MeasureString(gameOverText, gameOverFont);

                string scoreText = $"Score: {_world.Score}";
                string heightText = $"Height: {_world.Height}m";
                string exitText = "Press Enter to exit";

                var infoFont = new Font("Stencil", 24, FontStyle.Bold);
                SizeF scoreSize = g.MeasureString(scoreText, infoFont);
                SizeF heightSize = g.MeasureString(heightText, infoFont);
                SizeF exitSize = g.MeasureString(exitText, infoFont);

                float totalHeight = gameOverSize.Height + scoreSize.Height + heightSize.Height + 30; // 30 - отступы
                float startY = (screenHeight - totalHeight) / 2;

                g.DrawString(gameOverText, gameOverFont, Brushes.Red,
                    (screenWidth - gameOverSize.Width) / 2,
                    startY);

                g.DrawString(scoreText, infoFont, Brushes.White,
                    (screenWidth - scoreSize.Width) / 2,
                    startY + gameOverSize.Height + 10);

                g.DrawString(heightText, infoFont, Brushes.White,
                    (screenWidth - heightSize.Width) / 2,
                    startY + gameOverSize.Height + scoreSize.Height + 20);

                g.DrawString(exitText, infoFont, Brushes.DarkGray,
                    (screenWidth - exitSize.Width) / 2,
                    startY + gameOverSize.Height + heightSize.Height + 80);
            }
        }

        private Image GetSkeletonImageByName(string imageName)
        {
            switch (imageName)
            {
                case "SkeletonAttackRight":
                    return skeletonAttackRight;
                case "SkeletonAttackLeft":
                    return skeletonAttackLeft;
                case "SkeletonRunRight1":
                    return skeletonRunRight1;
                case "SkeletonRunRight1Damage":
                    return skeletonRunRight1Damage;
                case "SkeletonRunRight2":
                    return skeletonRunRight2;
                case "SkeletonRunRight2Damage":
                    return skeletonRunRight2Damage;
                case "SkeletonRunLeft1":
                    return skeletonRunLeft1;
                case "SkeletonRunLeft1Damage":
                    return skeletonRunLeft1Damage;
                case "SkeletonRunLeft2":
                    return skeletonRunLeft2;
                case "SkeletonRunLeft2Damage":
                    return skeletonRunLeft2Damage;
                case "SkeletonIdleRight":
                    return skeletonIdleRight;
                case "SkeletonIdleRightDamage":
                    return skeletonIdleRightDamage;
                case "SkeletonIdleLeft":
                    return skeletonIdleLeft;
                case "SkeletonIdleLeftDamage":
                    return skeletonIdleLeftDamage;
                case "SkeletonAttackLeftDamage":
                    return skeletonAttackLeftDamage;
                case "SkeletonAttackRightDamage":
                    return skeletonAttackRightDamage;
                default:
                    return skeletonIdleRight; 
            }
        }
    }
}