using HellTower.Controller;
using System.Drawing;
using System.Windows.Forms;

namespace HellTower.View
{
    partial class GameForm : Form
    {
        private readonly GameController _controller;
        private GameState currentState = GameState.MainMenu;
        private readonly MainMenu mainMenu;
        private readonly PauseMenu pauseMenu;
        private readonly OptionsMenu optionsMenu;

        public GameForm(GameController controller)
        {
            _controller = controller;
            mainMenu = new MainMenu();
            pauseMenu = new PauseMenu();
            optionsMenu = new OptionsMenu();

            optionsMenu.MusicVolumeChanged += v => _controller.SetMusicVolume(v);
            optionsMenu.EffectsVolumeChanged += v => _controller.SetEffectsVolume(v);


            mainMenu.buttons[0].Action = () =>
            {
                currentState = GameState.Playing;
                _controller.StartGame();
                _controller.StopMusic();
                _controller.PlayGameMusic();
            };
            mainMenu.buttons[1].Action = () =>
            {
                currentState = GameState.Options;
                Cursor.Show();
                optionsMenu.AddControlsToForm(this);
            };
            mainMenu.buttons[2].Action = () => this.Close();

            pauseMenu.buttons[0].Action = () =>
            {
                currentState = GameState.Playing;
                _controller.ResumeGameMusic();
            };
                pauseMenu.buttons[1].Action = () =>
            {
                currentState = GameState.Options;
                Cursor.Show();
                optionsMenu.AddControlsToForm(this);
            };
            pauseMenu.buttons[2].Action = () =>
            {
                currentState = GameState.MainMenu;
                pauseMenu.SetFirstSelected();
                _controller.ResetGame();
                _controller.StopMusic();
                _controller.PlayMenuMusic();
            };

            optionsMenu.buttons[0].Action = () =>
            {
                optionsMenu.RemoveControlsFromForm(this);
                if (_controller.IsGameRunning)
                    currentState = GameState.Paused;
                else
                    currentState = GameState.MainMenu;
                Cursor.Hide();
            };

            InitializeForm();
            _controller.PlayMenuMusic();
        }

        private void InitializeForm()
        {
            this.Text = "Hell Tower";
            this.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            Cursor.Hide();

            var timer = new Timer { Interval = 16 }; // ~60 FPS
            timer.Tick += (s, e) =>
            {
                if (currentState == GameState.Playing)
                    _controller.Update();
                this.Invalidate();
            };
            timer.Start();

            this.Paint += (s, e) =>
            {
                switch (currentState)
                {
                    case GameState.MainMenu:
                        mainMenu.Draw(e.Graphics, this.Width, this.Height);
                        break;
                    case GameState.Playing:
                        _controller.Render(e.Graphics);
                        break;
                    case GameState.Paused:
                        _controller.Render(e.Graphics);
                        pauseMenu.Draw(e.Graphics, this.Width, this.Height);
                        break;
                    case GameState.Options:
                        optionsMenu.Draw(e.Graphics, this.Width, this.Height);
                        break;
                }
            };

            this.KeyDown += (s, e) =>
            {
                switch (currentState)
                {
                    case GameState.MainMenu:
                        HandleMainMenuInput(e);
                        break;
                    case GameState.Playing:
                        HandleGameInput(e);
                        break;
                    case GameState.Paused:
                        HandlePauseMenuInput(e);
                        break;
                    case GameState.Options:
                        HandleOptionsInput(e);
                        break;
                }
            };

            this.KeyUp += (s, e) =>
            {
                if (currentState == GameState.Playing)
                    _controller.OnKeyUp(s, e);
            };
        }

        private void HandleMainMenuInput(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.W:
                    mainMenu.SelectPrevious();
                    break;
                case Keys.Down:
                case Keys.S:
                    mainMenu.SelectNext();
                    break;
                case Keys.Enter:
                case Keys.Space:
                    mainMenu.ExecuteSelected();
                    break;
                case Keys.Escape:
                    this.Close();
                    break;
            }
        }

        private void HandleGameInput(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && !_controller.IsGameOver)
            {
                currentState = GameState.Paused;
                _controller.PauseGameMusic();
                _controller.ResetPressedKeys();
            }
            else if (_controller.IsGameOver && e.KeyCode == Keys.Enter)
            {
                currentState = GameState.MainMenu;
                pauseMenu.SetFirstSelected();
                _controller.ResetGame();
                _controller.StopMusic();
                _controller.PlayMenuMusic();
            }   
            else
                _controller.OnKeyDown(this, e);
        }

        private void HandlePauseMenuInput(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.W:
                    pauseMenu.SelectPrevious();
                    break;
                case Keys.Down:
                case Keys.S:
                    pauseMenu.SelectNext();
                    break;
                case Keys.Enter:
                case Keys.Space:
                    pauseMenu.ExecuteSelected();
                    break;
                case Keys.Escape:
                    currentState = GameState.Playing;
                    _controller.ResumeGameMusic();
                    break;
            }
        }

        private void HandleOptionsInput(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                case Keys.Space:
                    optionsMenu.ExecuteSelected();
                    break;
                case Keys.Escape:
                    optionsMenu.RemoveControlsFromForm(this);
                    if (_controller.IsGameRunning)
                        currentState = GameState.Paused;
                    else
                        currentState = GameState.MainMenu;
                    Cursor.Hide();
                    break;
            }
        }
    }

    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        Options
    }
}