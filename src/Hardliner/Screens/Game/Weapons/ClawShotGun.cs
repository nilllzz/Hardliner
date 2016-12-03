using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Hardliner.Screens.Game.Weapons
{
    internal class ClawShotGun
    {
        private Player _player;
        private Level _level;
        private bool _appliedCooldown = false;

        internal int Cooldown { get; private set; }
        internal ClawRope ActiveRope { get; private set; }

        public ClawShotGun(Level level, Player player)
        {
            _player = player;
            _level = level;
        }

        internal void Update()
        {
            var gState = GamePad.GetState(PlayerIndex.One);
            var buttonPressed = gState.IsButtonDown(Buttons.RightShoulder);
            var hasRope = _level.HasRope;

            if (buttonPressed)
            {
                if (!hasRope )
                {
                    if (Cooldown == 0)
                    {
                        SpawnRope();
                    }
                }
                else
                {
                    UpdateRope();
                }
            }
            else
            {
                if (hasRope)
                {
                    if (ActiveRope.Status == ClawStatus.ClawHit)
                        if (_player.JetPackCharge < Player.MAX_JUMPCHARGE / 2f)
                            _player.JetPackCharge = Player.MAX_JUMPCHARGE / 2f;

                    DespawnRope();
                }
            }

            if (Cooldown > 0)
            {
                Cooldown--;
            }
        }

        private void UpdateRope()
        {
            if (ActiveRope.Status == ClawStatus.ClawHit && !_appliedCooldown)
            {
                _appliedCooldown = true;
                Cooldown = 30;
            }
            else if (ActiveRope.Status == ClawStatus.Intercepted ||
                ActiveRope.Status == ClawStatus.OutOfRange)
            {
                DespawnRope();
                Cooldown = 30;
            }
        }

        private void SpawnRope()
        {
            var rope = new ClawRope(_level, _player);
            ActiveRope = rope;
            _appliedCooldown = false;
            _level.AddObject(rope);
        }

        private void DespawnRope()
        {
            _level.RemoveObject(_level.Objects.First(o => o is ClawRope));
        }
    }
}
