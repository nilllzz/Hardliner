using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardliner.Content;
using Hardliner.Engine;
using Hardliner.Engine.Rendering;
using Hardliner.Engine.Rendering.Geometry.Composers;
using Hardliner.Engine.Rendering.Geometry.Texture;
using Hardliner.Engine.Rendering.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using static Core;

namespace Hardliner.Screens.Game
{
    internal class FirstPersonUI : Base3DObject<VertexPositionNormalTexture>
    {
        private const float WIDTH = 0.4f;
        private const float HEIGHT = 0.28f;
        private const float DEPTH = 0.075f;

        private IdentifiedTexture _texture;
        private Texture2D _uiOverlay;
        private readonly Player _player;
        private SpriteBatch _uiBatch;
        private RenderTarget2D _target;
        private SpriteFont _font;

        public override IdentifiedTexture Texture => _texture;

        public FirstPersonUI(ContentManager content, Player player)
        {
            _player = player;
            _uiBatch = new SpriteBatch(GameInstance.GraphicsDevice);
            _target = new RenderTarget2D(GameInstance.GraphicsDevice,
                GameInstance.Window.ClientBounds.Width, GameInstance.Window.ClientBounds.Height);

            _uiOverlay = content.Load<Texture2D>(Resources.Textures.UIOverlay);
            _font = content.Load<SpriteFont>(Resources.Fonts.UIFont);
            _texture = new IdentifiedTexture(_target);
        }

        internal void DrawTexture()
        {
            GameInstance.GraphicsDevice.SetRenderTarget(_target);
            GameInstance.GraphicsDevice.Clear(Color.Transparent);

            _uiBatch.Begin(blendState: BlendState.NonPremultiplied);

            _uiBatch.Draw(_uiOverlay, new Rectangle(0, 0, GameInstance.Window.ClientBounds.Width, GameInstance.Window.ClientBounds.Height), new Color(255, 255, 255, 120));

            if (_player.JumpCharge > 0f)
            {
                var length = (int)((_player.JumpCharge - Player.MIN_JUMPCHARGE) / (Player.MAX_JUMPCHARGE - Player.MIN_JUMPCHARGE) * 200f);

                if (length > 200)
                    length = 200;

                _uiBatch.DrawRectangle(new Rectangle(64, 64, 200, 10), new Color(255, 255, 255, 100));
                _uiBatch.DrawRectangle(new Rectangle(64, 64, length, 10), new Color(200, 200, 200));
            }
            else
            {
                _uiBatch.DrawRectangle(new Rectangle(64, 64, 200, 10), new Color(200, 200, 200, 50));
            }

            _uiBatch.DrawString(_font, Math.Round(_player.Position.Y * 3.28084f, 1) + " ft.", new Vector2(64, 46), Color.White);

            _uiBatch.End();

            GameInstance.GraphicsDevice.SetRenderTarget(null);
        }

        public override void Update()
        {
            CreateWorld();
        }

        protected override void CreateWorld()
        {
            var rotation = Matrix.CreateFromYawPitchRoll(_player.Yaw, _player.Pitch, 0f);
            var transformed = Vector3.Transform(new Vector3(0, 0, -0.25f), rotation);
            
            World = rotation * Matrix.CreateTranslation(_player.Position + new Vector3(0, Player.HEIGHT, 0) + transformed);
        }

        protected override void CreateGeometry()
        {
            var left = RectangleComposer.Create(new[]
                {
                    new Vector3(-WIDTH, HEIGHT, 0f),
                    new Vector3(0f, HEIGHT, -DEPTH),
                    new Vector3(-WIDTH, -HEIGHT, 0f),
                    new Vector3(0f, -HEIGHT, -DEPTH),
                },
                new GeometryTextureRectangle(0, 0, 0.5f, 1f));

            var right = RectangleComposer.Create(new[]
                {
                    new Vector3(0f, HEIGHT, -DEPTH),
                    new Vector3(WIDTH, HEIGHT, 0f),
                    new Vector3(0f, -HEIGHT, -DEPTH),
                    new Vector3(WIDTH, -HEIGHT, 0f),
                },
                new GeometryTextureRectangle(0.5f, 0, 0.5f, 1f));

            Geometry.AddVertices(left);
            Geometry.AddVertices(right);
        }
    }
}
