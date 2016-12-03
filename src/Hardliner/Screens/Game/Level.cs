using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardliner.Content;
using Hardliner.Engine;
using Hardliner.Engine.Rendering;
using Hardliner.Engine.Rendering.Shaders;
using Hardliner.Screens.Game.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using static Core;

namespace Hardliner.Screens.Game
{
    internal class Level
    {
        private LevelObjectCarrier _objects = new LevelObjectCarrier();
        private ObjectRenderer _renderer = new ObjectRenderer();
        private GameScreen _screen;

        internal bool HasRope => _objects.Any(o => o is ClawRope);

        internal IEnumerable<LevelObject> Objects => _objects;

        public Level(GameScreen screen)
        {
            _screen = screen;
        }

        internal void LoadContent(LevelObject[] initialObjects, ContentManager content)
        {
            _objects.AddRange(initialObjects);

            _renderer.LoadContent(GameInstance.GraphicsDevice, new TexturedShader(content.Load<Effect>(Resources.Shaders.Textured)));
            _objects.ForEach(o => o.LoadContent(GameInstance.GraphicsDevice));
        }

        internal void Update()
        {
            lock (_objects)
            {
                for (int i = 0; i < _objects.Count(); i++)
                {
                    if (_objects[i].ToBeRemoved)
                    {
                        _objects[i].Dispose();
                        _objects.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        _objects[i].Update();
                    }
                }

                _objects.Sort();
            }
        }

        internal void Render(Camera camera)
        {
            GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            _renderer.Render(_objects.OpaqueObjects, camera);
            GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            _renderer.Render(_objects.TransparentObjects, camera);
        }

        internal void AddObject(LevelObject obj)
        {
            obj.LoadContent(GameInstance.GraphicsDevice);
            _objects.Add(obj);
        }

        internal void RemoveObject(LevelObject obj)
        {
            obj.ToBeRemoved = true;
        }

        internal float GetCameraDistance(Vector3 position)
            => Vector3.Distance(position, _screen.Camera.Position);
    }
}
