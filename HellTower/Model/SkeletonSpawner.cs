using HellTower.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HellTower.Model
{
    public class SkeletonSpawner
    {
        private readonly GameWorld _world;
        private readonly Random _random = new Random();
        private float _timer = 0;
        private readonly HashSet<Platform> _usedPlatforms = new HashSet<Platform>();

        public SkeletonSpawner(GameWorld world)
        {
            _world = world;
        }

        public void Update(float deltaTime)
        {
            _timer += deltaTime;
            var visiblePlatforms = _world.Platforms.Where(p =>
                p.Y > _world.CameraY - GameSettings.PlatformHeight &&
                p.Y < _world.CameraY + GameSettings.ScreenHeight &&
                !_usedPlatforms.Contains(p));
            foreach (var platform in visiblePlatforms)
                TrySpawnSkeletonOnPlatform(platform, deltaTime);
        }

        private void TrySpawnSkeletonOnPlatform(Platform platform, float deltaTime)
        {
            if (_world.Enemies.OfType<Skeleton>().Count() >= GameSettings.MaxSkeletons) 
                return;
            if (_timer < GameSettings.SkeletonSpawnCooldown) 
                return;
            if (_random.NextDouble() > GameSettings.SkeletonSpawnChance) 
                return;
            if (IsPlayerOnPlatform(platform) || IsSkeletonOnPlatform(platform)) 
                return;
            var skeleton = new Skeleton(_world)
            {
                X = platform.X + platform.Width / 2 - 50,
                Y = platform.Y - 150,
                Health = 5,
                Platform = platform 
            };
            _world.Enemies.Add(skeleton);
            _usedPlatforms.Add(platform); 
            _timer = 0;
        }

        private bool IsPlayerOnPlatform(Platform platform)
        {
            bool horizontalOverlap = _world.Player.X + _world.Player.Width > platform.X &&
                                   _world.Player.X < platform.X + platform.Width;
            bool verticalPosition = _world.Player.Y + _world.Player.Height >= platform.Y &&
                                  _world.Player.Y + _world.Player.Height <= platform.Y + 10;
            return horizontalOverlap && verticalPosition;
        }

        private bool IsSkeletonOnPlatform(Platform platform)
        {
            return _world.Enemies.OfType<Skeleton>()
                .Any(s => s.X + s.Width > platform.X &&
                         s.X < platform.X + platform.Width &&
                         s.Y + s.Height >= platform.Y &&
                         s.Y + s.Height <= platform.Y + 10);
        }
    }
}
