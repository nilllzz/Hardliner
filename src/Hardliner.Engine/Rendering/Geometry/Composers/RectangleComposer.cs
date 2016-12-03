using Hardliner.Engine.Rendering.Geometry.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Engine.Rendering.Geometry.Composers
{
    public static class RectangleComposer
    {
        public static VertexInput[] Create(float width, float height)
            => Create(width, height, DefaultGeometryTextureDefinition.Instance);

        public static VertexInput[] Create(float width, float height, IGeometryTextureDefintion textureDefinition)
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

        public static VertexInput[] Create(Vector3[] positions)
            => Create(positions, DefaultGeometryTextureDefinition.Instance);

        public static VertexInput[] Create(Vector3[] positions, IGeometryTextureDefintion textureDefinition)
        {
            var normal1 = GetNormal(positions[0], positions[1], positions[2]);
            var normal2 = -GetNormal(positions[1], positions[2], positions[3]);

            return new VertexInput[]
            {
                new VertexInput
                {
                    Position = positions[0],
                    Normal = normal1,
                    TextureCoordinate = textureDefinition.Transform(new Vector2(0, 0)),
                    TextureIndex = textureDefinition.GetTextureIndex()
                },
                new VertexInput
                {
                    Position = positions[1],
                    Normal = normal1,
                    TextureCoordinate = textureDefinition.Transform(new Vector2(1, 0)),
                    TextureIndex = textureDefinition.GetTextureIndex()
                },
                new VertexInput
                {
                    Position = positions[2],
                    Normal = normal1,
                    TextureCoordinate = textureDefinition.Transform(new Vector2(0, 1)),
                    TextureIndex = textureDefinition.GetTextureIndex()
                },

                new VertexInput
                {
                    Position = positions[1],
                    Normal = normal2,
                    TextureCoordinate = textureDefinition.Transform(new Vector2(1, 0)),
                    TextureIndex = textureDefinition.GetTextureIndex()
                },
                new VertexInput
                {
                    Position = positions[2],
                    Normal = normal2,
                    TextureCoordinate = textureDefinition.Transform(new Vector2(0, 1)),
                    TextureIndex = textureDefinition.GetTextureIndex()
                },
                new VertexInput
                {
                    Position = positions[3],
                    Normal = normal2,
                    TextureCoordinate = textureDefinition.Transform(new Vector2(1, 1)),
                    TextureIndex = textureDefinition.GetTextureIndex()
                },
            };
        }
        
        private static Vector3 GetNormal(Vector3 pos1, Vector3 pos2, Vector3 pos3)
        {
            var side1 = pos2 - pos1;
            var side2 = pos3 - pos1;
            return Vector3.Cross(side1, side2);
        }

    }
}
