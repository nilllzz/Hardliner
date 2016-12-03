using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Engine.Rendering.Geometry
{
    public static class VertexTransformer
    {
        public static void Offset(VertexInput[] vertices, Vector3 offset)
        {
            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Position += offset;
        }

        public static void Rotate(VertexInput[] vertices, Vector3 rotation)
        {
            var transformation = Matrix.CreateRotationX(rotation.X) * 
                Matrix.CreateRotationY(rotation.Y) * 
                Matrix.CreateRotationZ(rotation.Z);
            
            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Position += Vector3.Transform(vertices[i].Position, transformation) - vertices[i].Position;
        }
    }
}
