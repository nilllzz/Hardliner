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
    internal class RedWarningLight : LevelObject
    {
        private Texture2D _texture;
        private Vector3 _position;

        public override Texture2D Texture0 => _texture;
        public override BlendState BlendState => BlendState.Additive;
        internal override bool IsOpaque => false;
        protected override float CameraDistance => _level.GetCameraDistance(_position);

        public RedWarningLight(Level level, Vector3 position)
            : base(level)
        {
            _texture = TextureFactory.FromColor(Color.Red);
            _position = position;

            Collider = new BoxCollider(_position, new Vector3(0.75f));
        }

        protected override void CreateWorld()
        {
            World = Matrix.CreateTranslation(_position);
        }

        protected override void CreateGeometry()
        {
            Geometry.AddVertices(CuboidComposer.Create(0.75f));
        }
    }
}
