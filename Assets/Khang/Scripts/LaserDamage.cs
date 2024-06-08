using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    //vars
    private int damage = 1;
    private HealthScript healthScript;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            healthScript = collider.gameObject.GetComponent<HealthScript>();
            if (healthScript != null )
            {
                healthScript.TakeDamage(damage);
            }
        }
    }
}