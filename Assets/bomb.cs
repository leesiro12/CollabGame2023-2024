using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public float explosionDelay = 3.0f;  // Time in seconds before the bomb explodes
    public float explosionRadius = 5.0f; // Radius of the explosion
    public float explosionForce = 700000f;  // Force of the explosion

    public GameObject explosionEffect;   // Prefab of the explosion effect

    private bool hasExploded = false;

    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        StartCoroutine(ExplodeAfterDelay(explosionDelay));
    }

    IEnumerator ExplodeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Explode();
    }

    void Explode()
    {
        Debug.Log("Explosion triggered");
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

            if (nearbyObject.gameObject.GetComponent<HealthScript>())
            {
                Vector2 direction = nearbyObject.transform.position - transform.position;
                Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
                //rb.velocity = new Vector2(direction.x * explosionDelay, direction.y * explosionDelay);
                //rb.AddForce(direction * explosionForce, ForceMode2D.Impulse);
                rb.AddForce( new Vector2 (direction.y, 1) * explosionForce, ForceMode2D.Impulse);
            }
            //// Add force to objects with Rigidbody2D
            //Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
            //if (rb != null)
            //{
            //    Debug.Log(rb.gameObject);
            //    Vector2 direction = nearbyObject.transform.position - transform.position;
            //    rb.AddForce(direction * explosionForce, ForceMode2D.Impulse);
                
            //}
        }

        // Destroy the bomb object
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
