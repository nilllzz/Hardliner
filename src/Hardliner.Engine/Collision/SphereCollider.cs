using System;
using Microsoft.Xna.Framework;

namespace Hardliner.Engine.Collision
{
    public class SphereCollider : ICollider
    {
        public BoundingSphere Sphere { get; set; }
        
        public bool Collide(ICollider collider)
        {
            if (collider is NoCollider)
                return false;

            if (collider is SphereCollider)
                return (collider as SphereCollider).Sphere.Intersects(Sphere);
            if (collider is BoxCollider)
                return (collider as BoxCollider).Box.Intersects(Sphere);

            throw new NotImplementedException();
        }
    }
}
