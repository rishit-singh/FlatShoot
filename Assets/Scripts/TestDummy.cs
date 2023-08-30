using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDummy : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float maxHealth = 50;

    private float currentHealth;

    public void Damage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0) {
            Die();
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Die() {
        Destroy(gameObject);
    }

    
}
