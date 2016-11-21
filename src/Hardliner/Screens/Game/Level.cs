using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardliner.Engine;
using Hardliner.Engine.Rendering;
using Microsoft.Xna.Framework.Graphics;
using static Core;

namespace Hardliner.Screens.Game
{
    internal class Level
    {
        private List<LevelObject> _objects = new List<LevelObject>();
        private ObjectRenderer _renderer = new ObjectRenderer();

        internal bool HasRope => _objects.Any(o => o is HookShotRope);

        internal IEnumerable<LevelObject> Objects => _objects;
        
        internal void LoadContent(IEnumerable<LevelObject> initialObjects)
        {
            _objects = initialObjects.ToList();

            _renderer.LoadContent(GameInstance.GraphicsDevice);
            _objects.ForEach(o => o.LoadContent(GameInstance.GraphicsDevice));
        }

        internal void Update()
        {
            lock (_objects)
            {
                for (int i = 0; i < _objects.Count; i++)
                    _objects[i].Update();
            }
        }

        internal void Render(Camera camera)
        {
            _renderer.Render(_objects, camera);
        }

        internal void AddObject(LevelObject obj)
        {
            obj.LoadContent(GameInstance.GraphicsDevice);
            _objects.Insert(0, obj);
        }

        internal void RemoveObject(LevelObject obj)
        {
            obj.Dispose();
            _objects.Remove(obj);
        }
    }
}
