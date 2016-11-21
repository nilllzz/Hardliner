namespace Hardliner.Engine.Collision
{
    public class NoCollider : ICollider
    {
        public float Top => 0;
        public float Bottom => 0;
        public bool Collides(ICollider collider) => false;
    }
}
