using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardliner.Content;
using Hardliner.Engine.Collision;
using Hardliner.Engine.Rendering.Geometry.Composers;
using Hardliner.Engine.Rendering.Geometry.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Screens.Game.Hub.BuildingParts.BuildingSide
{
    internal class Sides : LevelObject
    {
        private static Texture2D[] _textures;

        private readonly int _height, _width, _depth;
        private readonly Vector3 _position;

        public override Texture2D Texture0 => _textures[0];
        public override Texture2D Texture1 => _textures[1];
        public override Texture2D Texture2 => _textures[2];

        public Sides(Level level, ContentManager content, int width, int height, int depth, Vector3 position)
            : base(level)
        {
            _height = height;
            _width = width;
            _depth = depth;
            _position = position;

            Collider = new BoxCollider(_position + new Vector3(0, _height / 2f, 0), new Vector3(_width, _height, _depth));

            LoadTextures(content);
        }

        private static void LoadTextures(ContentManager content)
        {
            if (_textures == null)
            {
                _textures = new[]
                {
                    content.Load<Texture2D>(Resources.Textures.Hub.Wall1),
                    content.Load<Texture2D>(Resources.Textures.Hub.Window1),
                    content.Load<Texture2D>(Resources.Textures.Hub.Window2),
                };
            }
        }

        protected override void CreateWorld()
        {
            World = Matrix.CreateTranslation(_position + new Vector3(1f));
        }

        protected override void CreateGeometry()
        {
            var random = new Random();
            var halfWidth = _width / 2f;
            var halfDepth = _depth / 2f;

            for (var y = 0; y < _height; y+=2)
            {
                for (var z = 0; z < _depth; z+=2)
                {
                    var textureIndex = 0;
                    if (z % 3 != 0)
                        textureIndex = random.Next(1, 3);

                    Geometry.AddVertices(RectangleComposer.Create(new[]
                    {
                        new Vector3(-halfWidth - 1f, y + 1f , z - 1f - halfDepth),
                        new Vector3(-halfWidth - 1f, y + 1f , z + 1f - halfDepth),
                        new Vector3(-halfWidth - 1f, y - 1f , z - 1f - halfDepth),
                        new Vector3(-halfWidth - 1f, y - 1f , z + 1f - halfDepth),
                    }, new GeometryTextureIndex(textureIndex)));

                    Geometry.AddVertices(RectangleComposer.Create(new[]
                    {
                        new Vector3(halfWidth - 1f, y + 1f , z - 1f - halfDepth),
                        new Vector3(halfWidth - 1f, y + 1f , z + 1f - halfDepth),
                        new Vector3(halfWidth - 1f, y - 1f , z - 1f - halfDepth),
                        new Vector3(halfWidth - 1f, y - 1f , z + 1f - halfDepth),
                    }, new GeometryTextureIndex(textureIndex)));
                }
                for (var x = 0; x < _width; x += 2)
                {
                    var textureIndex = 0;
                    if (x % 3 != 0)
                        textureIndex = random.Next(1, 3);

                    Geometry.AddVertices(RectangleComposer.Create(new[]
                    {
                        new Vector3(x - 1f - halfWidth, y + 1f, -halfDepth - 1f),
                        new Vector3(x + 1f - halfWidth, y + 1f, -halfDepth - 1f),
                        new Vector3(x - 1f - halfWidth, y - 1f, -halfDepth - 1f),
                        new Vector3(x + 1f - halfWidth, y - 1f, -halfDepth - 1f),
                    }, new GeometryTextureIndex(textureIndex)));

                    Geometry.AddVertices(RectangleComposer.Create(new[]
                    {
                        new Vector3(x - 1f - halfWidth, y + 1f, halfDepth - 1f),
                        new Vector3(x + 1f - halfWidth, y + 1f, halfDepth - 1f),
                        new Vector3(x - 1f - halfWidth, y - 1f, halfDepth - 1f),
                        new Vector3(x + 1f - halfWidth, y - 1f, halfDepth - 1f),
                    }, new GeometryTextureIndex(textureIndex)));
                }
            }
        }
    }
}
