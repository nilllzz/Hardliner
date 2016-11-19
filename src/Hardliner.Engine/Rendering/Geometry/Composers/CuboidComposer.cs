using System.Collections.Generic;
using Hardliner.Engine.Rendering.Geometry.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Engine.Rendering.Geometry.Composers
{
    public static class CuboidComposer
    {
        public static VertexPositionNormalTexture[] Create(float length)
            => Create(length, length, length, DefaultGeometryTextureDefinition.Instance);

        public static VertexPositionNormalTexture[] Create(float length, IGeometryTextureDefintion textureDefinition)
            => Create(length, length, length, textureDefinition);

        public static VertexPositionNormalTexture[] Create(float width, float height, float depth)
            => Create(width, height, depth, DefaultGeometryTextureDefinition.Instance);

        public static VertexPositionNormalTexture[] Create(float width, float height, float depth,
            IGeometryTextureDefintion textureDefinition)
        {
            var vertices = new List<VertexPositionNormalTexture>();

            var halfWidth = width / 2f;
            var halfHeight = height / 2f;
            var halfDepth = depth / 2f;
            var front = RectangleComposer.Create(new[]
            {
                new Vector3(-halfWidth, -halfHeight, halfDepth),
                new Vector3(-halfWidth, halfHeight, halfDepth),
                new Vector3(halfWidth, -halfHeight, halfDepth),
                new Vector3(halfWidth, halfHeight, halfDepth),
            }, textureDefinition);
            textureDefinition.NextElement();

            var back = RectangleComposer.Create(new[]
            {
                new Vector3(-halfWidth, -halfHeight, -halfDepth),
                new Vector3(-halfWidth, halfHeight, -halfDepth),
                new Vector3(halfWidth, -halfHeight, -halfDepth),
                new Vector3(halfWidth, halfHeight, -halfDepth),
            }, textureDefinition);
            textureDefinition.NextElement();

            var right = RectangleComposer.Create(new[]
            {
                new Vector3(halfWidth, halfHeight, halfDepth),
                new Vector3(halfWidth, halfHeight, -halfDepth),
                new Vector3(halfWidth, -halfHeight, halfDepth),
                new Vector3(halfWidth, -halfHeight, -halfDepth),
            }, textureDefinition);
            textureDefinition.NextElement();

            var left = RectangleComposer.Create(new[]
            {
                new Vector3(-halfWidth, halfHeight, halfDepth),
                new Vector3(-halfWidth, halfHeight, -halfDepth),
                new Vector3(-halfWidth, -halfHeight, halfDepth),
                new Vector3(-halfWidth, -halfHeight, -halfDepth),
            }, textureDefinition);
            textureDefinition.NextElement();

            var top = RectangleComposer.Create(new[]
            {
                new Vector3(-halfWidth, halfHeight, -halfDepth),
                new Vector3(halfWidth, halfHeight, -halfDepth),
                new Vector3(-halfWidth, halfHeight, halfDepth),
                new Vector3(halfWidth, halfHeight, halfDepth),
            }, textureDefinition);
            textureDefinition.NextElement();

            var bottom = RectangleComposer.Create(new[]
            {
                new Vector3(-halfWidth, -halfHeight, -halfDepth),
                new Vector3(halfWidth, -halfHeight, -halfDepth),
                new Vector3(-halfWidth, -halfHeight, halfDepth),
                new Vector3(halfWidth, -halfHeight, halfDepth),
            }, textureDefinition);

            vertices.AddRange(front);
            vertices.AddRange(back);
            vertices.AddRange(right);
            vertices.AddRange(left);
            vertices.AddRange(top);
            vertices.AddRange(bottom);

            return vertices.ToArray();
        }
    }
}
