using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardliner.Content;
using Hardliner.Engine;
using Hardliner.Engine.Collision;
using Hardliner.Engine.Rendering;
using Hardliner.Engine.Rendering.Geometry.Composers;
using Hardliner.Engine.Rendering.Geometry.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hardliner.Screens.Game
{
    internal class HookShotRope : LevelObject
    {
        internal const float MAX_LENGTH = 25f;
        private const float SPEED = 0.5f;

        private float _length = 0f;
        private IdentifiedTexture _texture;
        private Player _origin;
        private float _yaw;
        private float _pitch;
        private Vector3 _offset;
        private bool _hookHit = false;

        internal float Length => _length;
        public override IdentifiedTexture Texture => _texture;

        public HookShotRope(Level level, ContentManager content, Player origin)
            : base(level)
        {
            _texture = new IdentifiedTexture(TextureFactory.FromColor(new Color(125, 83, 9)));
            _origin = origin;
            _yaw = _origin.Yaw;
            _pitch = _origin.Pitch;
            _offset = _origin.Position;
        }

        public override void Update()
        {
            _length += Math.Abs(_origin.Velocity.Z) * 2f + 0.5f;

            if (_length > MAX_LENGTH)
            {
                Console.WriteLine("Too long");
                _level.RemoveObject(this);
            }
            else
            {
                if (CheckIntersect())
                {
                    if (_hookHit)
                        ApplyVelocity();
                    _level.RemoveObject(this);
                }
                else
                {
                    CreateWorld();
                }
            }
        }

        private void ApplyVelocity()
        {
            var position = _origin.Position + new Vector3(0, 1, 0);
            
            var rotation = Matrix.CreateFromYawPitchRoll(_yaw - _origin.Yaw, _pitch, 0f);
            var endPoint = Vector3.Transform(new Vector3(0f, 1f, -_length), rotation) + _offset;

            var direction = (endPoint - position);
            direction.Normalize();

            var distance = Vector3.Distance(position, endPoint);

            var newVelocity = direction * (float)Math.Pow(distance, 2) * new Vector3(0.2f, 0.02f, 0.2f);
            Console.WriteLine(newVelocity);
            _origin.Velocity += newVelocity;
        }

        protected override void CreateWorld()
        {
            var startPoint = _origin.Position;
            var rotation = Matrix.CreateFromYawPitchRoll(_yaw, _pitch, 0f);
            var endPoint = Vector3.Transform(new Vector3(0f, 1f, -_length), rotation) + _offset;
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

        private bool CheckIntersect()
        {
            var position = _origin.Position + new Vector3(0, 1, 0);
            var rotation = Matrix.CreateFromYawPitchRoll(_yaw, _pitch, 0f);
            var endPoint = Vector3.Transform(new Vector3(0f, 1f, -_length), rotation) + _offset;

            var direction = (endPoint - position);
            direction.Normalize();
            var ropeLength = Vector3.Distance(endPoint, position);

            var collider = new RayCollider { Ray = new Ray(position, direction) };

            return _level.Objects.Any(o =>
            {
                if (!(o is Player))
                {
                    var intersectResult = collider.Intersects(o.Collider);
                    if (intersectResult.HasValue && Math.Abs(intersectResult.Value) <= ropeLength)
                    {
                        if (Math.Abs(intersectResult.Value - ropeLength) < SPEED)
                            _hookHit = true;
                        return true;
                    }
                }
                return false;
            });
        }

        protected override void CreateGeometry()
        {
            Geometry.AddVertices(CuboidComposer.Create(1f));
        }
    }
}
