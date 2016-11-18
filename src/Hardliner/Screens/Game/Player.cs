using Hardliner.Engine;
using Hardliner.Engine.Rendering;
using Hardliner.Engine.Rendering.Geometry.Composers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Screens.Game
{
    internal class Player : Base3DObject<VertexPositionNormalTexture>
    {
        private IdentifiedTexture _texture;

        internal Vector3 Position { get; set; }
        internal float Yaw { get; set; }
        internal float Pitch { get; set; }

        public override IdentifiedTexture Texture => _texture;

        public Player(Vector3 position)
        {
            _texture = new IdentifiedTexture(TextureFactory.FromColor(Color.Black));
            Position = position;
        }

        protected override void CreateWorld()
        {
            World = Matrix.CreateRotationZ(MathHelper.PiOver2) * 
                Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0f) * 
                Matrix.CreateTranslation(Position);
        }

        protected override void CreateGeometry()
        {
            Geometry.AddVertices(CylinderComposer.Create(0.2f, 0.5f, 10));
        }
    }
}
