using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Core;

namespace Hardliner
{
    public static class TextureFactory
    {
        public static Texture2D FromColor(Color color)
        {
            var texture = new Texture2D(GameInstance.GraphicsDevice, 1, 1);
            texture.SetData(new[] { color });
            return texture;
        }
    }
}
