using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardliner.Content;
using Hardliner.Engine.Rendering.Geometry.Composers;
using Hardliner.Engine.Rendering.Geometry.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Screens.Game.Hub.IntroMachine
{
    internal class UpperPart : LevelObject
    {
        private Texture2D _texture;

        public override Texture2D Texture0 => _texture;
        public override Texture2D Texture1 => _texture;

        public UpperPart(Level level, ContentManager content)
            : base(level)
        {
            _texture = content.Load<Texture2D>(Resources.Textures.Hub.Intro.TitleBackground);
        }

        protected override void CreateWorld()
        {
            World = Matrix.CreateTranslation(0, 101.9f, -0.1f);
        }

        protected override void CreateGeometry()
        {
            Geometry.AddVertices(RectangleComposer.Create(new[]
            {
                new Vector3(-0.5f, 0.25f, -0.25f),
                new Vector3(0.5f, 0.25f, -0.25f),
                new Vector3(-0.5f, -0.25f, 0),
                new Vector3(0.5f, -0.25f, 0),
            }));

            var textureRectangle = new GeometryTextureRectangle(new Rectangle(1, 1, 1, 1), _texture, 1);

            Geometry.AddVertices(RectangleComposer.Create(new[]
            {
                new Vector3(-0.5f, 0.25f, -0.25f),
                new Vector3(-0.5f, -0.25f, 0),
                new Vector3(-0.5f, 0f, -0.5f),
                new Vector3(-0.5f, -0.25f, -0.4f),
            }, textureRectangle));

            Geometry.AddVertices(RectangleComposer.Create(new[]
            {
                new Vector3(0.5f, 0.25f, -0.25f),
                new Vector3(0.5f, -0.25f, 0),
                new Vector3(0.5f, 0f, -0.5f),
                new Vector3(0.5f, -0.25f, -0.4f),
            }, textureRectangle));

            Geometry.AddVertices(RectangleComposer.Create(new[]
            {
                new Vector3(0.5f, 0.25f, -0.25f),
                new Vector3(0.5f, 0f, -0.5f),
                new Vector3(-0.5f, 0.25f, -0.25f),
                new Vector3(-0.5f, 0f, -0.5f),
            }, textureRectangle));

            Geometry.AddVertices(RectangleComposer.Create(new[]
            {
                new Vector3(0.5f, 0f, -0.5f),
                new Vector3(0.5f, -0.25f, -0.4f),
                new Vector3(-0.5f, 0f, -0.5f),
                new Vector3(-0.5f, -0.25f, -0.4f),
            }, textureRectangle));

            var test = Geometry.Vertices.Select(v => v.TextureIndex).ToArray();
        }
    }
}
