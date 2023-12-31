using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float MaxTime = 10f;

    private float BulletDamage;
    private Rigidbody2D RB;

    void OnEnable() {
        RB = GetComponent<Rigidbody2D>();
    }

    public void Fire(float bulletDamage, float speed, Vector3 direction) {
        this.BulletDamage = bulletDamage;
        RB.velocity = speed * direction;
      
        Destroy(gameObject, MaxTime); // TODO: pooling system 
    }

    void OnTriggerEnter2D(Collider2D other) {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

        if (damageable != null) {
            damageable.Damage(BulletDamage);
            Destroy(gameObject); // TODO: pooling system 
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            Destroy(gameObject); // TODO: pooling system 
        }
    }
}
