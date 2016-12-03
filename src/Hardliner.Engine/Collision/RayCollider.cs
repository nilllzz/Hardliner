using System;
using Microsoft.Xna.Framework;

namespace Hardliner.Engine.Collision
{
    public class RayCollider : ICollider
    {
        public float Top => 0;
        public float Bottom => 0;
        public float Left => 0;
        public float Right => 0;
        public float Front => 0;
        public float Back => 0;
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
            if (collider is MultiCollider)
                return MultiColliderHandler(collider as MultiCollider);

            throw new NotImplementedException();
        }

        private float? MultiColliderHandler(MultiCollider collider)
        {
            float? result = null;
            int index = 0;
            var colliders = collider.GetColliders();

            while (index < colliders.Length && result == null)
            {
                result = Intersects(colliders[index]);
                index++;
            }

            return result;
        }
    }
}
