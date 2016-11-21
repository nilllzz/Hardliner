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
        private Player _player;
        private Camera _camera;
        private FirstPersonUI _ui;
        private Level _level;

        internal override void Activate(Screen preScreen)
        {
            base.Activate(preScreen);

            _level = new Level();

            var floor = new TestFloor(_level, Content);
            _player = new Player(_level, new Vector3(0, 0, 0), this);
            _ui = new FirstPersonUI(_level, Content, _player);

            var log = new TestTreeLog(_level, Content, new Vector3(0, 1, -5));
            var log1 = new TestTreeLog(_level, Content, new Vector3(0, 2, -8));
            var log2 = new TestTreeLog(_level, Content, new Vector3(0, 3, -12));

            _level.LoadContent(new LevelObject[] { floor, log, log1, log2, _player, _ui });

            _camera = new FirstPersonCamera(GameInstance.GraphicsDevice, _player);
            //_camera = new ObserverCamera(GameInstance.GraphicsDevice);
        }

        internal override void Draw()
        {
            _ui?.DrawTexture();
        }

        internal override void Render()
        {
            _level.Render(_camera);
        }
        
        internal override void Update()
        {
            _level.Update();
            _camera.Update();
        }
    }
}
