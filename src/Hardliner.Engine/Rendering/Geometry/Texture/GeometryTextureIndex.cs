using Microsoft.Xna.Framework;

namespace Hardliner.Engine.Rendering.Geometry.Texture
{
    public class GeometryTextureIndex : IGeometryTextureDefintion
    {
        private readonly int _textureIndex;

        public GeometryTextureIndex(int textureIndex)
        {
            _textureIndex = textureIndex;
        }

        public int GetTextureIndex() => _textureIndex;

        public void NextElement() { }

        public Vector2 Transform(Vector2 normalVector) => normalVector;
    }
}
