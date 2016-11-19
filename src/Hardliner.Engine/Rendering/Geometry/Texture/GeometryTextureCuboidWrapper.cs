using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hardliner.Engine.Rendering.Geometry.Texture
{
    public class GeometryTextureCuboidWrapper : IGeometryTextureDefintion
    {
        private Dictionary<CuboidSide, IGeometryTextureDefintion> _definitions;
        private int _currentSideIndex = 1;

        public GeometryTextureCuboidWrapper()
        {
            _definitions = new Dictionary<CuboidSide, IGeometryTextureDefintion>();
        }

        public GeometryTextureCuboidWrapper(Dictionary<CuboidSide, Rectangle> definitions, Rectangle textureBounds)
        {
            _definitions = definitions.ToDictionary(
                p => p.Key, 
                p => new GeometryTextureRectangle(p.Value, textureBounds) as IGeometryTextureDefintion);
        }

        public void AddSide(CuboidSide side, IGeometryTextureDefintion textureDefinition)
        {
            _definitions.Add(side, textureDefinition);
        }

        public void NextElement()
        {
            _currentSideIndex++;
        }

        public Vector2 Transform(Vector2 normalVector)
        {
            var currentSide = (CuboidSide)_currentSideIndex;
            if (_definitions.ContainsKey(currentSide))
                return _definitions[currentSide].Transform(normalVector);
            else
                return normalVector;
        }
    }
}
