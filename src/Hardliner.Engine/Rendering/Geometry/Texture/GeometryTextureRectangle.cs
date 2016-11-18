using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Engine.Rendering.Geometry.Texture
{
    public class GeometryTextureRectangle : IGeometryTextureDefintion
    {
        private readonly Vector2 _textureStart, _textureEnd;

        public GeometryTextureRectangle(Rectangle textureRectangle, Rectangle textureBounds)
        {
            _textureStart = new Vector2((float)textureRectangle.Left / textureBounds.Width,
                (float)textureRectangle.Top / textureBounds.Height);
            _textureEnd = new Vector2((float)textureRectangle.Width / textureBounds.Width,
                (float)textureRectangle.Height / textureBounds.Height);
        }

        public GeometryTextureRectangle(Rectangle textureRectangle, Texture2D texture)
             : this(textureRectangle, texture.Bounds)
        { }

        public Vector2 Transform(Vector2 normalVector)
            => _textureStart + normalVector * _textureEnd;

        public void NextElement() { }
    }
}
