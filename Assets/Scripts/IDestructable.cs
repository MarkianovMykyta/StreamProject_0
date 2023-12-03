using System;

namespace DefaultNamespace
{
    public interface IDestructable
    {
        public event Action<IDestructable> OnHealthChanged;  
        
        bool IsAlive { get; }
        int Health { get; }
        int MaxHealth { get; }
        void ApplyDamage(int damage);
    }
}