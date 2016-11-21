namespace Hardliner.Engine.Collision
{
    public interface ICollider
    {
        bool Collides(ICollider collider);
        float Top { get; }
        float Bottom { get; }
    }
}
