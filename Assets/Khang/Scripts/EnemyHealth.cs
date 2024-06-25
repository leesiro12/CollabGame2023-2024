using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
<<<<<<< Updated upstream
    [SerializeField] int currentHealth;
    [SerializeField] int shieldHealth = 250;
=======
    [SerializeField] public int currentHealth;
    [SerializeField] int shieldHealth;
>>>>>>> Stashed changes
    [SerializeField] int currentShieldHealth;

    void Start()
    {
        currentHealth = maxHealth;
        currentShieldHealth = shieldHealth;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }

        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
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

}
