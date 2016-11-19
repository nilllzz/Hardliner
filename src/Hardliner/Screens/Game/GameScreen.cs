using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardliner.Engine;
using Hardliner.Engine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hardliner.Engine.Rendering.Sprites;
using static Core;

namespace Hardliner.Screens.Game
{
    internal class GameScreen : Screen
    {
        private ObjectRenderer _floorRenderer = new ObjectRenderer();
        private Player _player;
        private Camera _camera;
        private FirstPersonUI _ui;

        internal override void Activate(Screen preScreen)
        {
            base.Activate(preScreen);

            _floorRenderer.LoadContent(GameInstance.GraphicsDevice);

            var floor = new TestFloor(Content);
            floor.LoadContent(GameInstance.GraphicsDevice);
            _floorRenderer.Objects.Add(floor);

            var log = new TestTreeLog(Content, new Vector3(0, 1, 0));
            log.LoadContent(GameInstance.GraphicsDevice);
            _floorRenderer.Objects.Add(log);

            _player = new Player(new Vector3(0, 4, 0));
            _player.LoadContent(GameInstance.GraphicsDevice);
            _floorRenderer.Objects.Add(_player);

            _ui = new FirstPersonUI(Content, _player);
            _ui.LoadContent(GameInstance.GraphicsDevice);
            _floorRenderer.Objects.Add(_ui);

            _camera = new FirstPersonCamera(GameInstance.GraphicsDevice, _player);
            //_camera = new ObserverCamera(GameInstance.GraphicsDevice);
        }

        internal override void Draw()
        {
            _ui?.DrawTexture();
        }

        internal override void Render()
        {
            _floorRenderer.Draw(_camera);
        }
        
        internal override void Update()
        {
            _floorRenderer.Update();
            _camera.Update();
        }
    }
}
