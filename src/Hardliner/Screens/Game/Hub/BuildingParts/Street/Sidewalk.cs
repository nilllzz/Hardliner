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

namespace Hardliner.Screens.Game.Hub.BuildingParts.Street
{
    internal class Sidewalk : LevelObject
    {
        private Texture2D[] _textures = new Texture2D[2];
        private Vector3 _position;
        private Vector2 _size;

        public override Texture2D Texture0 => _textures[0];
        public override Texture2D Texture1 => _textures[1];

        public Sidewalk(Level level, ContentManager content, Vector3 position, Vector2 size)
            : base(level)
        {
            _textures[0] = content.Load<Texture2D>(Resources.Textures.Hub.Street.SidewalkTop);
            _textures[1] = content.Load<Texture2D>(Resources.Textures.Hub.Street.SidewalkSide);
            _position = position;
            _size = size;

            Collider = new BoxCollider(new Vector3(0, 0.1f, 0) + _position, new Vector3(size.X, 0.2f, size.Y));
        }

        protected override void CreateWorld()
        {
            World = Matrix.CreateTranslation(_position + new Vector3(0, 0.1f, 0));
        }

        protected override void CreateGeometry()
        {
            var sideTexture = new GeometryTextureIndex(1);
            var topTexture = new GeometryTextureMultiplier(_size);

            var cuboidTexture = new GeometryTextureCuboidWrapper();
            cuboidTexture.AddSide(new[] { CuboidSide.Back, CuboidSide.Front, CuboidSide.Left, CuboidSide.Right }, sideTexture);
            cuboidTexture.AddSide(new[] { CuboidSide.Top, CuboidSide.Bottom }, topTexture);

            Geometry.AddVertices(CuboidComposer.Create(_size.X, 0.2f, _size.Y, cuboidTexture));
        }
    }
}
