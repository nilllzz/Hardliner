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
    internal class Street : LevelObject
    {
        private Texture2D _texture;
        private Vector3 _position;
        private float _size;
        private bool _turn;

        public override Texture2D Texture0 => _texture;

        public Street(Level level, ContentManager content, Vector3 position, float size, bool turn = false)
            : base(level)
        {
            _texture = content.Load<Texture2D>(Resources.Textures.Hub.Street.StreetNormal);
            _position = position;
            _size = size;
            _turn = turn;

            if (!_turn)
                Collider = new BoxCollider(_position, new Vector3(8f, 0.001f, _size));
            else
                Collider = new BoxCollider(_position, new Vector3(_size, 0.001f, 8f));
        }

        protected override void CreateWorld()
        {
            if (_turn)
                World = Matrix.CreateRotationY(MathHelper.PiOver2);
            World *= Matrix.CreateTranslation(_position);
        }

        protected override void CreateGeometry()
        {
            Geometry.AddVertices(CuboidComposer.Create(8f, 0.001f, _size, 
                new GeometryTextureMultiplier(new Vector2(1f, _size / 2f))));
        }
    }
}
