﻿using Hardliner.Content;
using Hardliner.Engine;
using Hardliner.Engine.Collision;
using Hardliner.Engine.Rendering;
using Hardliner.Engine.Rendering.Geometry.Composers;
using Hardliner.Engine.Rendering.Geometry.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Screens.Game
{
    internal class TestTreeLog : LevelObject
    {
        private ContentManager _content;
        private IdentifiedTexture _texture;
        private Vector3 _position;

        public TestTreeLog(Level level, ContentManager content, Vector3 position)
            : base(level)
        {
            _content = content;
            _position = position;
            _texture = new IdentifiedTexture(content.Load<Texture2D>(Resources.Textures.TreeLog));

            Collider = new BoxCollider(position, new Vector3(1, 2, 1));
        }

        public override IdentifiedTexture Texture => _texture;

        protected override void CreateWorld()
        {
            World = Matrix.CreateRotationZ(MathHelper.PiOver2) * Matrix.CreateTranslation(_position);
        }

        protected override void CreateGeometry()
        {
            int height = 2;
            int edges = 12;
            float radius = 0.5f;

            Geometry.AddVertices(CylinderComposer.Create(radius, height, edges,
                new GeometryTextureRectangle(new Rectangle(0, 0, 96, 10), _texture.Resource),
                new GeometryTextureRectangle(new Rectangle(0, 20, 40, 40), _texture.Resource)));
        }
    }
}
