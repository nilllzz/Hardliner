using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Engine.Rendering.Geometry.Texture
{
    public class GeometryTextureRectangle : IGeometryTextureDefintion
    {
        private readonly Vector2 _textureStart, _textureEnd;
        private readonly int _textureIndex;

        public GeometryTextureRectangle(Rectangle textureRectangle, Rectangle textureBounds, int textureIndex = 0)
        {
            _textureStart = new Vector2((float)textureRectangle.Left / textureBounds.Width,
                (float)textureRectangle.Top / textureBounds.Height);
            _textureEnd = new Vector2((float)textureRectangle.Width / textureBounds.Width,
                (float)textureRectangle.Height / textureBounds.Height);

            _textureIndex = textureIndex;
        }

        public GeometryTextureRectangle(Rectangle textureRectangle, Texture2D texture, int textureIndex = 0)
             : this(textureRectangle, texture.Bounds, textureIndex)
        { }

        public GeometryTextureRectangle(float x, float y, float width, float height, int textureIndex = 0)
        {
            _textureStart = new Vector2(x, y);
            _textureEnd = new Vector2(width, height);
            _textureIndex = textureIndex;
        }

        public Vector2 Transform(Vector2 normalVector)
            => _textureStart + normalVector * _textureEnd;

        public void NextElement() { }

        public int GetTextureIndex() => _textureIndex;
    }
}
