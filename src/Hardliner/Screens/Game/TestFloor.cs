using Hardliner.Content;
using Hardliner.Engine;
using Hardliner.Engine.Rendering;
using Hardliner.Engine.Rendering.Geometry.Composers;
using Hardliner.Engine.Rendering.Geometry.Texture;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Screens.Game
{
    internal class TestFloor : Base3DObject<VertexPositionNormalTexture>
    {
        private ContentManager _content;
        private IdentifiedTexture _texture;

        public TestFloor(ContentManager content)
        {
            _content = content;
            _texture = new IdentifiedTexture(content.Load<Texture2D>(Resources.Textures.Leaves));
        }

        public override IdentifiedTexture Texture => _texture;

        protected override void CreateGeometry()
        {
            var vertices = RectangleComposer.Create(100f, 100f, new GeometryTextureMultiplier(100f));
            
            Geometry.AddVertices(vertices);
        }
    }
}
