using System;
using Microsoft.Xna.Framework.Content;
using static Core;

namespace Hardliner.Screens
{
    internal abstract class Screen : IDisposable
    {
        private ContentManager _content;

        internal ContentManager Content =>
            _content ?? (_content = new ContentManager(GameInstance.Services, "Content"));
        internal virtual bool IsRoot => true;
        protected Screen PreScreen { get; private set; }
        internal bool IsDisposed { get; private set; }

        internal virtual void Draw() { }
        internal abstract void Render();
        internal abstract void Update();

        internal virtual void Activate(Screen preScreen)
        {
            PreScreen = preScreen;
        }

        internal virtual void Deactivate()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Screen()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (_content != null) _content.Dispose();
                }

                _content = null;
                PreScreen = null;

                IsDisposed = true;
            }
        }
    }
}
