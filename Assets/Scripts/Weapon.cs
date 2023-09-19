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

    protected GameObject WeaponInstance;

    public void Shoot()
    {
    } 

    public void Aim(Vector2 range)
    {
    }

    public Weapon(int damage, int ammo, GameObject weaponInstance, GameObject target)
    {
        this.Damage = damage;
    }
}




