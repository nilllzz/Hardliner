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
    internal class Trashcan : LevelObject
    {
        private Texture2D _texture;
        private Vector3 _position;

        public override Texture2D Texture0 => _texture;

        public Trashcan(Level level, ContentManager content, Vector3 position)
            : base(level)
        {
            _texture = content.Load<Texture2D>(Resources.Textures.Hub.General.Trashcan);
            _position = position;
            
            Collider = new BoxCollider(_position + new Vector3(0, 0.5f, 0f), new Vector3(0.75f, 1f, 0.75f));
        }

        protected override void CreateWorld()
        {
            World = Matrix.CreateRotationZ(-MathHelper.PiOver2) * 
                Matrix.CreateTranslation(_position + new Vector3(0, 0.5f, 0f));
        }

        protected override void CreateGeometry()
        {
            var sideTexture = new GeometryTexturePoleWrapper(new Rectangle(0, 0, 16, 64), _texture.Bounds, 20);
            var topTexure = new GeometryTextureRectangle(new Rectangle(0, 64, 16, 16), _texture);

            Geometry.AddVertices(CylinderComposer.Create(0.375f, 1f, 20, sideTexture, topTexure));
        }
    }
}
