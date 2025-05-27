using HellTower.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HellTower.Model
{
    public class BatSpawner
    {
        private readonly GameWorld _world;
        private readonly Random _random = new Random();
        private float _timer = 0;

        public BatSpawner(GameWorld world)
        {
            _world = world;
        }

        public void Update(float deltaTime)
        {
            _timer += deltaTime;
            var visibleWindows = _world.Windows.Where(w =>
                w.Y > _world.CameraY - Window.Height &&
                w.Y < _world.CameraY + GameSettings.ScreenHeight);
            foreach (var window in visibleWindows)
                TrySpawnBatFromWindow(window, deltaTime);
        }

        private void TrySpawnBatFromWindow(Window window, float deltaTime)
        {
            if (window.BatsSpawned >= GameSettings.MaxBatsFromWindow) 
                return;
            if (_timer - window.LastSpawnTime < GameSettings.BatSpawnCooldown) 
                return;
            if (_random.NextDouble() > GameSettings.BatSpawnChance) 
                return;
            if (_world.Player.Y > window.Y) 
                return;
            var bat = new Bat
            {
                X = window.X + Window.Width / 2 - 50 / 2,
                Y = window.Y + Window.Height / 2 - 35 / 2,
                Health = 3
            };
            _world.Enemies.Add(bat);
            window.BatsSpawned++;
            window.LastSpawnTime = _timer;
        }
    }
}
