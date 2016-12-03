namespace Hardliner.Engine.Collision
{
    public class NoCollider : ICollider
    {
        public float Top => 0;
        public float Bottom => 0;
        public float Left => 0;
        public float Right => 0;
        public float Front => 0;
        public float Back => 0;
        public bool Collides(ICollider collider) => false;
    }
}
