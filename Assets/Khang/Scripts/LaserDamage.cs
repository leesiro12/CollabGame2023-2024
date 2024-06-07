using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    //vars
    [SerializeField] private int damage;
    public HealthScript healthScript;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            healthScript.TakeDamage(damage);
        }
    }
}
