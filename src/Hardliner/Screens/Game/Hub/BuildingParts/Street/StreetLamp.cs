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

namespace Hardliner.Screens.Game.Hub.BuildingParts.Street
{
    internal class StreetLamp : LevelObject
    {
        internal static LevelObject[] Factory(Level level, ContentManager content, Vector3 position, float rotation)
        {
            var lightPosition = new Vector3(0.35f, 2f, 0f);
            var rotationMatrix = Matrix.CreateRotationY(rotation);
            lightPosition = Vector3.Transform(lightPosition, rotationMatrix);

            return new LevelObject[]
            {
                new StreetLamp(level, content, position, rotation),
                new StreetLampLight(level, content, lightPosition + position)
            };
        }

        private Texture2D _texture;
        private Vector3 _position;
        private float _rotation;

        public override Texture2D Texture0 => _texture;

        private StreetLamp(Level level, ContentManager content, Vector3 position, float rotation)
            : base(level)
        {
            _texture = content.Load<Texture2D>(Resources.Textures.Hub.Street.StreetLamp);
            _position = position;
            _rotation = rotation;

            Collider =
                new BoxCollider(new Vector3(0, 5f, 0) + _position, new Vector3(0.2f, 10f, 0.2f));
        }

        protected override void CreateWorld()
        {
            World = Matrix.CreateRotationY(_rotation) *
                Matrix.CreateTranslation(_position + new Vector3(0, 5f, 0));
        }

        protected override void CreateGeometry()
        {
            Geometry.AddVertices(CuboidComposer.Create(0.2f, 10f, 0.2f,
                new GeometryTextureRectangle(new Rectangle(0, 4, 4, 4), _texture)));

            var sideTexture1 = new GeometryTextureRectangle(new Rectangle(0, 0, 8, 4), _texture);
            var sideTexture2 = new GeometryTextureRectangle(new Rectangle(12, 4, 4, 4), _texture);
            var bottomTexture = new GeometryTextureRectangle(new Rectangle(8, 0, 8, 4), _texture);
            var cuboidTexture = new GeometryTextureCuboidWrapper();
            cuboidTexture.AddSide(new[] { CuboidSide.Left, CuboidSide.Right }, sideTexture2);
            cuboidTexture.AddSide(new[] { CuboidSide.Front, CuboidSide.Back, CuboidSide.Top }, sideTexture1);
            cuboidTexture.AddSide(new[] { CuboidSide.Bottom }, bottomTexture);

            var topPart = CuboidComposer.Create(0.8f, 0.4f, 0.4f, cuboidTexture);
            VertexTransformer.Offset(topPart, new Vector3(0.25f, 5f, 0f));
            Geometry.AddVertices(topPart);
        }

        private class StreetLampLight : LevelObject
        {
            private Texture2D _texture;
            private Vector3 _position;

            public override Texture2D Texture0 => _texture;
            public override BlendState BlendState => BlendState.Additive;
            internal override bool IsOpaque => false;
            protected override float CameraDistance => _level.GetCameraDistance(_position);

            public StreetLampLight(Level level, ContentManager content, Vector3 position)
                : base(level)
            {
                _texture = content.Load<Texture2D>(Resources.Textures.Hub.Street.LampShade);
                _position = position;
            }

            protected override void CreateWorld()
            {
                World = Matrix.CreateRotationY(MathHelper.PiOver4) *
                    Matrix.CreateTranslation(_position + new Vector3(0, 4f, 0));
            }

            protected override void CreateGeometry()
            {
                Geometry.AddVertices(RectangleComposer.Create(new[]
                {
                    new Vector3(-4f, 4f, 0f),
                    new Vector3(4f, 4f, 0f),
                    new Vector3(-4f, -4f, 0f),
                    new Vector3(4f, -4f, 0f),
                }));
                Geometry.AddVertices(RectangleComposer.Create(new[]
                {
                    new Vector3(0f, 4f, -4f),
                    new Vector3(0f, 4f, 4f),
                    new Vector3(0f, -4f, -4f),
                    new Vector3(0f, -4f, 4f),
                }));
            }
        }
    }
}
