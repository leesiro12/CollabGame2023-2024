using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public float explosionDelay = 3.0f;  // Time in seconds before the bomb explodes
    public float explosionRadius = 5.0f; // Radius of the explosion
    public float explosionForce = 700f;  // Force of the explosion
    private bool startCountDown = false;
    public GameObject explosionEffect;   // Prefab of the explosion effect

    private bool hasExploded = false;

    
    private void Awake()
    {
        GetComponent<Rigidbody2D>();
        StartCoroutine(ExplodeAfterDelay());       
        
    }
    IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }

    void Explode()
    {
        Debug.Log("explode check");
        if (hasExploded) return;

        hasExploded = true;

        // Show explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
            
        }

        // Detect objects within the explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D nearbyObject in colliders)
        {
            // Add force to objects with Rigidbody2D
            Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = nearbyObject.transform.position - transform.position;
                rb.AddForce(direction.normalized * explosionForce);
            }

            // Check if object has a script that handles taking damage
            //Damageable damageable = nearbyObject.GetComponent<Damageable>();
            //if (damageable != null)
            //{
            //    damageable.TakeDamage(50); // Example damage value
            //}
        }

        // Destroy the bomb object
        //Destroy(explosionEffect);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            this.transform.SetParent(collision.transform);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
