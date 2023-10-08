using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int health = 100;

    public event EventHandler UnitDied;
    public event EventHandler Damaged;

    private int healthMax;

    private void Awake()
    {
        healthMax = health;
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health < 0) health = 0;

        OnDamaged();

        if (health == 0)
        {
            Die();
        }

        Debug.Log($"Current Health {health}");
    }

    public float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }

    private void Die()
    {
        OnUnitDied();
    }

    protected virtual void OnUnitDied()
    {
        UnitDied?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnDamaged()
    {
        Damaged?.Invoke(this, EventArgs.Empty);
    }
}
