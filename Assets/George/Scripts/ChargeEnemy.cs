using DG.Tweening;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChargeEnemy : MonoBehaviour
{
    // reference to rb
    Rigidbody2D rb;

    // defines time between charges
    [SerializeField] private float chargeCooldown = 2.5f;
    // will hold reference to charge coroutine
    private Coroutine chargeCoroutine = null;
    // how long the player will have double damage form the start of the charge
    [SerializeField] private float chargeLength = 1.0f;
    // determines the force with which the enemy will charge
    [SerializeField] private float chargeStrength = 80.0f;

    // if enemy is dashing
    private bool isCharging = false;

    // holds reference to warning sign object
    private GameObject warning;

    // transforms for patrol points and variable hold current point enemy is moving to
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Transform currentPoint;
    // track if enemy is/should patrol
    private bool isPatrolling;
    // reference to enemy movement speed while patrolling
    [SerializeField] private float patrolSpeed = 2;

    // defines how forceful the attack knockback is
    private float knockForce = 300f;
    // defines the length of the knockback
    private float knockbackLength = 0.3f;    


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Transform[] childrenTransforms = transform.parent.GetComponentsInChildren<Transform>();

        // find the warning UI object and save to warning variable
        // also find and save patrol points
        for (int i = 0; i < childrenTransforms.Length; i++)
        {
            if (childrenTransforms[i].gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                warning = childrenTransforms[i].gameObject;
                warning.SetActive(false);
            }
            else if (childrenTransforms[i].gameObject.layer != LayerMask.NameToLayer("Enemies"))
            {
                if (pointA == null)
                {
                    pointA = childrenTransforms[i];
                }
                else if (pointB == null)
                {
                    pointB = childrenTransforms[i];
                }
            }
        }

        currentPoint = pointA;

        // makes enemy start patrolling at beginning of game
        StartCoroutine(StartPatrol());
    }
<<<<<<< Updated upstream:Assets/George/Scripts/ChargeEnemy.cs



=======
 
>>>>>>> Stashed changes:Assets/George/Scripts/Enemies/ChargeEnemy.cs
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // get health script
        HealthScript script = collider.GetComponent<HealthScript>();

        // if health script found
        if (script != null)
        {
            // stop patrolling
            if(isPatrolling)
            {
                isPatrolling = false;
            }

            if (chargeCoroutine == null)
            {
                // start charging the enemy
                chargeCoroutine = StartCoroutine(ChargePlayer());
            }
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
        // get health script
        HealthScript script = collision.gameObject.GetComponent<HealthScript>();
        // if health script found
        if (script != null)
        {
            if (chargeCoroutine != null)
            {
                // start charging the player, at intervals
                StopCoroutine(chargeCoroutine);
                chargeCoroutine = null;
                isCharging = false;
            }

            if (warning)
            {
                warning.SetActive(false);
            }

            // if not patrolling, start
            if (!isPatrolling)
            {
                StartCoroutine(StartPatrol());
            }
        }
    }


    // when overlap begins
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // get health script
        HealthScript script = collision.gameObject.GetComponent<HealthScript>();
        // if health script found
        if (script != null)
        {
            if (isCharging)
            {
                // apply damage
                script.TakeDamage(1);

                // run coroutine to apply knockback to player
                StartCoroutine(Knockback(collision));
            }
        }
    }

    // when overlap ends
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(warning)
        {
            warning.SetActive(false);
        }
    }


    private void UpdateDirection(Collider2D collider)
    {
        // if facing the wrong way
        if ((collider.transform.position.x > transform.position.x && transform.localScale.x < 0) || (collider.transform.position.x < transform.position.x && transform.localScale.x > 0))
        {
            Flip();
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
            if (warning)
            {
                warning.SetActive(true);
            }
            yield return new WaitForSeconds(0.7f);

            // start charge
            rb.AddForce(new Vector2(transform.localScale.x * chargeStrength, 0));

            //// make sure the enemy is facing towards the player
            //if (rb.velocity.x > 0.0f && transform.localScale.x < 0 || rb.velocity.x < 0.0f && transform.localScale.x > 0)
            //{
            //    Flip();
            //}

            // remove warning
            if (warning)
            {
                warning.SetActive(false);
            }

            // make isCharging true for a short time
            isCharging = true;
            yield return new WaitForSeconds(chargeLength);
            isCharging = false;

            rb.velocity = new Vector3(0, 0, 0);

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

            // apply force to player, creating knockback effect
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(transform.localScale.x * knockForce, 0, 0));

            // wait for length of the knockback
            yield return new WaitForSeconds(knockbackLength);

            // set marker in script to false, indicating the player is no longer being knocked back
            movementScript.SetKnocked(false);
        }
    }

    IEnumerator StartPatrol()
    {
        // record that enemy is patrolling
        isPatrolling = true;

        while (isPatrolling)
        {
            // move towards next point
            if ((currentPoint.position - transform.position).x < 0.0f)
            {
                rb.velocity = new Vector2(-1 * patrolSpeed, rb.velocity.y);

            }
            else
            {
                rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);

            }
            
            // if enemy has reached patrol point, change point
            if (Mathf.Abs(currentPoint.position.x - transform.position.x) < 0.5f && currentPoint == pointA)
            {
                currentPoint = pointB;
                Flip();
            }
            else if (Mathf.Abs(currentPoint.position.x - transform.position.x) < 0.5f && currentPoint == pointB)
            {
                currentPoint = pointA;
                Flip();
            }

            yield return new WaitForFixedUpdate();
        }

        rb.velocity = new Vector3(0, 0, 0);
        yield return null;
    }

    // change facing direction
    private void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        return;
    }
}