using System;
using Hardliner.Engine.Rendering;
using Hardliner.Engine.Rendering.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Screens.Game
{
    internal abstract class LevelObject : Base3DObject<VertexInput>, IComparable
    {
        protected readonly Level _level;
        internal bool ToBeRemoved { get; set; }
        internal virtual bool IsOpaque { get; } = true;
        protected virtual float CameraDistance { get; } = 0f;

        public LevelObject(Level level)
        {
            _level = level;
        }

        public int CompareTo(object obj)
        {
            var other = obj as LevelObject;
            
            if (!IsOpaque && !other.IsOpaque)
            {
                return CameraDistance < other.CameraDistance ? 
                    1 : -1;
            }

            return 0;
        }
    }
}
