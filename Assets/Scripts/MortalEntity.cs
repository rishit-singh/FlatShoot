
using System;

public abstract class MortalEntity : PoolObject, IDamageable
{ 
    public float Health { get; private set; }

    public virtual void Die()
    {
        throw new NotImplementedException();
    }

    public void Damage(float amount)
    {
        this.Health -= amount; 
    }

    public void AddHealth(float amount)
    {
        this.Health += amount;
    }

    public MortalEntity(DisposableGameObject obj, float health) : base(obj)
    {
        this.Health = health;
    }    
}