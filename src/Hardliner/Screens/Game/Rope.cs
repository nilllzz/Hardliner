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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Screens.Game
{
    internal class Rope : Base3DObject<VertexPositionNormalTexture>
    {
        internal float MAX_LENGTH = 10f;

        private float _length = 0f;
        private IdentifiedTexture _texture;
        private Player _origin;
        private float _yaw;
        private float _pitch;

        internal float Length => _length;
        public override IdentifiedTexture Texture => _texture;

        public Rope(ContentManager content, Player origin)
        {
            _texture = new IdentifiedTexture(TextureFactory.FromColor(new Color(125, 83, 9)));
            _origin = origin;
            _yaw = _origin.Yaw;
            _pitch = _origin.Pitch;
        }

        public override void Update()
        {
            _length += 0.1f;
            CreateWorld();
        }

        protected override void CreateWorld()
        {
            var startPoint = _origin.Position;
            var rotation = Matrix.CreateFromYawPitchRoll(_yaw, _pitch, 0f);
            var endPoint = Vector3.Transform(new Vector3(0f, 1f, -_length), rotation);
            var midPoint = (endPoint + startPoint) / 2f;

            var distance = Vector3.Distance(startPoint, endPoint);
            var planeDistance = Vector2.Distance(new Vector2(endPoint.X, endPoint.Z), new Vector2(startPoint.X, startPoint.Z));

            var delta = endPoint - startPoint;
            var yaw = (float)Math.Atan2(delta.Z, delta.X);
            var pitch = (float)Math.Atan(delta.Y / planeDistance);

            World = Matrix.CreateScale(0.02f, 0.02f, distance) *
                Matrix.CreateFromYawPitchRoll(-yaw + MathHelper.PiOver2, -pitch, 0f) *
                Matrix.CreateTranslation(midPoint);
        }

        protected override void CreateGeometry()
        {
            Geometry.AddVertices(CuboidComposer.Create(1f));
        }
    }
}
