namespace Hardliner.Engine.Collision
{
    public interface ICollider
    {
        bool Collide(ICollider collider);
    }
}
