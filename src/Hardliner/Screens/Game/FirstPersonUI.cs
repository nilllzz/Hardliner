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
using Hardliner.Screens.Game.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using static Core;

namespace Hardliner.Screens.Game
{
    internal class FirstPersonUI : LevelObject
    {
        private const float WIDTH = 0.44f;
        private const float HEIGHT = 0.28f;
        private const float DEPTH = 0.075f;

        private Texture2D _texture;
        private Texture2D _uiOverlay;
        private readonly Player _player;
        private SpriteBatch _uiBatch;
        private RenderTarget2D _target;
        private SpriteFont _uiFont, _introFont;
        private Texture2D[] _noise;
        private ulong _hookShotMessageColor = 0;
        private int _introAlpha = 255;

        private readonly string[] _bootTexts = new[]
        {
            "",
            "",
            "",
            "",
            "Hello World",
            "Loading...",
            "Boot Sequence initiated",
            "OS Core loaded",
            "Accessed update submodule",
            "Recalculated objective trackKeeper",
            "Analyzed infrastructure data",
            "Connection speed tested",
            "Piped data to martial core",
            "Life support systems online",
            "Enabled hook shot",
            "Checking for faulty hardware",
            "Report: Positive outcome.",
            "Hardware 100% operational.",
            "Initialized combatSuit.py...",
            "Loaded UI display font.",
            "",
            "",
            "",
            "",
        };

        protected override float CameraDistance => 0f;
        internal override bool IsOpaque => false;
        public override Texture2D Texture0 => _texture;

        public FirstPersonUI(Level level, ContentManager content, Player player)
            : base(level)
        {
            _player = player;
            _uiBatch = new SpriteBatch(GameInstance.GraphicsDevice);
            _target = new RenderTarget2D(GameInstance.GraphicsDevice,
                GameInstance.Window.ClientBounds.Width, GameInstance.Window.ClientBounds.Height);

            _uiOverlay = content.Load<Texture2D>(Resources.Textures.UIOverlay);
            _uiFont = content.Load<SpriteFont>(Resources.Fonts.UIFont);
            _introFont = content.Load<SpriteFont>(Resources.Fonts.IntroFont);
            _texture = _target;

            GenerateNoise();
        }

        private void GenerateNoise()
        {
            const int width = 400, height = 280;
            var random = new Random();

            _noise = new Texture2D[5];
            for (var i = 0; i < _noise.Length; i++)
            {
                var texture = new Texture2D(GameInstance.GraphicsDevice, width, height);
                var data = new Color[width * height];
                for (var j = 0; j < data.Length; j++)
                {
                    var value = random.Next(10, 50) * 5;
                    data[j] = new Color(value, value, value);
                }
                texture.SetData(data);
                _noise[i] = texture;
            }
        }

        internal void DrawTexture()
        {
            GameInstance.GraphicsDevice.SetRenderTarget(_target);
            GameInstance.GraphicsDevice.Clear(Color.Transparent);

            _uiBatch.Begin(blendState: BlendState.NonPremultiplied);

            _uiBatch.Draw(_uiOverlay, GameInstance.Bounds, new Color(255, 255, 255, 120));

            DrawRopeStats();
            DrawJumpStats();
            DrawReticle();

            DrawBoot();

            _uiBatch.End();

            GameInstance.GraphicsDevice.SetRenderTarget(null);
        }

        private void DrawRopeStats()
        {
            var rope = _player.ClawShotGun.ActiveRope;
            if (rope != null)
            {
                if (rope.Status == ClawStatus.Searching && _level.HasRope)
                {
                    var textSize = _introFont.MeasureString("ACQUIRING TARGET...") * 0.5f;
                    var yellow = new Color(249, 201, 0, (int)(_hookShotMessageColor % 255));

                    _uiBatch.DrawString(_introFont, "ACQUIRING TARGET...",
                        new Vector2(GameInstance.Bounds.Width / 2f - textSize.X / 2f, 48),
                        yellow, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);

                    var lengthPercent = rope.Length / ClawRope.MAX_LENGTH;

                    _uiBatch.DrawRectangle(
                        new Rectangle(GameInstance.Bounds.Width / 2 - 150, 80, 300, 20),
                        new Color(150, 150, 150, 150));
                    _uiBatch.DrawRectangle(
                        new Rectangle(GameInstance.Bounds.Width / 2 - 148, 82, (int)(lengthPercent * 296), 16),
                        yellow);
                }
                else if (_player.ClawShotGun.Cooldown > 0)
                {
                    var text = "";
                    var color = Color.Black;

                    switch (rope.Status)
                    {
                        case ClawStatus.Intercepted:
                            text = "INTERCEPTED";
                            color = new Color(215, 71, 13, (int)(_hookShotMessageColor % 255));
                            break;
                        case ClawStatus.ClawHit:
                            text = "ACQUIRED";
                            color = new Color(13, 156, 216, (int)(_hookShotMessageColor % 255));
                            break;
                        case ClawStatus.OutOfRange:
                            text = "OUT OF RANGE";
                            color = new Color(215, 71, 13, (int)(_hookShotMessageColor % 255));
                            break;
                    }
                    
                    var textSize = _introFont.MeasureString(text) * 0.5f;

                    _uiBatch.DrawString(_introFont, text,
                        new Vector2(GameInstance.Bounds.Width / 2f - textSize.X / 2f, 48),
                        color, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
                }
            }
        }

        private void DrawJumpStats()
        {
            _uiBatch.DrawRectangle(new Rectangle(64, 64, 200, 10), new Color(200, 200, 200, 50));

            if (_player.JumpCharge > 0f)
            {
                var length = (int)((_player.JumpCharge - Player.MIN_JUMPCHARGE) / (Player.MAX_JUMPCHARGE - Player.MIN_JUMPCHARGE) * 200f);

                if (length > 200)
                    length = 200;

                _uiBatch.DrawRectangle(new Rectangle(64, 64, length, 10), new Color(255, 255, 255));
            }
            else if (_player.JetPackCharge > 0f)
            {
                var length = (int)((_player.JetPackCharge - Player.MIN_JUMPCHARGE) / (Player.MAX_JUMPCHARGE - Player.MIN_JUMPCHARGE) * 200f);

                if (length > 200)
                    length = 200;

                _uiBatch.DrawRectangle(new Rectangle(64, 64, length, 10), new Color(249, 201, 0));
            }

            _uiBatch.DrawString(_introFont, Math.Round(_player.Position.Y * 3.28084f, 1) + " ft.",
                new Vector2(64, 32), new Color(255, 255, 255, 50), 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);

            _uiBatch.DrawString(_introFont, Math.Round(_player.Velocity.Z * -100f, 1) + " mph",
                new Vector2(170, 32), new Color(255, 255, 255, 50), 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
        }

        private void DrawReticle()
        {
            _uiBatch.DrawRectangle(new Rectangle(GameInstance.Bounds.Width / 2 - 2,
                GameInstance.Bounds.Height / 2 - 8, 4, 16), Color.White);
            _uiBatch.DrawRectangle(new Rectangle(GameInstance.Bounds.Width / 2 - 8,
                GameInstance.Bounds.Height / 2 - 2, 16, 4), Color.White);
        }

        private void DrawBoot()
        {
            if (_introAlpha > 0)
            {
                var bootItems = new List<string>();
                var limit = (1 - (_introAlpha / 255f)) * (_bootTexts.Length);

                for (var i = 0; i < limit; i++)
                    bootItems.Add(_bootTexts[i]);

                while (bootItems.Count > 7)
                    bootItems.RemoveAt(0);

                _uiBatch.DrawString(_uiFont, string.Join("\n", bootItems.ToArray()),
                    new Vector2(GameInstance.Bounds.Width / 2f - 240, 170),
                    new Color(255, 255, 255, _introAlpha), 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);

                _uiBatch.Draw(_noise[_introAlpha % (_noise.Length * 3) / 3],
                    GameInstance.Bounds, new Color(80, 80, 80, _introAlpha));
            }
            else
            {
                _uiBatch.Draw(_noise[DateTime.UtcNow.Millisecond % (_noise.Length * 50) / 50],
                    GameInstance.Bounds, new Color(80, 80, 80, 10));
            }
        }

        public override void Update()
        {
            if (_introAlpha > 0 && _player.HasMoved)
                _introAlpha--;

            _hookShotMessageColor += 15;

            CreateWorld();
        }

        protected override void CreateWorld()
        {
            var rotation = Matrix.CreateFromYawPitchRoll(_player.Yaw, _player.Pitch, 0f);
            var transformed = Vector3.Transform(new Vector3(0, 0, (1 - _introAlpha / 255f) * -0.25f), rotation);

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
