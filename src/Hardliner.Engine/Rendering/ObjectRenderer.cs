using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Engine.Rendering
{
    public class ObjectRenderer : IDisposable
    {
        private IdentifiedTexture _lastDrawnTexture = null;
        private BasicEffect _effect;

        protected GraphicsDevice GraphicsDevice { get; private set; }
        public List<I3DObject> Objects { get; private set; } = new List<I3DObject>();
        public bool IsDisposed { get; private set; }

        public virtual void LoadContent(GraphicsDevice device)
        {
            GraphicsDevice = device;

            _effect = new BasicEffect(GraphicsDevice);
            _effect.TextureEnabled = true;
        }

        public virtual void Update()
        {
            lock (Objects)
            {
                foreach (var obj in Objects)
                    obj.Update();
            }
        }

        public virtual void Draw(Camera camera)
        {
            _effect.View = camera.View;
            _effect.Projection = camera.Projection;

            lock (Objects)
            {
                foreach (var o in Objects)
                {
                    _effect.World = o.World;

                    if (o.Texture != null && (_lastDrawnTexture == null || _lastDrawnTexture != o.Texture))
                    {
                        _lastDrawnTexture = o.Texture;
                        _effect.Texture = o.Texture.Resource;
                    }

                    foreach (var pass in _effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        GraphicsDevice.Indices = o.IndexBuffer;
                        GraphicsDevice.SetVertexBuffer(o.VertexBuffer);
                        GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, o.IndexBuffer.IndexCount / 3);
                    }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ObjectRenderer()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (_effect != null && !_effect.IsDisposed) _effect.Dispose();
                }

                _effect = null;
                Objects = null;

                IsDisposed = true;
            }
        }
    }
}
