
using UnityEngine;

public interface IShootable
{
    void Shoot();
    void Aim(Vector2 location);    
}

public interface IReloadable
{
    void Reload();
}

public class Weapon : IShootable, IReloadable
{   
    public int Damage;

    public void Shoot()
    {
    }

    public void Aim(Vector2 location)
    {
    }

    public void Reload()
    {
    }

    public Weapon(int damage)
    {
        this.Damage = damage;
    }
}




