using System;
using System.Drawing;
using System.Windows.Forms;

namespace HellTower.View
{
    public class OptionsMenu : Menu
    {
        private Slider musicSlider;
        private Slider effectsSlider;
        private Label musicLabel;
        private Label effectsLabel;

        public float MusicVolume => musicSlider.Value / 100f;
        public float EffectsVolume => effectsSlider.Value / 100f;

        public event Action<float> MusicVolumeChanged;
        public event Action<float> EffectsVolumeChanged;

        public OptionsMenu() : base("Resources/Images/Background/menu_background.png")
        {
            buttons.Add(new MenuButton("Back", Rectangle.Empty, null));

            musicSlider = new Slider { Minimum = 0, Maximum = 100, Value = 100, Width = 300, Height = 30 };
            effectsSlider = new Slider { Minimum = 0, Maximum = 100, Value = 100, Width = 300, Height = 30 };

            musicLabel = new Label
            {
                Text = "Music Volume",
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("Stencil", 18, FontStyle.Bold),
                AutoSize = true
            };
            effectsLabel = new Label
            {
                Text = "Effects Volume",
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("Stencil", 18, FontStyle.Bold),
                AutoSize = true
            };

            musicSlider.ValueChanged += (v) => MusicVolumeChanged?.Invoke(MusicVolume);
            effectsSlider.ValueChanged += (v) => EffectsVolumeChanged?.Invoke(EffectsVolume);
        }

        public void AddControlsToForm(Form form)
        {
            if (!form.Controls.Contains(musicSlider))
            {
                int screenWidth = form.Width;
                int screenHeight = form.Height;

                musicSlider.Location = new Point((screenWidth - musicSlider.Width) / 2, screenHeight / 2 + 100);
                musicLabel.Location = new Point(musicSlider.Left, musicSlider.Top - 35);

                effectsSlider.Location = new Point((screenWidth - effectsSlider.Width) / 2, screenHeight / 2 + 190);
                effectsLabel.Location = new Point(effectsSlider.Left, effectsSlider.Top - 35);

                form.Controls.Add(musicSlider);
                form.Controls.Add(musicLabel);
                form.Controls.Add(effectsSlider);
                form.Controls.Add(effectsLabel);
            }
        }

        public void RemoveControlsFromForm(Form form)
        {
            if (form.Controls.Contains(musicSlider))
            {
                form.Controls.Remove(musicSlider);
                form.Controls.Remove(musicLabel);
                form.Controls.Remove(effectsSlider);
                form.Controls.Remove(effectsLabel);
            }
        }

        public override void Draw(Graphics g, int screenWidth, int screenHeight)
        {
            base.Draw(g, screenWidth, screenHeight);

            var buttonWidth = 200;
            var buttonHeight = 50;

            buttons[0].Bounds = new Rectangle(
                (screenWidth - buttonWidth) / 2,
                screenHeight - 100,
                buttonWidth,
                buttonHeight);

            buttons[0].IsSelected = true;

            var titleFont = new Font("Stencil", 36, FontStyle.Bold);
            g.DrawString("OPTIONS AND CONTROLS", titleFont, Brushes.White,
                (screenWidth - g.MeasureString("OPTIONS AND CONTROLS", titleFont).Width) / 2,
                100);

            var controlsFont = new Font("Stencil", 18, FontStyle.Bold);
            string controlsText = "CONTROLS:\n\n" +
                                 "Move Left: A\n" +
                                 "Move Right: D\n" +
                                 "Jump: Space\n" +
                                 "Drop Down: S\n" +
                                 "Dash: Shift\n" +
                                 "Attack: J\n" +
                                 "Energy Attack: K (20 energy)\n" +
                                 "Heal: Hold Ctrl (40 energy)\n" +
                                 "Menu Navigation: W/S or Arrows\n" +
                                 "Select: Enter or Space\n" +
                                 "Pause: Esc";

            g.DrawString(controlsText, controlsFont, Brushes.White, (screenWidth - g.MeasureString("CONTROLS:", controlsFont).Width) / 2 - 80, 200);
        }
    }
}
