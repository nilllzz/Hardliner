using Microsoft.Xna.Framework;

namespace Hardliner.Engine.Rendering.Geometry.Texture
{
    public class GeometryTextureMultiplier : IGeometryTextureDefintion
    {
        private readonly float _multiplier;

        public GeometryTextureMultiplier(float repeatTexture = 1f)
        {
            _multiplier = repeatTexture;
        }

        public Vector2 Transform(Vector2 normalVector)
            => normalVector * _multiplier;

        public void NextElement() { }
    }
}
