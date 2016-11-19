namespace Hardliner.Engine.Collision
{
    public class NoCollider : ICollider
    {
        public bool Collide(ICollider collider) => false;
    }
}
