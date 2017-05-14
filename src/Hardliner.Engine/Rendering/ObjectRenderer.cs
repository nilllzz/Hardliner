using Hardliner.Engine.Rendering.Shaders;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hardliner.Engine.Rendering
{
    public class ObjectRenderer : IDisposable
    {
        private TexturedShader _effect;

        protected GraphicsDevice GraphicsDevice { get; private set; }
        public bool IsDisposed { get; private set; }

        public virtual void LoadContent(GraphicsDevice device, TexturedShader effect)
        {
            GraphicsDevice = device;

            _effect = effect;
            //_effect = new BasicEffect(GraphicsDevice);
            //_effect.TextureEnabled = true;
        }
        
        public virtual void Render(IEnumerable<I3DObject> objects, Camera camera)
        {
            _effect.View = camera.View;
            _effect.Projection = camera.Projection;

            lock (objects)
            {
                foreach (var o in objects.Where(o => o.IsVisible))
                {
                    if (o.BlendState != null)
                        GraphicsDevice.BlendState = o.BlendState;
                    else
                        GraphicsDevice.BlendState = BlendState.AlphaBlend;

                    _effect.World = o.World;
                    if (o.Texture0 != null) _effect.Texture0 = o.Texture0;
                    if (o.Texture1 != null) _effect.Texture1 = o.Texture1;
                    if (o.Texture2 != null) _effect.Texture2 = o.Texture2;
                    _effect.CurrentTechnique.Passes["Pass1"].Apply();
                    
                    GraphicsDevice.Indices = o.IndexBuffer;
                    GraphicsDevice.SetVertexBuffer(o.VertexBuffer);
                    GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, o.IndexBuffer.IndexCount / 3);
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

                IsDisposed = true;
            }
        }
    }
}
