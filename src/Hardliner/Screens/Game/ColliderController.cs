using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardliner.Engine.Collision;
using Microsoft.Xna.Framework;

namespace Hardliner.Screens.Game
{
    internal class ColliderController
    {
        private LevelObject _obj;
        private Level _level;
        private bool _updatedGroundY;
        private float _groundY;

        internal float GroundY
        {
            get
            {
                if (!_updatedGroundY)
                {
                    _groundY = GetGroundY();
                    _updatedGroundY = true;
                }

                return _groundY;
            }
        }
        internal bool HadCollisionX { get; private set; } = false;
        internal bool HadCollisionZ { get; private set; } = false;

        public ColliderController(LevelObject obj, Level level)
        {
            _obj = obj;
            _level = level;
        }

        internal void Update()
        {
            _updatedGroundY = false;
            HadCollisionX = false;
            HadCollisionZ = false;
        }

        internal Vector3 ChangePosition(Vector3 currentPosition, Vector3 velocity, Vector3 scale)
        {
            var newPosition = currentPosition + velocity;

            var xCollider = new BoxCollider(new BoundingBox(
                new Vector3(_obj.Collider.Left + velocity.X, _obj.Collider.Bottom, _obj.Collider.Front),
                new Vector3(_obj.Collider.Right + velocity.X, _obj.Collider.Top, _obj.Collider.Back)));
            var zCollider = new BoxCollider(new BoundingBox(
                new Vector3(_obj.Collider.Left, _obj.Collider.Bottom, _obj.Collider.Front + velocity.Z),
                new Vector3(_obj.Collider.Right, _obj.Collider.Top, _obj.Collider.Back + velocity.Z)));

            int i = 0;
            bool collisionX = false, collisionZ = false;
            float setX = 0f, setZ = 0f;

            while (i < _level.Objects.Count() && (!collisionX || !collisionZ))
            {
                var o = _level.Objects.ElementAt(i);

                if (!ReferenceEquals(o, _obj))
                {
                    if (!collisionX &&
                        o.Collider.Top != _groundY &&
                        (_obj.Collider.Right <= o.Collider.Left || _obj.Collider.Left >= o.Collider.Right) &&
                        o.Collider.Collides(xCollider))
                    {
                        collisionX = true;
                        if (_obj.Collider.Right <= o.Collider.Left)
                            setX = o.Collider.Left - scale.X / 2f;
                        else
                            setX = o.Collider.Right + scale.X / 2f;
                    }

                    if (!collisionZ &&
                        o.Collider.Top != _groundY &&
                        (_obj.Collider.Back <= o.Collider.Front || _obj.Collider.Front >= o.Collider.Back) &&
                        o.Collider.Collides(zCollider))
                    {
                        collisionZ = true;
                        if (_obj.Collider.Back <= o.Collider.Front)
                            setZ = o.Collider.Front - scale.Z / 2f;
                        else
                            setZ = o.Collider.Back + scale.Z / 2f;
                    }
                }
                
                i++;
            }

            if (collisionX)
            {
                newPosition.X = setX;
                HadCollisionX = true;
            }
            if (collisionZ)
            {
                newPosition.Z = setZ;
                HadCollisionZ = true;
            }

            return newPosition;
        }

        private float GetGroundY()
        {
            var ground = float.MinValue;

            foreach (var o in _level.Objects)
            {
                var colliderTop = o.Collider.Top;
                if (colliderTop > ground && colliderTop <= _obj.Collider.Bottom)
                {
                    var collider = new BoxCollider(new BoundingBox(
                        new Vector3(_obj.Collider.Left, colliderTop - 0.01f, _obj.Collider.Front),
                        new Vector3(_obj.Collider.Right, colliderTop + 0.01f, _obj.Collider.Back)));
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
