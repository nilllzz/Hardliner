﻿using Microsoft.Xna.Framework;

namespace Hardliner.Engine.Rendering.Geometry.Texture
{
    internal class DefaultGeometryTextureDefinition : IGeometryTextureDefintion
    {
        private static DefaultGeometryTextureDefinition _instance;

        public static DefaultGeometryTextureDefinition Instance
            => _instance ?? (_instance = new DefaultGeometryTextureDefinition());

        private DefaultGeometryTextureDefinition() { }

        public Vector2 Transform(Vector2 normalVector) => normalVector;

        public void NextElement() { }
        public int GetTextureIndex() => 0;
    }
}
