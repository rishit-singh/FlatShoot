
using UnityEngine;

public interface IShootable
{
    void Shoot();
    void Aim(Vector2);    
}

public interface IReloadable
{
    void Reload();
}

public class Weapon : IShootable, IReloadable
{   
    public int Damage;

    void Shoot()
    {
    }

    void Aim(Vector2 aim)
    {
    }

    void Reload()
    {
    }

    public Weapon(int damage)
    {
        this.Damage = damage;
    }
}


