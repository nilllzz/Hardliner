using System;
using Hardliner.Engine;
using Hardliner.Engine.Collision;
using Hardliner.Engine.Rendering;
using Hardliner.Engine.Rendering.Geometry.Composers;
using Hardliner.Screens.Game.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hardliner.Screens.Game
{
    internal class Player : LevelObject
    {
        internal const float HEIGHT = 2f;

        private const float MAX_SPEED = 0.2f;
        private const float SLOWDOWN = 0.005f;
        internal const float MIN_JUMPCHARGE = 0.1f;
        internal const float MAX_JUMPCHARGE = 0.5f;
        private const float JUMP_STRENGTH = 0.5f;

        private const Buttons JETPACK_BUTTON = Buttons.LeftTrigger;

        private readonly GameScreen _screen;
        private Texture2D _texture;
        private Vector3 _velocity;
        private float _jumpCharge = 0f;
        private Vector3 _position;
        private float _lookMovement = 0f;
        private ColliderController _colliderController;

        internal float Yaw { get; set; }
        internal float Pitch { get; set; }
        internal float Roll { get; set; }
        internal float JumpCharge => _jumpCharge;
        internal float JetPackCharge { get; set; }
        internal Vector3 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }
        internal Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        internal ClawShotGun ClawShotGun { get; private set; }
        public override Texture2D Texture0 => _texture;
        internal bool HasMoved { get; private set; } = false;

        public Player(Level level, Vector3 position, GameScreen screen)
            : base(level)
        {
            _screen = screen;
            _texture = TextureFactory.FromColor(Color.DarkGray);
            _position = position;
            _colliderController = new ColliderController(this, level);
            ClawShotGun = new ClawShotGun(level, this);

            IsVisible = false;
            CreateCollider();

            Pitch = -0.4f;
        }

        protected override void CreateWorld()
        {
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
            _colliderController.Update();

            LookAround();
            Movement();
            Jump();
            UpdateVelocity();
            Gravity();
            ClawShotGun.Update();

            CreateCollider();
            CreateWorld();
        }

        private void Movement()
        {
            var gState = GamePad.GetState(PlayerIndex.One);

            var speedVector = new Vector2(0.02f, 0.008f);
            // when the player is off the ground and not using a jetpack, movement speed is half.
            if (_colliderController.GroundY != _position.Y && !gState.IsButtonDown(JETPACK_BUTTON))
            {
                speedVector *= 0.5f;
            }

            var direction = gState.ThumbSticks.Left;
            Vector2 movement = direction * speedVector;

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

            if (_position.Y == _colliderController.GroundY)
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
            var gState = GamePad.GetState(PlayerIndex.One);
            if (_position.Y == _colliderController.GroundY)
            {
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

                        var jumpVelocity = _jumpCharge * JUMP_STRENGTH;
                        _velocity.Y += jumpVelocity;
                        _velocity.Z -= jumpVelocity / 2f;
                        JetPackCharge = MAX_JUMPCHARGE / 1.5f - _jumpCharge / 8f;

                        _jumpCharge = 0f;
                    }
                }
            }
            else
            {
                if (_velocity.Y < 0.1f && JetPackCharge > 0f && gState.IsButtonDown(JETPACK_BUTTON))
                {
                    _velocity.Y += 0.02f;
                    JetPackCharge -= 0.01f;
                    if (JetPackCharge <= 0f)
                        JetPackCharge = 0f;
                }
            }
        }

        private void LookAround()
        {
            var gState = GamePad.GetState(PlayerIndex.One);
            Vector2 look = gState.ThumbSticks.Right * 0.075f;

            if (look.X != 0f || look.Y != 0f)
            {
                if (_lookMovement < 1f)
                {
                    _lookMovement = MathHelper.Lerp(_lookMovement, 1f, 0.15f);
                    if (_lookMovement >= 1f)
                        _lookMovement = 1f;
                }

                look *= _lookMovement;

                var yawDiff = look.X;
                if (yawDiff != 0f)
                {
                    var rotation = Matrix.CreateFromYawPitchRoll(yawDiff, 0f, 0f);
                    _velocity = Vector3.Transform(_velocity, rotation);
                    Yaw -= yawDiff;
                }

                Pitch += look.Y;

                Yaw = MathHelper.WrapAngle(Yaw);
                Pitch = MathHelper.WrapAngle(Pitch);
            }
            else
            {
                _lookMovement = 0f;
            }

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
            if (_position.Y > _colliderController.GroundY)
            {
                _velocity.Y -= 0.01f;
            }
            else
            {
                _velocity.Y = 0f;
                _position.Y = _colliderController.GroundY;
                JetPackCharge = 0f;
            }
        }

        private void UpdateVelocity()
        {
            var rotation = Matrix.CreateRotationY(Yaw);
            var transformedVelocity = Vector3.Transform(_velocity, rotation);

            _position = _colliderController.ChangePosition(_position, transformedVelocity, new Vector3(0.5f, 0f, 0.5f));

            if (transformedVelocity.Z != 0f || transformedVelocity.X != 0f)
                HasMoved = true;

            //if (_colliderController.HadCollisionX)
            //    _velocity.X = 0f;
            //if (_colliderController.HadCollisionZ)
            //    _velocity.Z = 0f;
        }
    }
}
