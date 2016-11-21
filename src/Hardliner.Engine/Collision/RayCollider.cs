using System;
using Microsoft.Xna.Framework;

namespace Hardliner.Engine.Collision
{
    public class RayCollider : ICollider
    {
        public float Top => 0;
        public float Bottom => 0;
        public Ray Ray { get; set; }

        public bool Collides(ICollider collider)
        {
            return Intersects(collider).HasValue;
        }

        public float? Intersects(ICollider collider)
        {
            if (collider is NoCollider || collider is RayCollider)
                return null;

            if (collider is SphereCollider)
                return Ray.Intersects((collider as SphereCollider).Sphere);
            if (collider is BoxCollider)
                return Ray.Intersects((collider as BoxCollider).Box);

            throw new NotImplementedException();
        }
    }
}
