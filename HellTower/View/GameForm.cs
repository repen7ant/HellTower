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

            mainMenu.buttons[0].Action = () =>
            {
                currentState = GameState.Playing;
                _controller.StartGame();
            };
            mainMenu.buttons[1].Action = () =>
            {
                currentState = GameState.Options;
                Cursor.Show();
            };
            mainMenu.buttons[2].Action = () => this.Close();

            pauseMenu.buttons[0].Action = () => currentState = GameState.Playing;
            pauseMenu.buttons[1].Action = () =>
            {
                currentState = GameState.Options;
                Cursor.Show();
            };
            pauseMenu.buttons[2].Action = () =>
            {
                currentState = GameState.MainMenu;
                pauseMenu.SetFirstSelected();
                _controller.ResetGame();
            };

            optionsMenu.buttons[0].Action = () =>
            {
                if (_controller.IsGameRunning)
                    currentState = GameState.Paused;
                else
                    currentState = GameState.MainMenu;
                Cursor.Hide();
            };

            InitializeForm();
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
                _controller.ResetPressedKeys();
            }
            else if (_controller.IsGameOver && e.KeyCode == Keys.Enter)
            {
                currentState = GameState.MainMenu;
                pauseMenu.SetFirstSelected();
                _controller.ResetGame();
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
                    if (_controller.IsGameRunning)
                        currentState = GameState.Paused;
                    else
                        currentState = GameState.MainMenu;
                    Cursor.Hide();
                    break;
                    //добавить настройку звука
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