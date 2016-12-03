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
        Texture2D Texture0 { get; }
        Texture2D Texture1 { get; }
        Texture2D Texture2 { get; }
        bool IsVisible { get; set; }
        BlendState BlendState { get; }

        void Update();
    }
}
