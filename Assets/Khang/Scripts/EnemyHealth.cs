using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] public int currentHealth;
    [SerializeField] int shieldHealth = 250;
    [SerializeField] int currentShieldHealth;

    [SerializeField] private bool isShielding = false;

    void Start()
    {
        currentHealth = maxHealth;
        currentShieldHealth = shieldHealth;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    protected virtual void Death()
    {
        Destroy(this.gameObject);
    }

    public void TakeDamage(int damage)
    {
        if (!isShielding)
        {
            currentHealth -= damage;
        }
    }

    public void addShield()
    {
        currentHealth += shieldHealth;
    }

    public void shieldDamage(int value)
    {
        currentShieldHealth -= value;
    }

    public void shieldOff()
    {
        currentHealth -= currentShieldHealth;
    }

    public void setIsShielding(bool newValue)
    {
        isShielding = newValue;
    }
}
