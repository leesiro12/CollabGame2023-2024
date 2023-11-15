using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    // the length of time the projectiles should stay in the level
    private float lifespan = 1.6f;
    // how long the projectiles have been in the level
    private float elapsedTime = 0.0f;


    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        // when object has been in the level for the length of the lifespan
        if (elapsedTime > lifespan)
        {
            // remove from level
            Destroy(gameObject);
        }
    }

    // when an overlap occurs
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if the object has tag of Block
        if(collision.CompareTag("Block"))
        {
            // destroy self
            Destroy(gameObject);
        }
        
        // if the overlapped object has tag of Player and contains PlayerHealth script
        if (collision.CompareTag("Enemies") && collision.gameObject.GetComponent<EnemyHealth>() != null)
        {
            // apply damage
            collision.GetComponent<EnemyHealth>().TakeDamage(10);
            Debug.Log("damage taken");
        }
    }
}
