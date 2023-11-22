namespace DefaultNamespace
{
    public interface IDestructable
    {
        bool IsAlive { get; }
        int Health { get; }
        void Attack(int damage);
    }
}