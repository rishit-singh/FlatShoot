using Unity.VisualScripting;
using UnityEngine;

public class Player : MortalEntity, IControllable 
{
    public Weapon CurrentWeapon;

    public Weapon[] Weapons;

    public float Health;

    [SerializeField]
    public GameObject Instance;

    public void Damage(int amount)
    {
        this.Health -= amount;
    }

    public void Move(Vector2 direction)
    {
        throw new System.NotImplementedException();
    }

    public void Attack(Vector2 range)
    {
        throw new System.NotImplementedException();
    }

    public Player(float health) : base(new DisposableGameObject(), health) 
    {
        this.Health = health;

        this.Obj = new DisposableGameObject(this.Instance, Time.time, -1);
    }
}