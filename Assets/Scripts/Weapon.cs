using UnityEngine;

public interface IShootable
{
    void Shoot();
    void Aim(Vector2 location);    
}

public interface IReloadable
{
    void Reload(int amount);
}

public class Weapon : IShootable, IReloadable
{   
    public int Damage;

    public int Shots { get; private set; } 

    public void Shoot()
    {
    }

    public void Aim(Vector2 location)
    {
    }

    public void Reload(int amount)
    {
    }

    public Weapon(int damage)
    {
        this.Damage = damage;
    }
}




