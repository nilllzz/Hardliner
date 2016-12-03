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
        private Texture2D _texture;

        public TestFloor(Level level, ContentManager content)
            : base(level)
        {
            _content = content;
            _texture = content.Load<Texture2D>(Resources.Textures.Leaves);
            Collider = new BoxCollider(new Vector3(0, -0.1f, 0), new Vector3(1000, 0.1f, 1000));
        }

        public override Texture2D Texture0 => _texture;

        protected override void CreateGeometry()
        {
            var vertices = RectangleComposer.Create(1000f, 1000f, new GeometryTextureMultiplier(1000f));
            
            Geometry.AddVertices(vertices);
        }
    }
}
