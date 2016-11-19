using System;
using Hardliner.Engine;
using Hardliner.Engine.Rendering;
using Hardliner.Engine.Rendering.Geometry.Composers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hardliner.Screens.Game
{
    internal class Player : Base3DObject<VertexPositionNormalTexture>
    {
        internal const float HEIGHT = 1.6f;

        private const float SPEED = 0.08f;
        private const float SLOWDOWN = 0.005f;
        internal const float MIN_JUMPCHARGE = 0.1f;
        internal const float MAX_JUMPCHARGE = 0.5f;

        private IdentifiedTexture _texture;
        private Vector3 _position;
        private Vector3 _velocity;
        private float _jumpCharge = 0f;

        internal Vector3 Position => _position;
        internal float Yaw { get; set; }
        internal float Pitch { get; set; }
        internal float JumpCharge => _jumpCharge;
        public override IdentifiedTexture Texture => _texture;

        public Player(Vector3 position)
        {
            _texture = new IdentifiedTexture(TextureFactory.FromColor(Color.DarkGray));
            _position = position;
        }

        protected override void CreateWorld()
        {
            //World = Matrix.CreateRotationZ(MathHelper.PiOver2) *
            //    Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0f) *
            //    Matrix.CreateTranslation(Position);
            World = Matrix.CreateRotationZ(MathHelper.PiOver2) *
                Matrix.CreateTranslation(Position + new Vector3(0, HEIGHT / 2, 0));
        }

        protected override void CreateGeometry()
        {
            Geometry.AddVertices(CylinderComposer.Create(0.1f, HEIGHT, 10));
        }

        public override void Update()
        {
            LookAround();
            Movement();
            Jump();
            Velocity();
            Gravity();

            CreateWorld();
        }

        private void Movement()
        {
            var gState = GamePad.GetState(PlayerIndex.One);
            Vector2 movement = gState.ThumbSticks.Left * new Vector2(0.02f, 0.008f);
            
            _velocity += new Vector3(movement.X, 0f, -movement.Y);
            if (_velocity.X > SPEED)
            {
                _velocity.X = SPEED;
            }
            else if (_velocity.X < -SPEED)
            {
                _velocity.X = -SPEED;
            }
            if (_velocity.Z > SPEED)
            {
                _velocity.Z = SPEED;
            }
            else if (_velocity.Z < -SPEED)
            {
                _velocity.Z = -SPEED;
            }

            if (_position.Y == 0f)
            {
                if (_velocity.X > 0f)
                {
                    _velocity.X -= SLOWDOWN;
                    if (_velocity.X <= 0f)
                        _velocity.X = 0f;
                }
                else if (_velocity.X < 0f)
                {
                    _velocity.X += SLOWDOWN;
                    if (_velocity.X >= 0f)
                        _velocity.X = 0f;
                }
                if (_velocity.Z > 0f)
                {
                    _velocity.Z -= SLOWDOWN;
                    if (_velocity.Z <= 0f)
                        _velocity.Z = 0f;
                }
                else if (_velocity.Z < 0f)
                {
                    _velocity.Z += SLOWDOWN;
                    if (_velocity.Z >= 0f)
                        _velocity.Z = 0f;
                }
            }
        }

        private void Jump()
        {
            if (_position.Y == 0f)
            {
                var gState = GamePad.GetState(PlayerIndex.One);
                if (gState.IsButtonDown(Buttons.A))
                {
                    if (_jumpCharge == 0f)
                    {
                        _jumpCharge = MIN_JUMPCHARGE;
                    }
                    _jumpCharge += 0.01f;
                }
                else
                {
                    if (_jumpCharge > 0f)
                    {
                        if (_jumpCharge > MAX_JUMPCHARGE)
                            _jumpCharge = MAX_JUMPCHARGE;

                        _velocity.Y += _jumpCharge;
                        _jumpCharge = 0f;
                    }
                }
            }
        }

        private void LookAround()
        {
            var gState = GamePad.GetState(PlayerIndex.One);
            Vector2 look = gState.ThumbSticks.Right * 0.1f;

            Yaw -= look.X;
            Pitch += look.Y;
        }

        private void Gravity()
        {
            if (_position.Y > 0f)
            {
                _velocity.Y -= 0.01f;
            }
            else
            {
                _velocity.Y = 0f;
                _position.Y = 0f;
            }
        }

        private void Velocity()
        {
            var rotation = Matrix.CreateRotationY(Yaw);
            var transformedVelocity = Vector3.Transform(_velocity, rotation);

            _position += transformedVelocity;
        }
    }
}
