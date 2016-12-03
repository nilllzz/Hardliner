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

namespace Hardliner.Screens.Game.Hub.BuildingParts.RoofTop
{
    internal class Wall : LevelObject
    {
        private Texture2D[] _textures = new Texture2D[2];
        private Vector3 _position;
        private Vector2 _size;
        private bool _turn;

        public override Texture2D Texture0 => _textures[0];
        public override Texture2D Texture1 => _textures[1];

        public Wall(Level level, ContentManager content, Vector3 position, Vector2 size, bool turn = false)
            : base(level)
        {
            _textures[0] = content.Load<Texture2D>(Resources.Textures.Hub.Wall2);
            _textures[1] = content.Load<Texture2D>(Resources.Textures.Hub.Wall3);
            _position = position;
            _size = size;
            _turn = turn;

            if (turn)
                Collider = new BoxCollider(new Vector3(0, _size.Y / 2f, 0) + _position, new Vector3(1f, size.Y, size.X));
            else
                Collider = new BoxCollider(new Vector3(0, _size.Y / 2f, 0) + _position, new Vector3(size.X, size.Y, 1f));
        }

        protected override void CreateWorld()
        {
            if (_turn)
                World = Matrix.CreateRotationY(MathHelper.PiOver2);
            World *= Matrix.CreateTranslation(_position + new Vector3(0, _size.Y / 2f, 0));
        }

        protected override void CreateGeometry()
        {
            var side1Texture = new GeometryTextureMultiplier(new Vector2(_size.X, _size.Y));
            var side2Texture = new GeometryTextureMultiplier(new Vector2(1, _size.Y));
            var topTexture = new GeometryTextureMultiplier(new Vector2(_size.X, 1), 1);

            var cuboidTexture = new GeometryTextureCuboidWrapper();
            cuboidTexture.AddSide(new[] { CuboidSide.Back, CuboidSide.Front }, side1Texture);
            cuboidTexture.AddSide(new[] { CuboidSide.Left, CuboidSide.Right }, side2Texture);
            cuboidTexture.AddSide(new[] { CuboidSide.Top, CuboidSide.Bottom }, topTexture);

            Geometry.AddVertices(CuboidComposer.Create(_size.X, _size.Y, 1f, cuboidTexture));
        }
    }
}
