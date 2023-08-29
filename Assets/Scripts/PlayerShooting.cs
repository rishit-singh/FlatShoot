using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    // Get this info from gun type?
    [SerializeField]
    private float bulletSpeed = 20f;
    [SerializeField]
    private float bulletDamage = 10f;
    [SerializeField]
    private float shootingTime = 0.1f;
    [SerializeField]
    private int maxBullets = 10;

    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform shootingPos;

    private int currentBullets;
    private float lastShootingTime;

    void Start() {
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

    private void Shoot() {
        if (currentBullets > 0 && Time.time >=  lastShootingTime + shootingTime) {
            currentBullets--;
            lastShootingTime = Time.time;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            Vector3 direction = (mousePosition - shootingPos.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // TODO: will change to a pooling system later so im not instantiating+destroying so many objects >.>
            Bullet curBullet = Instantiate(bullet, shootingPos.position, Quaternion.Euler(0f, 0f, angle)).GetComponent<Bullet>();
            curBullet.Fire(bulletDamage, bulletSpeed, direction);

        } else if (currentBullets == 0) {
            // TODO
            Debug.Log("Press R to reload.");
        }
    }

    private void Reload() {
        currentBullets = maxBullets;
    }
}
