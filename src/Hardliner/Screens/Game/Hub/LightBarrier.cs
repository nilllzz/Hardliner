using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardliner.Engine.Rendering.Geometry.Composers;
using Hardliner.Engine.Rendering.Geometry.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Screens.Game.Hub
{
    internal class LightBarrier : LevelObject
    {
        private static Dictionary<int, Texture2D> TextureCache = new Dictionary<int, Texture2D>();

        private float _height;
        private Texture2D _texture;
        private Player _player;

        public override Texture2D Texture0 => _texture;
        internal override bool IsOpaque => false;
        public override BlendState BlendState => BlendState.Additive;

        public LightBarrier(Level level, Player player, float height)
            : base(level)
        {
            _player = player;
            _height = height;
            SetTexture();
        }

        private void SetTexture()
        {
            if (_player.Position.Y <= _height)
                IsVisible = false;
            else
            {
                IsVisible = true;
                var alpha = (int)(_player.Position.Y - _height);
                if (alpha > 25)
                    alpha = 25;
                
                Texture2D texture;
                if (!TextureCache.TryGetValue(alpha, out texture))
                {
                    texture = TextureFactory.FromColor(new Color(255, 250, 199, alpha));
                    TextureCache.Add(alpha, texture);
                }

                _texture = texture;
            }
        }

        public override void Update()
        {
            SetTexture();
        }

        protected override void CreateWorld()
        {
            World = Matrix.CreateTranslation(0, _height, 0);
        }

        protected override void CreateGeometry()
        {
            var vertices = RectangleComposer.Create(1000f, 1000f);

            Geometry.AddVertices(vertices);
        }
    }
}
