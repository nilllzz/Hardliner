using System;
using Hardliner.Engine;
using Hardliner.Engine.Collision;
using Hardliner.Engine.Rendering;
using Hardliner.Engine.Rendering.Geometry.Composers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hardliner.Screens.Game
{
    internal class Player : LevelObject
    {
        internal const float HEIGHT = 1.6f;

        private const float MAX_SPEED = 0.2f;
        private const float SLOWDOWN = 0.005f;
        internal const float MIN_JUMPCHARGE = 0.1f;
        internal const float MAX_JUMPCHARGE = 0.5f;

        private readonly GameScreen _screen;
        private IdentifiedTexture _texture;
        private Vector3 _velocity;
        private float _jumpCharge = 0f;
        private Vector3 _position;
        private float _groundY;

        internal float Yaw { get; set; }
        internal float Pitch { get; set; }
        internal float Roll { get; set; }
        internal float JumpCharge => _jumpCharge;
        internal Vector3 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }
        internal Vector3 Position => _position;
        public override IdentifiedTexture Texture => _texture;

        public Player(Level level, Vector3 position, GameScreen screen)
            : base(level)
        {
            _screen = screen;
            _texture = new IdentifiedTexture(TextureFactory.FromColor(Color.DarkGray));
            _position = position;

            IsVisible = false;
            CreateCollider();
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

        private void CreateCollider()
        {
            Collider = new BoxCollider(new BoundingBox(
                new Vector3(-0.25f, 0, -0.25f) + _position, 
                new Vector3(0.25f, HEIGHT, 0.25f) + _position));
        }

        public override void Update()
        {
            _groundY = GetGroundY();

            LookAround();
            Movement();
            Jump();
            UpdateVelocity();
            Gravity();
            Rope();

            CreateCollider();
            CreateWorld();
        }

        private void Movement()
        {
            var gState = GamePad.GetState(PlayerIndex.One);
            Vector2 movement = gState.ThumbSticks.Left * new Vector2(0.02f, 0.008f);

            _velocity += new Vector3(movement.X, 0f, -movement.Y);

            if (_velocity.X > MAX_SPEED)
            {
                _velocity.X = MAX_SPEED;
            }
            else if (_velocity.X < -MAX_SPEED)
            {
                _velocity.X = -MAX_SPEED;
            }
            if (_velocity.Z > MAX_SPEED)
            {
                _velocity.Z = MAX_SPEED;
            }
            else if (_velocity.Z < -MAX_SPEED)
            {
                _velocity.Z = -MAX_SPEED;
            }

            if (_position.Y == _groundY)
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
            if (_position.Y == _groundY)
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
                        _velocity.Z -= 0.1f;
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

            Yaw = MathHelper.WrapAngle(Yaw);
            Pitch = MathHelper.WrapAngle(Pitch);

            if (look.X < 0f)
            {
                if (Roll < 0.05f)
                {
                    Roll -= 0.04f * look.X;
                }
            }
            else if (look.X > 0f)
            {
                if (Roll > -0.05f)
                {
                    Roll -= 0.04f * look.X;
                }
            }
            else
            {
                if (Roll != 0f)
                {
                    Roll = MathHelper.Lerp(Roll, 0f, 0.1f);
                }
            }
        }

        private void Gravity()
        {
            if (_position.Y > _groundY)
            {
                _velocity.Y -= 0.01f;
            }
            else
            {
                _velocity.Y = 0f;
                _position.Y = _groundY;
            }
        }

        private void UpdateVelocity()
        {
            var rotation = Matrix.CreateRotationY(Yaw);
            var transformedVelocity = Vector3.Transform(_velocity, rotation);

            _position += transformedVelocity;
        }

        private void Rope()
        {
            var gState = GamePad.GetState(PlayerIndex.One);
            if (gState.Triggers.Right != 0f && !_level.HasRope)
            {
                _level.AddObject(new HookShotRope(_level, _screen.Content, this));
            }
        }

        private float GetGroundY()
        {
            var ground = float.MinValue;

            foreach (var o in _level.Objects)
            {
                var colliderTop = o.Collider.Top;
                if (colliderTop > ground && colliderTop <= Collider.Bottom)
                {
                    var collider = new BoxCollider(new BoundingBox(
                        new Vector3(-0.25f + _position.X, colliderTop - 0.01f, -0.25f + _position.Z), 
                        new Vector3(0.25f + _position.X, colliderTop + 0.01f, 0.25f + _position.Z)));
                    if (o.Collider.Collides(collider))
                    {
                        ground = colliderTop;
                    }
                }
            }
            
            return ground;
        }
    }
}
