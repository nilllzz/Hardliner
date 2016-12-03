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

namespace Hardliner.Screens.Game.Hub.BuildingParts.General
{
    internal class WireFence : LevelObject
    {
        private Texture2D _texture;
        private Vector3 _position;
        private float _size;
        private bool _turn;

        public override Texture2D Texture0 => _texture;
        protected override float CameraDistance => _level.GetCameraDistance(_position);
        internal override bool IsOpaque => false;

        public WireFence(Level level, ContentManager content, Vector3 position, float size, bool turn = false)
            : base(level)
        {
            _texture = content.Load<Texture2D>(Resources.Textures.Hub.General.Fence);
            _position = position;
            _size = size;
            _turn = turn;

            if (!_turn)
                Collider = new BoxCollider(_position + new Vector3(0, 1.5f, 0f), new Vector3(_size, 3f, 0.01f));
            else
                Collider = new BoxCollider(_position + new Vector3(0, 1.5f, 0f), new Vector3(0.01f, 3f, _size));
        }

        protected override void CreateWorld()
        {
            if (_turn)
                World = Matrix.CreateRotationY(MathHelper.PiOver2);
            World *= Matrix.CreateTranslation(_position + new Vector3(0, 1.5f, 0f));
        }

        protected override void CreateGeometry()
        {
            Geometry.AddVertices(CuboidComposer.Create(_size, 3f, 0.01f,
                new GeometryTextureMultiplier(new Vector2(_size * 3f, 1f))));
        }
    }
}
