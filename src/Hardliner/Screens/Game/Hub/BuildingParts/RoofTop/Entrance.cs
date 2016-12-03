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
    internal class Entrance : LevelObject
    {
        private Texture2D[] _textures = new Texture2D[2];
        private Vector3 _position;

        public override Texture2D Texture0 => _textures[0];
        public override Texture2D Texture1 => _textures[1];

        public Entrance(Level level, ContentManager content, Vector3 position)
            : base(level)
        {
            _textures[0] = content.Load<Texture2D>(Resources.Textures.Hub.Wall1);
            _textures[1] = content.Load<Texture2D>(Resources.Textures.Hub.Door1);
            _position = position;
            
            Collider = new BoxCollider(new Vector3(0, 1.5f, 0) + _position, new Vector3(4f, 3f, 3f));
        }

        protected override void CreateWorld()
        {
            World = Matrix.CreateTranslation(_position + new Vector3(0, 1.5f, 0));
        }

        protected override void CreateGeometry()
        {
            var side1Texture = new GeometryTextureMultiplier(new Vector2(3, 2));
            var side2Texture = new GeometryTextureMultiplier(new Vector2(2, 2));
            var doorTexture = new GeometryTextureIndex(1);
            var topTexture = new GeometryTextureMultiplier(new Vector2(8, 4));

            var cuboidTexture = new GeometryTextureCuboidWrapper();
            cuboidTexture.AddSide(new[] { CuboidSide.Back, CuboidSide.Front }, side1Texture);
            cuboidTexture.AddSide(new[] { CuboidSide.Right }, side2Texture);
            cuboidTexture.AddSide(new[] { CuboidSide.Left }, doorTexture);
            cuboidTexture.AddSide(new[] { CuboidSide.Top, CuboidSide.Bottom }, topTexture);

            Geometry.AddVertices(CuboidComposer.Create(4f, 3f, 3f, cuboidTexture));
        }
    }
}
