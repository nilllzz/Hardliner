using Hardliner.Engine.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Engine.Rendering
{
    public interface I3DObject
    {
        VertexBuffer VertexBuffer { get; set; }
        IndexBuffer IndexBuffer { get; set; }
        Matrix World { get; set; }
        IdentifiedTexture Texture { get; }
        bool IsVisible { get; set; }
        ICollider Collider { get; set; }

        void Update();
        void LoadContent(GraphicsDevice device);
    }
}
