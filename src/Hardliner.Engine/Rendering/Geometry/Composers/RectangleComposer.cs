using Hardliner.Engine.Rendering.Geometry.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Engine.Rendering.Geometry.Composers
{
    public static class RectangleComposer
    {
        public static VertexPositionNormalTexture[] Create(float width, float height)
            => Create(width, height, DefaultGeometryTextureDefinition.Instance);

        public static VertexPositionNormalTexture[] Create(float width, float height, IGeometryTextureDefintion textureDefinition)
        {
            var halfWidth = width / 2f;
            var halfHeight = height / 2f;

            return Create(new[]
            {
                new Vector3(-halfWidth, 0, -halfHeight),
                new Vector3(halfWidth, 0, -halfHeight),
                new Vector3(-halfWidth, 0, halfHeight),
                new Vector3(halfWidth, 0, halfHeight),

            }, textureDefinition);
        }

        public static VertexPositionNormalTexture[] Create(Vector3[] positions)
            => Create(positions, DefaultGeometryTextureDefinition.Instance);

        public static VertexPositionNormalTexture[] Create(Vector3[] positions, IGeometryTextureDefintion textureDefinition)
        {
            return new VertexPositionNormalTexture[]
            {
                new VertexPositionNormalTexture
                {
                    Position = positions[0],
                    Normal = new Vector3(0, 1, 0),
                    TextureCoordinate = textureDefinition.Transform(new Vector2(0, 0))
                },
                new VertexPositionNormalTexture
                {
                    Position = positions[1],
                    Normal = new Vector3(0, 1, 0),
                    TextureCoordinate = textureDefinition.Transform(new Vector2(1, 0))
                },
                new VertexPositionNormalTexture
                {
                    Position = positions[2],
                    Normal = new Vector3(0, 1, 0),
                    TextureCoordinate = textureDefinition.Transform(new Vector2(0, 1))
                },

                new VertexPositionNormalTexture
                {
                    Position = positions[1],
                    Normal = new Vector3(0, 1, 0),
                    TextureCoordinate = textureDefinition.Transform(new Vector2(1, 0))
                },
                new VertexPositionNormalTexture
                {
                    Position = positions[2],
                    Normal = new Vector3(0, 1, 0),
                    TextureCoordinate = textureDefinition.Transform(new Vector2(0, 1))
                },
                new VertexPositionNormalTexture
                {
                    Position = positions[3],
                    Normal = new Vector3(0, 1, 0),
                    TextureCoordinate = textureDefinition.Transform(new Vector2(1, 1))
                },
            };
        }
        
    }
}
