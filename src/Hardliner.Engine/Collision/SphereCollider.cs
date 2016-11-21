using System;
using Microsoft.Xna.Framework;

namespace Hardliner.Engine.Collision
{
    public class SphereCollider : ICollider
    {
        public float Top => Sphere.Center.Y + Sphere.Radius;
        public float Bottom => Sphere.Center.Y - Sphere.Radius;
        public BoundingSphere Sphere { get; set; }
        
        public bool Collides(ICollider collider)
        {
            if (collider is NoCollider)
                return false;

            if (collider is SphereCollider)
                return (collider as SphereCollider).Sphere.Intersects(Sphere);
            if (collider is BoxCollider)
                return (collider as BoxCollider).Box.Intersects(Sphere);
            if (collider is RayCollider)
                return (collider as RayCollider).Ray.Intersects(Sphere).HasValue;

            throw new NotImplementedException();
        }
    }
}
