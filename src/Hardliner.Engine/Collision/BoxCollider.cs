using System;
using Microsoft.Xna.Framework;

namespace Hardliner.Engine.Collision
{
    public class BoxCollider : ICollider
    {
        public BoundingBox Box { get; set; }
        public float Top => Box.Max.Y;
        public float Bottom => Box.Min.Y;

        public BoxCollider() { }

        public BoxCollider(BoundingBox box)
        {
            Box = box;
        }

        public BoxCollider(Vector3 position, Vector3 size)
        {
            Box = new BoundingBox(position - size / 2f, position + size / 2f);
        }

        public bool Collides(ICollider collider)
        {
            if (collider is NoCollider)
                return false;
            
            if (collider is SphereCollider)
                return (collider as SphereCollider).Sphere.Intersects(Box);
            if (collider is BoxCollider)
                return (collider as BoxCollider).Box.Intersects(Box);
            if (collider is RayCollider)
                return (collider as RayCollider).Ray.Intersects(Box).HasValue;

            throw new NotImplementedException();
        }
    }
}
