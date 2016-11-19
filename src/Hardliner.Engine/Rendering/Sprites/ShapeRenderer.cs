using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Engine.Rendering.Sprites
{
    internal class ShapeRenderer
    {
        private static ShapeRenderer _instance;

        internal static ShapeRenderer Get(GraphicsDevice device) => _instance ?? (_instance = new ShapeRenderer(device));

        private readonly Texture2D _pixel;

        private ShapeRenderer(GraphicsDevice device)
        {
            _pixel = new Texture2D(device, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        internal void DrawRectangle(SpriteBatch batch, Rectangle rectangle, Color color)
        {
            batch.Draw(_pixel, rectangle, color);
        }
    }
}
