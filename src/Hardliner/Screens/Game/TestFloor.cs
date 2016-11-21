using Hardliner.Content;
using Hardliner.Engine;
using Hardliner.Engine.Collision;
using Hardliner.Engine.Rendering;
using Hardliner.Engine.Rendering.Geometry.Composers;
using Hardliner.Engine.Rendering.Geometry.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Screens.Game
{
    internal class TestFloor : LevelObject
    {
        private ContentManager _content;
        private IdentifiedTexture _texture;

        public TestFloor(Level level,ContentManager content)
            : base(level)
        {
            _content = content;
            _texture = new IdentifiedTexture(content.Load<Texture2D>(Resources.Textures.Leaves));
            Collider = new BoxCollider(new Vector3(0, -0.1f, 0), new Vector3(100, 0.1f, 100));
        }

        public override IdentifiedTexture Texture => _texture;

        protected override void CreateGeometry()
        {
            var vertices = RectangleComposer.Create(100f, 100f, new GeometryTextureMultiplier(100f));
            
            Geometry.AddVertices(vertices);
        }
    }
}
