using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    // Get this info from gun type?
    [SerializeField]
    private float BulletSpeed = 20f;

    [SerializeField]
    private float BulletDamage = 10f;

    [SerializeField]
    private float ShootingTime = 0.1f;

    [SerializeField]
    private int MaxBullets = 10;

    [SerializeField]
    private GameObject BulletInstance;

    [SerializeField]
    private Transform shootingPos;

    private int CurrentBullets;
    private float LastShootingTime;

    void Start() 
    {
        Reload();
    }

    void Update()
    {
        // TODO: will change input system later

        // add hold to keep firing in the case of something like a machine gun?
        if (Input.GetMouseButtonDown(0)) {
            Shoot();
        }

        if (Input.GetKeyDown("r")) {
            Reload();
        }
    }

    private void Shoot() 
    {
        if (CurrentBullets > 0 && Time.time >= (LastShootingTime + ShootingTime)) {
            CurrentBullets--;
            LastShootingTime = Time.time;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            
            Vector3 direction = (mousePosition - shootingPos.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // TODO: will change to a pooling system later so im not instantiating+destroying so many objects >.>
            Bullet curBullet = Instantiate(BulletInstance, shootingPos.position, Quaternion.Euler(0f, 0f, angle)).GetComponent<Bullet>();
            curBullet.Fire(BulletDamage, BulletSpeed, direction);
        } else if (CurrentBullets == 0) 
            Debug.Log("Press R to reload."); // TODO
    }

    private void Reload() 
    {
        CurrentBullets = MaxBullets;
    }
}
 
