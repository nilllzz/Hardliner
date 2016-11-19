using System;
using Microsoft.Xna.Framework;

namespace Hardliner.Engine.Collision
{
    public class BoxCollider : ICollider
    {
        public BoundingBox Box { get; set; }

        public bool Collide(ICollider collider)
        {
            if (collider is NoCollider)
                return false;
            
            if (collider is SphereCollider)
                return (collider as SphereCollider).Sphere.Intersects(Box);
            if (collider is BoxCollider)
                return (collider as BoxCollider).Box.Intersects(Box);

            throw new NotImplementedException();
        }
    }
}
