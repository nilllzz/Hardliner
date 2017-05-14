using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardliner.Engine;
using Hardliner.Engine.Collision;
using Hardliner.Engine.Rendering.Geometry.Composers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Screens.Game.Weapons
{
    internal class ClawRope : LevelObject
    {
        internal const float MAX_LENGTH = 50f;
        private const float SPEED = 1.2f;
        private const float SIZE = 0.02f;
        private const float MAX_REEL_IN_SPEED = 1.2f;

        private Texture2D _texture;
        private Player _player;
        private float _yaw, _pitch;
        private Vector3 _originPosition;
        private float _reelInSpeed = 0.01f;

        internal ClawStatus Status { get; private set; } = ClawStatus.Searching;
        internal float Length { get; private set; }
        public override Texture2D Texture0 => _texture;

        public ClawRope(Level level, Player player)
            : base(level)
        {
            _player = player;

            _originPosition = _player.Position + GetPlayerOffset();
            _yaw = _player.Yaw;
            _pitch = _player.Pitch;
            _texture = TextureFactory.FromColor(new Color(79, 64, 35));
        }

        protected override void CreateGeometry()
        {
            Geometry.AddVertices(CuboidComposer.Create(1f));
        }

        public override void Update()
        {
            if (Status != ClawStatus.Disengaged)
            {
                if (Status == ClawStatus.ClawHit)
                {
                    DrawInPlayer();
                }
                else
                {
                    Length += SPEED;

                    if (Length > MAX_LENGTH)
                    {
                        Status = ClawStatus.OutOfRange;
                    }
                    else
                    {
                        var intersectResult = CheckIntersect();
                        if (Status != ClawStatus.ClawHit && intersectResult)
                        {
                            Status = ClawStatus.Intercepted;
                        }
                    }
                }
            }
            
            CreateWorld();
        }

        private static Vector3 GetPlayerOffset() => new Vector3(0, Player.HEIGHT, 0);

        private Vector3 GetEndPoint()
        {
            var rotation = Matrix.CreateFromYawPitchRoll(_yaw, _pitch, 0f);
            var endPoint = Vector3.Transform(new Vector3(0f, 0f, -Length), rotation) + _originPosition;

            return endPoint;
        }

        private bool CheckIntersect()
        {
            var endPoint = GetEndPoint();
            var startPoint = _player.Position + GetPlayerOffset();

            var direction = (endPoint - startPoint);
            direction.Normalize();
            var distance = Vector3.Distance(startPoint, endPoint);

            var collider = new RayCollider { Ray = new Ray(startPoint, direction) };

            return _level.Objects.Any(o =>
            {
                if (!(o is Player))
                {
                    var rayCastResult = collider.Intersects(o.Collider);
                    if (rayCastResult.HasValue && Math.Abs(rayCastResult.Value) <= distance)
                    {
                        if (Math.Abs(rayCastResult.Value - distance) <= SPEED)
                            Status = ClawStatus.ClawHit;

                        return true;
                    }
                }
                return false;
            });
        }

        private void DrawInPlayer()
        {
            var endPoint = GetEndPoint();
            var startPoint = _player.Position + GetPlayerOffset();

            var distance = Vector3.Distance(startPoint, endPoint);

            if (distance < 0.5f)
            {
                Status = ClawStatus.Disengaged;
            }
            else
            {
                _reelInSpeed = MathHelper.Lerp(_reelInSpeed, MAX_REEL_IN_SPEED, 0.05f);

                var direction = (endPoint - startPoint);
                direction.Normalize();
                
                direction.Z = -_reelInSpeed;
                direction.X = 0f;
                direction.Y /= 2f;

                var rotation = Matrix.CreateFromYawPitchRoll(_yaw - _player.Yaw, 0f, 0f);
                direction = Vector3.Transform(direction, rotation);

                _player.Velocity = direction;
            }
        }

        protected override void CreateWorld()
        {
            var rotation = Matrix.CreateFromYawPitchRoll(_player.Yaw, 0f, 0f);
            var visualOffset = Vector3.Transform(new Vector3(0.1f, -0.1f, 0f), rotation);

            var endPoint = GetEndPoint();
            var startPoint = _player.Position + GetPlayerOffset() + visualOffset;
            var midPoint = (endPoint + startPoint) / 2f;

            var distance = Vector3.Distance(startPoint, endPoint);
            var planeDistance = Vector2.Distance(new Vector2(endPoint.X, endPoint.Z), new Vector2(startPoint.X, startPoint.Z));

            var delta = endPoint - startPoint;
            var yaw = (float)Math.Atan2(delta.Z, delta.X);
            var pitch = (float)Math.Atan(delta.Y / planeDistance);

            World = Matrix.CreateScale(SIZE, SIZE, distance) *
                Matrix.CreateFromYawPitchRoll(-yaw + MathHelper.PiOver2, -pitch, 0f) *
                Matrix.CreateTranslation(midPoint);
        }
    }
}
