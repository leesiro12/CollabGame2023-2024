using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int playerHealth;


    void Start()
    {
        playerHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log(damage);
        playerHealth -= damage;
    }

    public void Heal(int amount)
    {
        playerHealth += amount;
    }
}
