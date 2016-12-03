using Microsoft.Xna.Framework;

namespace Hardliner.Engine.Rendering.Geometry.Texture
{
    public class GeometryTextureMultiplier : IGeometryTextureDefintion
    {
        private readonly Vector2 _multiplier;
        private readonly int _textureIndex;

        public GeometryTextureMultiplier(float multiplier, int textureIndex = 0)
        {
            _multiplier = new Vector2(multiplier);
            _textureIndex = textureIndex;
        }

        public GeometryTextureMultiplier(Vector2 multiplier, int textureIndex = 0)
        {
            _multiplier = multiplier;
            _textureIndex = textureIndex;
        }

        public Vector2 Transform(Vector2 normalVector)
            => normalVector * _multiplier;

        public void NextElement() { }
        public int GetTextureIndex() => _textureIndex;
    }
}
