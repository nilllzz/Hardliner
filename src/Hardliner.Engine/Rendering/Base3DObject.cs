using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Engine.Rendering
{
    public abstract class Base3DObject<VertexType> : I3DObject, IDisposable where VertexType : struct
    {
        private const string FIELD_NAME_VERTEXDECLARATION = "VertexDeclaration";

        public Geometry<VertexType> Geometry { get; protected set; } = new Geometry<VertexType>();
        public VertexBuffer VertexBuffer { get; set; }
        public IndexBuffer IndexBuffer { get; set; }
        public Matrix World { get; set; } = Matrix.Identity;
        public bool IsDisposed { get; private set; }
        public virtual IdentifiedTexture Texture => null;
        protected GraphicsDevice GraphicsDevice { get; private set; }

        private static VertexDeclaration GetVertexDeclaration()
            => (VertexDeclaration)typeof(VertexType).GetField(FIELD_NAME_VERTEXDECLARATION).GetValue(null);
        
        public virtual void LoadContent(GraphicsDevice device)
        {
            GraphicsDevice = device;

            CreateWorld();
            CreateGeometry();
            CreateBuffers();
        }

        protected abstract void CreateGeometry();

        protected void CreateBuffers()
        {
            var vertices = Geometry.Vertices;
            var indices = Geometry.Indices;
            
            VertexBuffer = new VertexBuffer(GraphicsDevice, GetVertexDeclaration(), vertices.Length, BufferUsage.WriteOnly);
            VertexBuffer.SetData(vertices);

            IndexBuffer = new IndexBuffer(GraphicsDevice, typeof(int), indices.Length, BufferUsage.WriteOnly);
            IndexBuffer.SetData(indices);
        }

        protected virtual void CreateWorld() { }

        public virtual void Update() { }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Base3DObject()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (!VertexBuffer.IsDisposed) VertexBuffer.Dispose();
                    if (!IndexBuffer.IsDisposed) IndexBuffer.Dispose();
                    if (!Geometry.IsDisposed) Geometry.Dispose();
                }

                VertexBuffer = null;
                IndexBuffer = null;
                Geometry = null;

                IsDisposed = true;
            }
        }
    }
}
