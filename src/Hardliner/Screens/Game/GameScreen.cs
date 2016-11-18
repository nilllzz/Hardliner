using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardliner.Engine;
using Hardliner.Engine.Rendering;
using Microsoft.Xna.Framework;
using static Core;

namespace Hardliner.Screens.Game
{
    internal class GameScreen : Screen
    {
        private ObjectRenderer _floorRenderer = new ObjectRenderer();
        private Player _player;
        private Camera _camera;

        internal override void Activate(Screen preScreen)
        {
            base.Activate(preScreen);
            
            _floorRenderer.LoadContent(GameInstance.GraphicsDevice);

            var floor = new TestFloor(Content);
            floor.LoadContent(GameInstance.GraphicsDevice);
            _floorRenderer.Objects.Add(floor);

            var random = new Random();
            for (int i = 0; i < 100; i++)
            {
                var log = new TestTreeLog(Content, new Vector3(random.Next(-10, 10), 1, random.Next(-10, 10)));
                log.LoadContent(GameInstance.GraphicsDevice);
                _floorRenderer.Objects.Add(log);
            }
            
            _player = new Player(new Vector3(0, 4, 0));
            _player.LoadContent(GameInstance.GraphicsDevice);
            _floorRenderer.Objects.Add(_player);

            // _camera = new PlayerCamera(GameInstance.GraphicsDevice, _player);
            _camera = new ObserverCamera(GameInstance.GraphicsDevice);
        }

        internal override void Draw()
        {
            _floorRenderer.Draw(_camera);
        }

        internal override void Update()
        {
            _player.Update();
            _camera.Update();
            _floorRenderer.Update();
        }
    }
}
