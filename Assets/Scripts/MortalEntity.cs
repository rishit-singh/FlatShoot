
using System;
using UnityEngine;


public abstract class MortalEntity : DisposableGameObject, IDamageable
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

    public MortalEntity(GameObject instance, float health, float autoDisposeTime = -1) : base(instance, Time.time, autoDisposeTime)
    {
        this.Health = health;
    }    
}