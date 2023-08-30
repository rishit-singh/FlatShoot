using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDummy : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float MaxHealth = 50;

    private float CurrentHealth;

    public void Damage(float amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
            Die();
    }

    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    private void Die() {
        Destroy(gameObject);
    }
}
