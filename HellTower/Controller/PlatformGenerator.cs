using HellTower.Model;
using System;
using System.Linq;

namespace HellTower.Controller
{
    public class PlatformGenerator
    {
        private readonly GameWorld _world;
        private int highestPlatformY;
        private readonly Random _random = new Random();

        public PlatformGenerator(GameWorld world)
        {
            _world = world;
            highestPlatformY = GameSettings.ScreenHeight;
        }

        public void Reset() => highestPlatformY = GameSettings.ScreenHeight;

        public void GenerateInitialPlatforms()
        {
            int blockCount = GameSettings.ScreenWidth / GameSettings.BlockWidth + 1;
            var width = blockCount * GameSettings.BlockWidth;
            _world.Platforms.Add(new Platform
            {
                X = 0,
                Y = GameSettings.ScreenHeight + 10,
                Width = width
            });
            for (var i = 0; i < 10; i++)
                GenerateNewPlatform();
        }

        public void GenerateNewPlatformsIfNeeded()
        {
            while (highestPlatformY > _world.CameraY - GameSettings.ScreenHeight)
                GenerateNewPlatform();
        }

        private void GenerateNewPlatform()
        {
            int y = highestPlatformY - _random.Next(GameSettings.PlatformMinVerticalDistance, GameSettings.PlatformMaxVerticalDistance);
            int platformCount = _random.Next(1, 4);
            int sectionWidth = GameSettings.ScreenWidth / platformCount;
            int minBlocks = GameSettings.PlatformMinWidth / GameSettings.BlockWidth; // 6 блоков
            int maxBlocks = GameSettings.PlatformMaxWidth / GameSettings.BlockWidth; // 13 блоков
            for (var i = 0; i < platformCount; i++)
            {
                int maxBlocksInSection = (sectionWidth - 20) / GameSettings.BlockWidth;
                maxBlocks = Math.Min(maxBlocks, maxBlocksInSection);
                int blockCount = _random.Next(minBlocks, maxBlocks + 1);
                int width = blockCount * GameSettings.BlockWidth;
                if (platformCount == 1)
                {
                    maxBlocks = (GameSettings.ScreenWidth - 100) / GameSettings.BlockWidth;
                    blockCount = _random.Next(maxBlocks / 2, maxBlocks + 1);
                    width = blockCount * GameSettings.BlockWidth;
                }
                int sectionStart = i * sectionWidth;
                int sectionEnd = (i + 1) * sectionWidth;
                int maxX = sectionEnd - width;
                int x = _random.Next(sectionStart, maxX);
                x = (x / GameSettings.BlockWidth) * GameSettings.BlockWidth;
                int minGap = 60;
                if (x + width + minGap > sectionEnd)
                {
                    width = sectionEnd - x - minGap;
                    blockCount = width / GameSettings.BlockWidth;
                    width = blockCount * GameSettings.BlockWidth;

                    if (width < GameSettings.PlatformMinWidth)
                    {
                        blockCount = GameSettings.PlatformMinWidth / GameSettings.BlockWidth;
                        width = blockCount * GameSettings.BlockWidth;
                        x = sectionEnd - width - minGap;
                        x = (x / GameSettings.BlockWidth) * GameSettings.BlockWidth;
                    }
                }
                int platformY = y + _random.Next(-50, 50);
                _world.Platforms.Add(new Platform
                {
                    X = x,
                    Y = platformY,
                    Width = width
                });
            }
            highestPlatformY = y;
        }
    }
}