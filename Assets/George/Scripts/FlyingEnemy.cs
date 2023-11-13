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
            if(isRushing)
            {
                isRushing = false;
                StopCoroutine(rushCoroutine);
            }    

            // if not patrolling, start
            if(!isPatrolling)
            {
                StartCoroutine(StartPatrol());
            }
        }
    }

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

        while(isPatrolling)
        {
            // add velocity towards the next patrol point
            rb.velocity = patrolSpeed * Vector3.Normalize(currentPoint.position - transform.position);

            // if enemy close to next patrol point
            if (Mathf.Abs((currentPoint.position - transform.position).magnitude) <= 0.5f)
            {
                // switch patrol point
                if(currentPoint == pointA)
                {
                    currentPoint = pointB;
                }
                else
                {
                    currentPoint = pointA;
                }
                Flip();
            }

            yield return new WaitForFixedUpdate();
        }

        rb.velocity = new Vector3 (0, 0, 0);
        yield return null;
    }

    IEnumerator RushPlayer(Collider2D playerColl)
    {
        while(isRushing)
        {
            // make sure the enemy is facing towards the player
            if ((((transform.position - currentPoint.position).x) > 0 && (transform.position - playerColl.transform.position).x < 0) || ((transform.position - currentPoint.position).x) < 0 && (transform.position - playerColl.transform.position).x > 0)
            {
                Flip();
            }

            // wait for charge time
            yield return new WaitForSeconds(chargeTime);

            // used to record time passed
            float elapsedTime = 0.0f;

            // until the rush time has passed
            while (elapsedTime < rushTime)
            {
                elapsedTime += Time.deltaTime;

                // add velocity towards the player
                rb.velocity = rushSpeed * Vector3.Normalize(playerColl.transform.position - transform.position);

                yield return new WaitForEndOfFrame();
            }

            elapsedTime = 0.0f;

            // until cooldown time has been met
            while (elapsedTime < cooldown)
            {
                elapsedTime += Time.deltaTime;

                // give the enemy no velocity
                rb.velocity = new Vector3(0, 0, 0);

                yield return new WaitForEndOfFrame();
            }
        }

        yield return null;
    }


    // when enemy comes into contact with the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // find health script
        HealthScript healthScript = collision.gameObject.GetComponent<HealthScript>();

        // if found
        if(healthScript != null)
        {
            // apply damage
            healthScript.TakeDamage(damageAmount);
        }
    }
}
