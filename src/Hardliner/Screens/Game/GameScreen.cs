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
using Hardliner.Screens.Game.Hub.BuildingParts.Generators;
using Hardliner.Screens.Game.Hub.BuildingParts.RoofTop;
using Hardliner.Screens.Game.Hub.BuildingParts.Street;
using Hardliner.Screens.Game.Hub.BuildingParts.General;

namespace Hardliner.Screens.Game
{
    internal class GameScreen : Screen
    {
        private Player _player;
        private FirstPersonUI _ui;
        private Level _level;

        internal Camera Camera { get; private set; }

        internal override void Activate(Screen preScreen)
        {
            base.Activate(preScreen);

            _level = new Level(this);

            var floor = new TestFloor(_level, Content);
            _player = new Player(_level, new Vector3(0, 100, 0), this);
            _ui = new FirstPersonUI(_level, Content, _player);
            
            var sky = new Hub.SkyCylinder(_level, _player);
            var light1 = new Hub.LightBarrier(_level, _player, 14);
            var light2 = new Hub.LightBarrier(_level, _player, 18);
            var light3 = new Hub.LightBarrier(_level, _player, 22);
            var light4 = new Hub.LightBarrier(_level, _player, 26);
            var light5 = new Hub.LightBarrier(_level, _player, 30);
            var light6 = new Hub.LightBarrier(_level, _player, 34);
            var introMachine = new Hub.IntroMachine.UpperPart(_level, Content);

            var objects = new List<LevelObject>();
            objects.AddRange(new LevelObject[] { _player, _ui,
                sky, light1, light2, light3, light4, light5, light6, introMachine });

            objects.AddRange(RoofTop1.Generate(_level, Content, 
                new Vector3(0, 20, 0), new Vector2(20, 10), RoofTopOptions.Fans | RoofTopOptions.Entrance));
            objects.Add(new Hub.BuildingParts.BuildingSide.Sides(_level, Content, 20, 20, 10, new Vector3(0, 0, 0)));

            objects.AddRange(RoofTop1.Generate(_level, Content,
                new Vector3(-40, 50, 0), new Vector2(30, 30), RoofTopOptions.Fans | RoofTopOptions.Entrance | RoofTopOptions.RedWarningLights));
            objects.Add(new Hub.BuildingParts.BuildingSide.Sides(_level, Content, 30, 50, 30, new Vector3(-40, 0, 0)));

            objects.AddRange(RoofTop1.Generate(_level, Content,
                new Vector3(30, 50, 0), new Vector2(30, 30), RoofTopOptions.Fans | RoofTopOptions.Entrance | RoofTopOptions.RedWarningLights));
            objects.Add(new Hub.BuildingParts.BuildingSide.Sides(_level, Content, 30, 50, 30, new Vector3(30, 0, 0)));

            objects.AddRange(RoofTop1.Generate(_level, Content,
                new Vector3(0, 60, -40), new Vector2(30, 30), RoofTopOptions.Fans | RoofTopOptions.Entrance | RoofTopOptions.RedWarningLights));
            objects.Add(new Hub.BuildingParts.BuildingSide.Sides(_level, Content, 30, 60, 30, new Vector3(0, 0, -40)));

            objects.AddRange(StreetLamp.Factory(_level, Content, new Vector3(-10, 90, 0), 0f));
            objects.AddRange(StreetLamp.Factory(_level, Content, new Vector3(-7, 90, 0), MathHelper.Pi));

            objects.Add(new Sidewalk(_level, Content, new Vector3(0, 0, -7f), new Vector2(40, 4)));
            objects.Add(new Street(_level, Content, new Vector3(0, 0, -13f), 40f, true));
            objects.Add(new Sidewalk(_level, Content, new Vector3(0, 0, -19f), new Vector2(40, 4)));

            objects.AddRange(StreetLamp.Factory(_level, Content, new Vector3(-15, 0, -8f), MathHelper.PiOver2));
            objects.AddRange(StreetLamp.Factory(_level, Content, new Vector3(0, 0, -8f), MathHelper.PiOver2));
            objects.AddRange(StreetLamp.Factory(_level, Content, new Vector3(15, 0, -8f), MathHelper.PiOver2));

            objects.AddRange(StreetLamp.Factory(_level, Content, new Vector3(-15, 0, -18f), -MathHelper.PiOver2));
            objects.AddRange(StreetLamp.Factory(_level, Content, new Vector3(0, 0, -18f), -MathHelper.PiOver2));
            objects.AddRange(StreetLamp.Factory(_level, Content, new Vector3(15, 0, -18f), -MathHelper.PiOver2));

            objects.Add(new WireFence(_level, Content, new Vector3(0, 0, -13f), 10f, false));
            objects.Add(new WireFence(_level, Content, new Vector3(0.1f, 0, -13f), 10f, true));

            objects.Add(new Trashcan(_level, Content, new Vector3(-5f, 0, -12f)));


            _level.LoadContent(objects.ToArray(), Content);

            Camera = new FirstPersonCamera(GameInstance.GraphicsDevice, _player);
            //_camera = new ObserverCamera(GameInstance.GraphicsDevice);
        }

        internal override void Draw()
        {
            _ui?.DrawTexture();
        }

        internal override void Render()
        {
            _level.Render(Camera);
        }
        
        internal override void Update()
        {
            _level.Update();
            Camera.Update();
        }
    }
}
