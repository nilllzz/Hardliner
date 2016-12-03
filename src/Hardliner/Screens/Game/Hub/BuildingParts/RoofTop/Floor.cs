using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardliner.Content;
using Hardliner.Engine.Collision;
using Hardliner.Engine.Rendering.Geometry;
using Hardliner.Engine.Rendering.Geometry.Composers;
using Hardliner.Engine.Rendering.Geometry.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Screens.Game.Hub.BuildingParts.RoofTop
{
    internal class Floor : LevelObject
    {
        private static VertexInput[] VertexCache;

        private ContentManager _content;
        private Texture2D _texture;
        private Vector3 _position;
        private Vector2 _size;

        public override Texture2D Texture0 => _texture;

        public Floor(Level level, ContentManager content, Vector3 position, Vector2 size)
            : base(level)
        {
            _content = content;
            _texture = content.Load<Texture2D>(Resources.Textures.Hub.Floor1);
            _position = position;
            _size = size;

            Collider = new BoxCollider(position, new Vector3(size.X, 0.1f, size.Y));
        }

        protected override void CreateWorld()
        {
            World = Matrix.CreateTranslation(_position);
        }

        protected override void CreateGeometry()
        {
            if (VertexCache == null)
                VertexCache = RectangleComposer.Create(_size.X, _size.Y, new GeometryTextureMultiplier(_size));

            Geometry.AddVertices(VertexCache);
        }
    }
}
