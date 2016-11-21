using System;
using Hardliner.Core;
using Hardliner.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Core;

namespace Hardliner.Screens.Game
{
    internal class FirstPersonCamera : Camera
    {
        private Player _player;
        private Random _shakeRandom = new Random();

        public FirstPersonCamera(GraphicsDevice device, Player player)
            : base(device)
        {
            GameInstance.ViewportSizeChanged += CreateProjection;
            FOVChanged += CreateProjection;

            _player = player;

            // create initial matrices
            CreateProjection();
            CreateView();
        }
        
        public override void Update()
        {
            Position = _player.Position + new Vector3(0, Player.HEIGHT, 0) + GetFallShake();
            Yaw = _player.Yaw;
            Pitch = _player.Pitch;
            Roll = _player.Roll;

            CreateView();

            var speed = -_player.Velocity.Z * 10f;
            var targetFOV = 90 - speed;
            FOV = MathHelper.Lerp(FOV, targetFOV, 0.5f);
        }

        private Vector3 GetFallShake()
        {
            float shake = -_player.Velocity.Y;
            if (shake > 0f)
                return _shakeRandom.NextUnitSphereVector() * shake * 0.01f;

            return Vector3.Zero;
        }
    }
}
