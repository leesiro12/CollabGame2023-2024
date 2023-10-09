using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int maxHealth;
    [SerializeField] private int playerHealth;


    void Start()
    {
        maxHealth = 100;
        playerHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        playerHealth =- damage;
    }
}
