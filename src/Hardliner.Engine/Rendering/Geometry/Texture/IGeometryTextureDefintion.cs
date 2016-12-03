using Microsoft.Xna.Framework;

namespace Hardliner.Engine.Rendering.Geometry.Texture
{
    public interface IGeometryTextureDefintion
    {
        void NextElement();
        Vector2 Transform(Vector2 normalVector);
        int GetTextureIndex();
    }
}
