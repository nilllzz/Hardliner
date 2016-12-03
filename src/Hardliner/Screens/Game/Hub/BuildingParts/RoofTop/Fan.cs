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
    internal class Fan : LevelObject
    {
        private Texture2D[] _textures = new Texture2D[2];
        private Vector3 _position;

        public override Texture2D Texture0 => _textures[0];
        public override Texture2D Texture1 => _textures[1];

        public Fan(Level level, ContentManager content, Vector3 position)
            : base(level)
        {
            _textures[0] = content.Load<Texture2D>(Resources.Textures.Hub.FanFront);
            _textures[1] = content.Load<Texture2D>(Resources.Textures.Hub.FanSide);
            _position = position;

            Collider = new BoxCollider(_position + new Vector3(0, 0.5f, 0), new Vector3(1f, 1f, 0.5f));
        }

        protected override void CreateWorld()
        {
            World = Matrix.CreateTranslation(_position + new Vector3(0, 0.5f, 0));
        }

        protected override void CreateGeometry()
        {
            var cuboidTexture = new GeometryTextureCuboidWrapper();
            cuboidTexture.AddSide(
                new[] { CuboidSide.Left, CuboidSide.Right, CuboidSide.Top, CuboidSide.Bottom }, new GeometryTextureIndex(1));

            Geometry.AddVertices(CuboidComposer.Create(1f, 1f, 0.5f, cuboidTexture));
        }
    }
}
