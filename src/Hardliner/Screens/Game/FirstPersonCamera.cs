﻿using Hardliner.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Core;

namespace Hardliner.Screens.Game
{
    internal class FirstPersonCamera : Camera
    {
        private Player _player;

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
            Position = _player.Position + new Vector3(0, Player.HEIGHT, 0);
            Yaw = _player.Yaw;
            Pitch = _player.Pitch;

            CreateView();
        }
    }
}
