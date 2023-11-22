using System.Collections;
using TMPro;
using UnityEngine;

public class ChargeEnemy : MonoBehaviour
{
    //// defines time between attacks
    //[SerializeField] private float attackCooldown = 1.5f;
    //// will hold reference to attack coroutine
    //private Coroutine attackCoroutine;

    // reference to rb
    Rigidbody2D rb;
    // defines time between charges
    [SerializeField] private float chargeCooldown = 2.0f;
    // will hold reference to charge coroutine
    private Coroutine chargeCoroutine;
    // how long the player will have double damage form the start of the charge
    [SerializeField] private float chargeLength = 1.0f;

    //// if touching player
    //private bool inContact = false;

    // if enemy is dashing
    private bool isCharging = false;

    // holds reference to warning sign object
    private GameObject warning;
    
    // defines how forcefull the attack knockback is
    private float knockForce = 300f;


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

                StartCoroutine(Knockback(collision));
            }

            //// start coroutine for timed attack and hold reference in coroutine variable
            //attackCoroutine = StartCoroutine(TimedAttack(collision));
        }
    }


    // when overlap ends
    private void OnCollisionExit2D(Collision2D collision)
    {
        //if (attackCoroutine != null)
        //{
        //    // stop the coroutine form running
        //    StopCoroutine(attackCoroutine);
        //}

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

    //// apply damage at regular interval
    //IEnumerator TimedAttack(Collision2D collision)
    //{

    //    // get health script
    //    HealthScript script = collision.gameObject.GetComponent<HealthScript>();
    //    // if health script found
    //    if (script != null)
    //    {
    //        // keep looping while routine is active
    //        while (true)
    //        {
    //            // delay
    //            yield return new WaitForSeconds(attackCooldown);

    //            script.TakeDamage(10);

    //            Debug.Log("dealt 10 damage (repeat)");
    //        }
    //    }
    //    else
    //    {
    //        // otherwise, stop coroutine
    //        yield return null;
    //    }
    //}


    // regularly charge player while they are in the trigger range
    IEnumerator ChargePlayer()
    {
        // keep looping until routine is stopped
        while (true)
        {
            //// while not touching, wait charge time, charge, wait cooldown time
            //if (!inContact)
            //{
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
            //}
            //else
            //{
            //    // wait for contact to end without causing infinite loop
            //    yield return new WaitForSeconds(0.1f);
            //}
        }
    }

    IEnumerator Knockback(Collision2D collision)
    {
        SimpleMovement movementScript = collision.gameObject.GetComponent<SimpleMovement>();

        if (movementScript != null)
        {
            movementScript.SetKnocked(true);

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(transform.localScale.x * knockForce, 0, 0));

            yield return new WaitForSeconds(0.3f);

            movementScript.SetKnocked(false);
        }
    }
}