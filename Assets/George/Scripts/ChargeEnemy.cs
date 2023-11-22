using System.Collections;
using TMPro;
using UnityEngine;

public class ChargeEnemy : MonoBehaviour
{
    // reference to rb
    Rigidbody2D rb;
    // defines time between charges
    [SerializeField] private float chargeCooldown = 2.0f;
    // will hold reference to charge coroutine
    private Coroutine chargeCoroutine;
    // how long the player will have double damage form the start of the charge
    [SerializeField] private float chargeLength = 1.0f;

    // if enemy is dashing
    private bool isCharging = false;

    // holds reference to warning sign object
    private GameObject warning;
    
    // defines how forcefull the attack knockback is
    private float knockForce = 300f;
    // defines the length of the knockback
    private float knockbackLength = 0.3f;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        warning = transform.GetChild(0).gameObject;
    }



    private void OnTriggerEnter2D(Collider2D collider)
    {
        // get health script
        HealthScript script = collider.GetComponent<HealthScript>();

        // if health script found
        if (script != null)
        {
            // start charging the enemy
            chargeCoroutine = StartCoroutine(ChargePlayer());
            // make sure enemy is facing player
            UpdateDirection(collider);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        // get health script
        HealthScript script = collider.GetComponent<HealthScript>();

        // if health script found
        if (script != null)
        {
            // make sure enemy is facing player
            UpdateDirection(collider);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (chargeCoroutine != null)
        {
            // start charging the player, at intervals
            StopCoroutine(chargeCoroutine);
        }

        //inContact = false;
        warning.SetActive(false);
    }


    // when overlap begins
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // get health script
        HealthScript script = collision.gameObject.GetComponent<HealthScript>();
        // if health script found
        if (script != null)
        {
            //inContact = true;

            if (isCharging)
            {
                // apply double damage
                script.TakeDamage(20);

                // run coroutine to apply knockback to player
                StartCoroutine(Knockback(collision));
            }
        }
    }

    // when overlap ends
    private void OnCollisionExit2D(Collision2D collision)
    {
        //inContact = false;
        warning.SetActive(false);
    }


    private void UpdateDirection(Collider2D collider)
    {
        // if facing the wrong way
        if ((collider.transform.position.x > transform.position.x && transform.localScale.x < 0) || (collider.transform.position.x < transform.position.x && transform.localScale.x > 0))
        {
            // flip x scale
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }


    // regularly charge player while they are in the trigger range
    IEnumerator ChargePlayer()
    {
        // keep looping until routine is stopped
        while (true)
        {
            // wait, apply warning, wait
            yield return new WaitForSeconds(0.3f);
            warning.SetActive(true);
            yield return new WaitForSeconds(0.7f);

            // start charge
            rb.velocity = new Vector2(transform.localScale.x * 10, 0f);

            // remove warning
            warning.SetActive(false);

            // make isCharging true for a short time
            isCharging = true;
            yield return new WaitForSeconds(chargeLength);
            isCharging = false;

            yield return new WaitForSeconds(chargeCooldown - chargeLength);
        }
    }

    // apply knockback effect to hit player
    IEnumerator Knockback(Collision2D collision)
    {
        // attempt to save ref to the player's movement script
        SimpleMovement movementScript = collision.gameObject.GetComponent<SimpleMovement>();

        // if script found
        if (movementScript != null)
        {
            // set marker in script to true, indicating the player is being knocked back
            movementScript.SetKnocked(true);

            // apply froce to player, creating knockback effect
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(transform.localScale.x * knockForce, 0, 0));

            // wait for length of the knockback
            yield return new WaitForSeconds(knockbackLength);

            // set marker in script to false, indicating the player is no longer being knocked back
            movementScript.SetKnocked(false);
        }
    }
}