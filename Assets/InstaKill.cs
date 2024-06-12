using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstaKill : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthScript playerHealthScript = collision.gameObject.GetComponent<HealthScript>();
            if (playerHealthScript != null)
            {
                playerHealthScript.TakeDamage(5);
            }
        }
    }
}
