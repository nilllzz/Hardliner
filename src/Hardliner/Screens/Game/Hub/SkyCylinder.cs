using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardliner.Engine.Rendering.Geometry.Composers;
using Hardliner.Engine.Rendering.Geometry.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Core;

namespace Hardliner.Screens.Game.Hub
{
    internal class SkyCylinder : LevelObject
    {
        private Texture2D _texture;
        private Player _player;
        private float _rotation;

        public override Texture2D Texture0 => _texture;
        internal override bool IsOpaque => false;
        protected override float CameraDistance => 100f;

        public SkyCylinder(Level level, Player player)
            : base(level)
        {
            _player = player;
            _texture = SkyTextureGenerator.Generate(720, 280, 120);

            // dirty texture rotation:

            var data = new Color[_texture.Width * _texture.Height];
            _texture.GetData(data);
            var rotated = new Texture2D(GameInstance.GraphicsDevice, _texture.Height, _texture.Width);
            var rotatedData = new Color[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                int x = i;
                int y = 0;
                while (x >= _texture.Width)
                {
                    x -= _texture.Width;
                    y++;
                }

                int index = y + x * _texture.Height;

                rotatedData[index] = data[i];
            }

            rotated.SetData(rotatedData);
            _texture = rotated;

        }

        public override void Update()
        {
            _rotation += 0.00005f;

            CreateWorld();
        }

        protected override void CreateWorld()
        {
            World = Matrix.CreateRotationZ(-MathHelper.PiOver2) *
                Matrix.CreateRotationY(_rotation) *
                Matrix.CreateTranslation(new Vector3(0, 1000, 0) + new Vector3(_player.Position.X, 0f, _player.Position.Z));
        }

        protected override void CreateGeometry()
        {
            Geometry.AddVertices(CylinderComposer.Create(2000f, 4000f, 50, 
                new GeometryTexturePoleWrapper(_texture.Bounds, _texture.Bounds, 50),
                new GeometryTextureRectangle(new Rectangle(1, 1, 1, 1), _texture)));
        }
    }
}
