using System.Collections.Generic;
using System.Linq;

namespace Hardliner.Engine.Collision
{
    public class MultiCollider : ICollider
    {
        private List<ICollider> _colliders;

        public float Bottom => _colliders.Min(c => c.Bottom);
        public float Top => _colliders.Max(c => c.Bottom);
        public float Left => _colliders.Min(c => c.Left);
        public float Right => _colliders.Max(c => c.Right);
        public float Front => _colliders.Min(c => c.Front);
        public float Back => _colliders.Max(c => c.Back);

        public MultiCollider(IEnumerable<ICollider> colliders)
        {
            _colliders = colliders.ToList();
        }

        public void AddCollider(ICollider collider)
        {
            _colliders.Add(collider);
        }

        public void ClearColliders()
        {
            _colliders.Clear();
        }

        public bool Collides(ICollider collider)
        {
            return _colliders.Any(c => c.Collides(collider));
        }

        internal ICollider[] GetColliders() => _colliders.ToArray();
    }
}
