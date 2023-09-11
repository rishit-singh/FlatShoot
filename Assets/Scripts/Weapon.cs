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

public class Weapon : IShootable
{   
    public int Damage;

    public void Shoot()
    {
    }

    public void Aim(Vector2 location)
    {
    }

    public Weapon(int damage, int ammo)
    {
        this.Damage = damage;
    }
}




