using Microsoft.Xna.Framework;

namespace Hardliner.Engine.Rendering.Geometry.Texture
{
    public class GeometryTexturePoleWrapper : IGeometryTextureDefintion
    {
        private int _element;
        private readonly int _totalElements;
        private readonly Vector2 _textureStart, _textureEnd;
        private readonly int _textureindex;

        public GeometryTexturePoleWrapper(Rectangle textureRectangle, Rectangle textureBounds, int elements, int textureIndex = 0)
        {
            _totalElements = elements;

            _textureStart = new Vector2((float)textureRectangle.Left / textureBounds.Width,
                (float)textureRectangle.Top / textureBounds.Height);
            _textureEnd = new Vector2((float)textureRectangle.Width / textureBounds.Width,
                (float)textureRectangle.Height / textureBounds.Height / _totalElements);

            _textureindex = textureIndex;
        }

        public void NextElement()
        {
            _element++;
        }

        public Vector2 Transform(Vector2 normalVector)
        {
            return (_textureStart + (normalVector * _textureEnd) + new Vector2(0f, _textureEnd.Y * _element));
        }

        public int GetTextureIndex() => _textureindex;
    }
}
