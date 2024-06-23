using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : MonoBehaviour
{
    //public float detectionRange = 10f; // Range to detect the player
    public float chaseSpeed = 5f; // Speed during chase
    public float idleSpeed = 2f; // Speed while idle
    public float patrolDistance = 10f; // Distance to patrol from the starting position
    public float shieldCooldown = 5f; // Cooldown for shield activation
    public int maxHealth = 100;
    public int shieldHealth = 60;

    private int currentHealth;
    private bool isShieldActive = false;
    private float shieldCooldownTimer;
    private bool isChasing = false;
    private bool isPatrolling;

    private Transform player;
    // transforms for patrol points and variable hold current point enemy is moving to
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Transform currentPoint;

    [SerializeField] private float patrolSpeed = 2;
    private Rigidbody2D rb;
    private Coroutine activateShieldCoroutine;

   

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        //player = GameObject.FindGameObjectWithTag("Player").transform;

        Transform[] childrenTransforms = transform.parent.GetComponentsInChildren<Transform>();

        for (int i = 0; i < childrenTransforms.Length; i++)
        {
            
            if (childrenTransforms[i].gameObject.layer == LayerMask.NameToLayer("Enemies"))
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
        StartCoroutine(Patrol());
    }

    void Update()
    {
        // Check if player is within detection range
        //if (Vector3.Distance(transform.position, player.position) < detectionRange)
        //{
        //    if (!isChasing)
        //    {
        //        StartChase();
        //    }
        //}
        //else if (isChasing)
        //{
        //    StopChase();
        //}

        // Move around within the patrol area while idle
        //if (!isChasing)
        //{
        //    Patrol();
        //    // Flip the sprite based on movement direction
        //    FlipSprite(patrolTarget - transform.position);
        //}
        //else
        //{
        //    // Flip the sprite based on movement direction during chase
        //    FlipSprite(player.position - transform.position);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthScript script = collision.GetComponent<HealthScript>();

        if (script != null)
        {
            if (isPatrolling)
            {
                isPatrolling = false;

                if (!isChasing)
                {
                    isChasing = true;
                }
            }
        }
    }
    void StartChase()
    {
        Debug.Log("Chasing player!");
        isChasing = true;

        // Activate shield periodically during the chase
        if (!isShieldActive && Time.time > shieldCooldownTimer)
        {
            //ActivateShield();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // get health script
        HealthScript script = collision.gameObject.GetComponent<HealthScript>();
        // if health script found
        if (script != null)
        {
            if (activateShieldCoroutine != null)
            {
                StopCoroutine(activateShieldCoroutine);
                activateShieldCoroutine = null;
                isShieldActive = false;
            }
            // if not patrolling, start
            if (!isPatrolling)
            {
                StartCoroutine(Patrol());
            }
            
            // make sure enemy is facing player
            UpdateDirection(GetComponent<Collider2D>());

        }
    }

    void StopChase()
    {
        Debug.Log("Stopping chase!");
        isChasing = false;
    }

    //IEnumerator ActivateShield(Collider2D playerColl)
    //{
        
    //    Debug.Log("Shield activated!");
    //    isShieldActive = true;

    //    // Reset shield cooldown timer
    //    shieldCooldownTimer = Time.time + shieldCooldown;
    //}

    public void TakeDamage(int damage)
    {
        if (isShieldActive)
        {
            // If the shield is active, deduct damage from the shield
            shieldHealth -= damage;

            // If the shield is broken, deactivate it
            if (shieldHealth <= 0)
            {
                DeactivateShield();
            }
        }
        else
        {
            // If the shield is not active, deduct damage from the enemy's health
            currentHealth -= damage;

            // Check if the enemy is defeated
            if (currentHealth <= 0)
            {
                DefeatEnemy();
            }
        }
    }

    void DeactivateShield()
    {
        Debug.Log("Shield deactivated!");
        isShieldActive = false;
        shieldHealth = 60; // Reset shield health for the next use
    }

    private void UpdateDirection(Collider2D collider)
    {
        // if facing the wrong way
        if ((collider.transform.position.x > transform.position.x && transform.localScale.x < 0) || (collider.transform.position.x < transform.position.x && transform.localScale.x > 0))
        {
            Flip();
        }
    }

    void DefeatEnemy()
    {
        Debug.Log("Enemy defeated!");
        // Implement any actions you want to take when the enemy is defeated
        Destroy(gameObject);
    }

   

    IEnumerator Patrol()
    {
        isPatrolling = true;

        while (isPatrolling)
        {
            Debug.Log("Shield Enemy Patrolling");
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
    }

    void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        return;
    }
}
