using HellTower.Model;
using HellTower.Model.Entity;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HellTower.Controller
{
    public class InputHandler
    {
        private readonly GameWorld _world;
        private readonly HashSet<Keys> _pressedKeys = new HashSet<Keys>();
        private bool _wasLeftPressed;
        private bool _wasRightPressed;
        private readonly Audio.SoundManager _soundManager;

        public InputHandler(GameWorld world, Audio.SoundManager soundManager)
        {
            _world = world;
            _soundManager = soundManager;
        }

        public void ResetPressedKeys() => _pressedKeys.Clear();
        public void AddPressedKey(Keys key) => _pressedKeys.Add(key);
        public void RemovePressedKey(Keys key) => _pressedKeys.Remove(key);

        public void Update()
        {
            bool isLeftPressed = _pressedKeys.Contains(Keys.A) || _pressedKeys.Contains(Keys.Left);
            bool isRightPressed = _pressedKeys.Contains(Keys.D) || _pressedKeys.Contains(Keys.Right);
            if (isLeftPressed && !isRightPressed)
            {
                _world.Player.MoveLeft();
                _wasLeftPressed = true;
            }
            else if (isRightPressed && !isLeftPressed)
            {
                _world.Player.MoveRight();
                _wasRightPressed = true;
            }
            else if ((_wasLeftPressed && !isLeftPressed) || (_wasRightPressed && !isRightPressed))
            {
                _world.Player.StopMoving();
                _wasLeftPressed = false;
                _wasRightPressed = false;
            }
            if ((_pressedKeys.Contains(Keys.Space))
                && _world.Player.IsGrounded)
                _world.Player.Jump();

            if (!_pressedKeys.Contains(Keys.Space))
                _world.Player.IsHoldingJump = false;

            if (_pressedKeys.Contains(Keys.ShiftKey) && !_world.Player.IsDashing && _world.Player.CanDash)
            {
                var direction = _world.Player.IsFacingRight ? 1 : -1;
                _world.Player.Dash(direction);
                _soundManager.PlayEffect("dash_sound.wav");
            }

            if (_pressedKeys.Contains(Keys.S))
                _world.Player.DropDown();

            if (_pressedKeys.Contains(Keys.ControlKey))
                _world.Player.StartHealing();

            else if (_world.Player.IsHealing)
                _world.Player.InterruptHealing();

            if (_world.Player.IsInvincible && _world.Player.invincibilityTimer == GameSettings.InvincibilityDuration)
                _soundManager.PlayEffect("damage_sound.wav");

            if (_pressedKeys.Contains(Keys.J) && !_world.Player.IsAttacking)
            {
                _soundManager.PlayEffect("attack_sound.wav");
                AttackDirection direction;
                if (_pressedKeys.Contains(Keys.W))
                    direction = AttackDirection.Up;
                else if (_pressedKeys.Contains(Keys.S))
                    direction = AttackDirection.Down;
                else
                {
                    bool _isLeftPressed = _pressedKeys.Contains(Keys.A) || _pressedKeys.Contains(Keys.Left);
                    bool _isRightPressed = _pressedKeys.Contains(Keys.D) || _pressedKeys.Contains(Keys.Right);
                    if (_isLeftPressed)
                        direction = AttackDirection.Left;
                    else if (_isRightPressed)
                        direction = AttackDirection.Right;
                    else
                        direction = _world.Player.IsFacingRight ? AttackDirection.Right : AttackDirection.Left;
                }
                _world.Player.Attack(direction);
            }

            if (_pressedKeys.Contains(Keys.K) && _world.Player.CanUseEnergyAttack &&
                    !_world.Player.IsKnockback)
            {
                _soundManager.PlayEffect("energy_attack_sound.wav");
                _world.Player.Energy -= 20;
                _world.Player.EnergyAttackCooldownTimer = 0.25f; 
                var energyAttack = new EnergyAttack
                {
                    X = _world.Player.IsFacingRight ?
                        _world.Player.X + _world.Player.Width - 25:
                        _world.Player.X - 365,
                    Y = _world.Player.Y - 37,
                    IsFacingRight = _world.Player.IsFacingRight
                };
                _world.EnergyAttacks.Add(energyAttack);
            }
        }
    }
}
