using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Core;

namespace Hardliner
{
    public static class TextureFactory
    {
        public static Texture2D FromColor(Color color)
             => FromColor(new[] { color });

        public static Texture2D FromColor(Color[] colors)
        {
            var texture = new Texture2D(GameInstance.GraphicsDevice, colors.Length, 1);
            texture.SetData(colors);
            return texture;
        }
    }
}
