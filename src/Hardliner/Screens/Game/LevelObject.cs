using Hardliner.Engine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Screens.Game
{
    internal abstract class LevelObject : Base3DObject<VertexPositionNormalTexture>
    {
        protected readonly Level _level;
        
        public LevelObject(Level level)
        {
            _level = level;
        }
    }
}
