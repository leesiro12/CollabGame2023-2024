using System.Collections;
using UnityEngine;

public class ChargeEnemy : MonoBehaviour
{
    // defines time between attacks
    private float attackCooldown = 1.5f;
    // will hold reference to attack coroutine
    private Coroutine attackCoroutine;

    // reference to rb
    Rigidbody2D rb;
    // set speed threshold for double damage
    private float speedThreshold = 3.0f;
    // defines time between charges
    private float chargeCooldown = 2.0f;
    // will hold reference to charge coroutine
    private Coroutine chargeCoroutine;

    // if touching player
    private bool inContact = false;

    private GameObject warning;


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
        if(chargeCoroutine != null)
        {
            StopCoroutine(chargeCoroutine);
        }

        inContact = false;
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
            inContact = true;

            if (rb.velocity.x >= speedThreshold || rb.velocity.x <= -speedThreshold)
            {
                // apply double damage
                script.TakeDamage(20);
                Debug.Log("dealt 20 damage");
            }

            else
            {
                // apply damage
                script.TakeDamage(10);
                Debug.Log("dealt 10 damage");
            }

            // start coroutine for timed attack and hold reference in coroutine variable
            attackCoroutine = StartCoroutine(TimedAttack(collision));
        }
    }


    // when overlap ends
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (attackCoroutine != null)
        { 
            // stop the coroutine form running
            StopCoroutine(attackCoroutine);
        }

        inContact = false;
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

    // apply damage at regular interval
    IEnumerator TimedAttack(Collision2D collision)
    {

        // get health script
        HealthScript script = collision.gameObject.GetComponent<HealthScript>();
        // if health script found
        if (script != null)
        {
            // keep looping while routine is active
            while (true)
            {
                // delay
                yield return new WaitForSeconds(attackCooldown);

                script.TakeDamage(10);

                Debug.Log("dealt 10 damage (repeat)");
            }
        }
        else
        {
            // otherwise, stop coroutine
            yield return null;
        }
    }


    // regularly charge player while they are in the trigger range
    IEnumerator ChargePlayer()
    {
        // keep looping until routine is stopped
        while(true)
        {
            // while not touching, wait charge time, charge, wait cooldown time
            if(!inContact)
            {
                yield return new WaitForSeconds(0.7f);
                warning.SetActive(true);
                yield return new WaitForSeconds(0.3f);

                rb.velocity = new Vector2(transform.localScale.x * 10, 0f);

                warning.SetActive(false);
                yield return new WaitForSeconds(chargeCooldown);
            }
            else
            {
                // wait for contact to end without causing infinite loop
                yield return new WaitForSeconds(0.1f);
            }
        }        
    }
}
