﻿using HellTower.Model;
using HellTower.Model.Entity;
using HellTower.View;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HellTower.Controller
{
    public class GameController
    {
        private readonly GameWorld _world;
        private readonly GameRenderer _renderer;
        private readonly InputHandler _inputHandler;
        private readonly PlatformGenerator _platformGenerator;
        private readonly WindowGenerator _windowGenerator;
        private readonly BatSpawner _batSpawner;
        private readonly SkeletonSpawner _skeletonSpawner;
        private readonly Audio.SoundManager _soundManager;
        public bool IsGameRunning { get; private set; }
        public bool IsGameOver => _world.IsGameOver;

        public GameController()
        {
            _world = new GameWorld();
            _renderer = new GameRenderer(_world);
            _inputHandler = new InputHandler(_world, new Audio.SoundManager("Resources/Audio"));
            _platformGenerator = new PlatformGenerator(_world);
            _windowGenerator = new WindowGenerator(_world);
            _batSpawner = new BatSpawner(_world);
            _skeletonSpawner = new SkeletonSpawner(_world);
            _soundManager = new Audio.SoundManager("Resources/Audio");
        }

        public void ResetGame()
        {
            _world.Reset();
            ResetPressedKeys();
            _platformGenerator.Reset();
            IsGameRunning = false;
        }

        public void ResetPressedKeys() => _inputHandler.ResetPressedKeys();

        public void StartGame()
        {
            _world.Reset();
            _inputHandler.ResetPressedKeys();
            _platformGenerator.Reset();
            InitializeGame();
            IsGameRunning = true;
        }

        private void InitializeGame()
        {
            _world.Player = new Player
            {
                X = GameSettings.ScreenWidth / 2 - 25,
                Y = GameSettings.ScreenHeight - 150
            };
            _platformGenerator.GenerateInitialPlatforms();
            _windowGenerator.GenerateInitialWindow();
        }

        public void Update()
        {
            if (!_world.IsGameOver)
            {
                _inputHandler.Update();
                _world.Update();
                _platformGenerator.GenerateNewPlatformsIfNeeded();
                _windowGenerator.GenerateWindows();
                _batSpawner.Update(0.016f);
                _skeletonSpawner.Update(0.016f);
            }
            else
                IsGameRunning = false;
        }

        public void Render(Graphics g) =>_renderer.Render(g, GameSettings.ScreenWidth, GameSettings.ScreenHeight);

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!_world.IsGameOver)
                _inputHandler.AddPressedKey(e.KeyCode);
        }

        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (!_world.IsGameOver)
                _inputHandler.RemovePressedKey(e.KeyCode);
        }

        public void PlayMenuMusic() => _soundManager.PlayMusic("menu_music.mp3");
        public void PlayGameMusic() => _soundManager.PlayMusic("game_music.mp3");
        public void PauseGameMusic() => _soundManager.PauseMusic();
        public void ResumeGameMusic() => _soundManager.ResumeMusic();
        public void StopMusic() => _soundManager.StopMusic();
        public void SetMusicVolume(float v) => _soundManager.SetMusicVolume(v);
        public void SetEffectsVolume(float v) => _soundManager.SetEffectsVolume(v);
        public void PlayEffect(string effect) => _soundManager.PlayEffect(effect);
    }
}