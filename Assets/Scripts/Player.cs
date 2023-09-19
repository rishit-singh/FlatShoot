using System;
using UnityEngine;

public class Player : MortalEntity, IControllable 
{
    public Weapon CurrentWeapon;

    public Weapon[] Weapons;

    public float Health;

    [SerializeField]
    public GameObject Instance;

    [SerializeField]
    public GameObject WeaponInstance;

    private Rigidbody2D RigidBody;

    public void Damage(int amount)
    {
        this.Health -= amount;
        
        if (this.Health < 0)
            this.Die();
    }

    public void Move(Vector2 direction)
    {
        this.RigidBody.AddForce(direction);   
    } 

    public void Attack(Vector2 range)
    {
        throw new NotImplementedException();
    }

    public override void Die()
    {
        UnityEngine.Object.Destroy(this.Obj);
    }

    public Player(GameObject instance, float health) : base(instance, health) 
    {
        this.Health = health;

        Rigidbody2D rigidBody = instance.GetComponent<Rigidbody2D>();

        if (rigidBody == null)
            throw new System.Exception("Player object must have a Rigidbody2D component"); 
    
        this.RigidBody = rigidBody; 
    }
}