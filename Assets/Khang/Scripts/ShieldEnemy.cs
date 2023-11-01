using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : MonoBehaviour
{
    //movement related
    [SerializeField] private float moveSpeed = 15.0f;

    //health related variables
    [SerializeField] private int maxHealth = 75;
    [SerializeField] private int currentHealth;
    private int miscHealth;

    //shield function vars
    private float shieldCooldown = 30.0f;
    private int shieldHealth = 30;
    private float shieldDuration = 6.0f;
    private bool shield;

    //other
    private Rigidbody2D rb;

    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        miscHealth = currentHealth;
        if (currentHealth <= 0)
        {
            GameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //player loses health script here
        ///////////////////////////
    }


    private IEnumerator ShieldUp()
    {
        if (!shield)
        {
            shield = true;
            currentHealth = currentHealth + shieldHealth;
            yield return new WaitForSeconds(shieldDuration);
            currentHealth = -shieldHealth;
        }
    }
}
