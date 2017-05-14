namespace Hardliner.Engine.Collision
{
    public class NoCollider : ICollider
    {
        private static NoCollider _instance;
        public static NoCollider Instance
            => _instance ?? (_instance = new NoCollider());

        public float Top => 0;
        public float Bottom => 0;
        public float Left => 0;
        public float Right => 0;
        public float Front => 0;
        public float Back => 0;
        public bool Collides(ICollider collider) => false;
    }
}
