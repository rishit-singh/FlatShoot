using Unity.VisualScripting;
using UnityEngine;

public class Player : MortalEntity, IControllable 
{
    public Weapon CurrentWeapon;

    public Weapon[] Weapons;

    public float Health;

    [SerializeField]
    public GameObject Instance; 

    private Rigidbody RigidBody;

    public void Damage(int amount)
    {
        this.Health -= amount;
    }

    public void Move(Vector2 direction)
    {
        this.RigidBody.AddForce(direction);   
    } 


    public void Attack(Vector2 range)
    {
        throw new System.NotImplementedException();
    }

    public Player(float health) : base(null, health) 
    {
        this.Obj = this.Instance; 
        this.Health = health;
    }
}