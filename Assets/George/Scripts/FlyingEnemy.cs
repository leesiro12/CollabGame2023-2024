using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    // transforms for patrol points and variable hold current point enemy is moving to
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    private Transform currentPoint;

    // holds reference to warning sign object
    private GameObject warning;
    // defines how forcefull the attack knockback is
    [SerializeField] private float knockForce = 300f;
    // defines the length of the knockback
    private float knockbackLength = 0.3f;

    // reference to rigidbody
    private Rigidbody2D rb;
    // reference to enemy movement speed while patrolling
    [SerializeField] private float patrolSpeed = 2;
    // time before rushing enemy
    [SerializeField] private float chargeTime = 0.5f;
    // reference to enemy movement speed while rushing the player
    [SerializeField] private float rushSpeed = 12;
    // how long the enemy will charge for
    [SerializeField] private float rushTime = 0.6f;
    // time between rushes
    [SerializeField] private float cooldown = 2;
    // record time passed since last rush
    private float elapsedTime;

    // the amount of damage the enemy will deal
    [SerializeField] private int damageAmount;

    // track if enemy is/should patrol
    private bool isPatrolling;
    // track if enemy is/should attack
    private bool isRushing;
    // hold reference to rushing coroutine
    private Coroutine rushCoroutine;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        warning = transform.GetChild(0).gameObject;

        currentPoint = pointA;

        // makes enemy start patrolling at beginning of game
        StartCoroutine(StartPatrol());
    }

    // something enters detection radius
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // get health script
        HealthScript script = collider.GetComponent<HealthScript>();
        // if health script found
        if (script != null)
        {
            // if currenty patrolling, stop
            if (isPatrolling)
            {
                isPatrolling = false;

                // if not currently rushing, start
                if (!isRushing)
                {
                    isRushing = true;
                    rushCoroutine = StartCoroutine(RushPlayer(collider));
                }
            }
        }
    }

    // something exits detection radius
    private void OnTriggerExit2D(Collider2D collider)
    {
        // get health script
        HealthScript script = collider.GetComponent<HealthScript>();
        // if health script found
        if (script != null)
        {
            // if rushing, stop
            if (isRushing)
            {
                isRushing = false;
                StopCoroutine(rushCoroutine);
                warning.SetActive(false);
            }

            // if not patrolling, start
            if (!isPatrolling)
            {
                StartCoroutine(StartPatrol());
            }
        }
    }

    // when enemy comes into contact with the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // find health script
        HealthScript healthScript = collision.gameObject.GetComponent<HealthScript>();

        // if found
        if (healthScript != null)
        {
            // apply damage
            healthScript.TakeDamage(damageAmount);

            // stop the current charge
            elapsedTime = rushTime;

            // run coroutine to apply knockback to player
            StartCoroutine(Knockback(collision));
        }
    }
<<<<<<< Updated upstream:Assets/George/Scripts/FlyingEnemy.cs
=======
   
>>>>>>> Stashed changes:Assets/George/Scripts/Enemies/FlyingEnemy.cs

    // change facing direction
    private void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        return;
    }

    IEnumerator StartPatrol()
    {
        // record that enemy is patrolling
        isPatrolling = true;

        while (isPatrolling)
        {
            // add velocity towards the next patrol point
            rb.velocity = patrolSpeed * Vector3.Normalize(currentPoint.position - transform.position);

            // if enemy close to next patrol point
            if (Mathf.Abs((currentPoint.position - transform.position).magnitude) <= 0.5f)
            {
                // switch patrol point
                if (currentPoint == pointA)
                {
                    currentPoint = pointB;
                }
                else
                {
                    currentPoint = pointA;
<<<<<<< Updated upstream:Assets/George/Scripts/FlyingEnemy.cs
=======

>>>>>>> Stashed changes:Assets/George/Scripts/Enemies/FlyingEnemy.cs
                }
                Flip();
            }

            yield return new WaitForFixedUpdate();
        }

        rb.velocity = new Vector3(0, 0, 0);
        yield return null;
    }

    IEnumerator RushPlayer(Collider2D playerColl)
    {
        while (isRushing)
        {
            // make sure the enemy is facing towards the player
            if ((playerColl.transform.position.x > transform.position.x && transform.localScale.x < 0) || (playerColl.transform.position.x < transform.position.x && transform.localScale.x > 0))
            {
                Flip();
            }

            warning.SetActive(true);

            // wait for charge time
            yield return new WaitForSeconds(chargeTime);

            warning.SetActive(false);

            // used to record time passed
            elapsedTime = 0.0f;

            // until the rush time has passed
            while (elapsedTime < rushTime)
            {
                elapsedTime += Time.deltaTime;

                // add velocity towards the player
                rb.velocity = rushSpeed * Vector3.Normalize(playerColl.transform.position - transform.position);

                // prevent issues with while loop
                yield return new WaitForEndOfFrame();
            }

            elapsedTime = 0.0f;

            // until cooldown time has been met
            while (elapsedTime < cooldown)
            {
                elapsedTime += Time.deltaTime;

                // give the enemy no velocity
                rb.velocity = new Vector3(0, 0, 0);

                // prevent issues with while loop
                yield return new WaitForEndOfFrame();
            }
        }

        yield return null;
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