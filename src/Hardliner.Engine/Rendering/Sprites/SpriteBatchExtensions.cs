using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Engine.Rendering.Sprites
{
    public static class SpriteBatchExtensions
    {
        public static void DrawRectangle(this SpriteBatch batch, Rectangle rectangle, Color color)
        {
            ShapeRenderer.Get(batch.GraphicsDevice).DrawRectangle(batch, rectangle, color);
        }
    }
}
