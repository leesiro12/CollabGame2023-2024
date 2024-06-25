using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : MonoBehaviour
{
    public float detectionRange = 10f; // Range to detect the player
<<<<<<< Updated upstream
    public float chaseSpeed = 5f; // Speed during chase
    public float idleSpeed = 2f; // Speed while idle
    public float patrolDistance = 10f; // Distance to patrol from the starting position
    public float shieldCooldown = 5f; // Cooldown for shield activation
    public int maxHealth = 100;
    public int shieldHealth = 60;
=======
    public float chaseSpeed; // Speed during chase
    public float patrolDistance; // Distance to patrol from the starting position
    public float shieldCooldown; // Cooldown for shield activation
    public Transform player;
    public Animator anim;

    [SerializeField] private float patrolSpeed;
>>>>>>> Stashed changes

    private bool isShieldActive = false;
    private float shieldCooldownTimer;
    private bool isChasing = false;

<<<<<<< Updated upstream
    private Transform player;
    private Vector3 startPosition;
    private Vector3 patrolTarget;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = transform.position;
        patrolTarget = GetRandomPointInPatrolArea();
=======

    // transforms for patrol points and variable hold current point enemy is moving to
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Transform currentPoint;
    private Rigidbody2D rb;
    private Coroutine activateShieldCoroutine;

   

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //player = GameObject.FindGameObjectWithTag("Player").transform;
>>>>>>> Stashed changes

        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
<<<<<<< Updated upstream
        // Check if player is within detection range
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
=======
        //// Check if player is within detection range
        //if (Vector2.Distance(transform.position, player.position) < detectionRange)
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

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthScript script = collision.GetComponent<HealthScript>();

        if (script != null)
>>>>>>> Stashed changes
        {
            if (!isChasing)
            {
<<<<<<< Updated upstream
                StartChase();
=======
                StopCoroutine(Patrol());
                isPatrolling = false;

                if (!isChasing)
                {
                    StartCoroutine(Chasing());
                    isChasing = true;
                }
>>>>>>> Stashed changes
            }
        }
        else if (isChasing)
        {
            StopChase();
        }

        // Move around within the patrol area while idle
        if (!isChasing)
        {
            Patrol();
            // Flip the sprite based on movement direction
            FlipSprite(patrolTarget - transform.position);
        }
        else
        {
            // Flip the sprite based on movement direction during chase
            FlipSprite(player.position - transform.position);
        }
    }
<<<<<<< Updated upstream

    void StartChase()
    {
        Debug.Log("Chasing player!");
        isChasing = true;

        // Activate shield periodically during the chase
        if (!isShieldActive && Time.time > shieldCooldownTimer)
        {
            ActivateShield();
=======
    private void OnTriggerExit2D(Collider2D collision)
    {
        // get health script
        HealthScript script = collision.gameObject.GetComponent<HealthScript>();
        // if health script found
        if (script != null)
        {
            StopCoroutine(Chasing());
            isChasing = false;
            //if (activateShieldCoroutine != null)
            //{
            //    StopCoroutine(activateShieldCoroutine);
            //    activateShieldCoroutine = null;
            //    isShieldActive = false;
            //}
            // if not patrolling, start
            if (!isPatrolling)
            {
                StartCoroutine(Patrol());
                isPatrolling = true;
            }
            
            // make sure enemy is facing player
            UpdateDirection(GetComponent<Collider2D>());

>>>>>>> Stashed changes
        }
    }
    IEnumerator Chasing()
    {
<<<<<<< Updated upstream
        Debug.Log("Stopping chase!");
        isChasing = false;
    }

    void ActivateShield()
    {
        Debug.Log("Shield activated!");
        isShieldActive = true;

        // Reset shield cooldown timer
        shieldCooldownTimer = Time.time + shieldCooldown;
    }

    public void TakeDamage(int damage)
    {
        if (isShieldActive)
=======
        while (isChasing)
>>>>>>> Stashed changes
        {
            Debug.Log("Chasing player!");
            anim.Play("Walk");
            isPatrolling = false;
            // make sure the enemy is facing towards the player
            if (rb.velocity.x > 0.0f && transform.localScale.x < 0 || rb.velocity.x < 0.0f && transform.localScale.x > 0)
            {
                Flip();
                // Calculate the direction to the player
                Vector2 direction = (player.position - transform.position).normalized;

                rb.velocity = new Vector2(direction.x * chaseSpeed, Quaternion.identity.y);

            }
            yield return null;
        }

        yield return null;

    }
<<<<<<< Updated upstream

    void DeactivateShield()
    {
        Debug.Log("Shield deactivated!");
        isShieldActive = false;
        shieldHealth = 60; // Reset shield health for the next use
    }

    void DefeatEnemy()
    {
        Debug.Log("Enemy defeated!");
        // Implement any actions you want to take when the enemy is defeated
        Destroy(gameObject);
    }

    Vector3 GetRandomPointInPatrolArea()
    {
        // Get a random point within the patrol area
        Vector3 randomDirection = Random.insideUnitSphere * patrolDistance;
        randomDirection += startPosition;
        return randomDirection;
    }
=======
    IEnumerator Patrol()
    {       
>>>>>>> Stashed changes

    void Patrol()
    {
        // Move towards the patrol target
        transform.position = Vector3.MoveTowards(transform.position, patrolTarget, idleSpeed * Time.deltaTime);

        // If reached the patrol target, get a new random target
        if (Vector3.Distance(transform.position, patrolTarget) < 0.1f)
        {
<<<<<<< Updated upstream
            patrolTarget = GetRandomPointInPatrolArea();
=======
            anim.Play("Walk");
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
>>>>>>> Stashed changes
        }
        yield break;
    }

<<<<<<< Updated upstream
    void FlipSprite(Vector3 direction)
=======
    //void StartChase()
    //{
    //    Debug.Log("Chasing player!");
    //    isChasing = true;
    //    anim.Play("Walk");
    //    isPatrolling = false;
    //    // make sure the enemy is facing towards the player
    //    if (rb.velocity.x > 0.0f && transform.localScale.x < 0 || rb.velocity.x < 0.0f && transform.localScale.x > 0)
    //    {
    //        Flip();
    //        // Calculate the direction to the player
    //        Vector2 direction = (player.position - transform.position).normalized;

    //        rb.velocity = new Vector2(direction.x * chaseSpeed, Quaternion.identity.y);

    //    }

    //    //// Activate shield periodically during the chase
    //    //if (!isShieldActive && Time.time > shieldCooldownTimer)
    //    //{
    //    //    //ActivateShield();
    //    //}
    //}



    //IEnumerator ActivateShield(Collider2D playerColl)
    //{
        
    //    Debug.Log("Shield activated!");
    //    isShieldActive = true;

    //    // Reset shield cooldown timer
    //    shieldCooldownTimer = Time.time + shieldCooldown;
    //}

    public void TakeDamage(int damage)
    {
        //if (isShieldActive)
        //{
        //    // If the shield is active, deduct damage from the shield
        //    shieldHealth -= damage;

        //    // If the shield is broken, deactivate it
        //    if (shieldHealth <= 0)
        //    {
        //        DeactivateShield();
        //    }
        //}
        //else
        //{
        //    // If the shield is not active, deduct damage from the enemy's health
        //    currentHealth -= damage;

        //    // Check if the enemy is defeated
        //    if (currentHealth <= 0)
        //    {
        //        DefeatEnemy();
        //    }
        //}
    }

    void DeactivateShield()
    {
        Debug.Log("Shield deactivated!");
        isShieldActive = false;
        //shieldHealth = 60; // Reset shield health for the next use
    }

    private void UpdateDirection(Collider2D collider)
    {
        // if facing the wrong way
        if ((collider.transform.position.x > transform.position.x && transform.localScale.x < 0) || (collider.transform.position.x < transform.position.x && transform.localScale.x > 0))
        {
            Flip();
        }
    }  
   


    void Flip()
>>>>>>> Stashed changes
    {
        // Flip the sprite based on the direction
        if (direction.x > 0)
        {
            // Moving right
            spriteRenderer.flipX = false;
        }
        else if (direction.x < 0)
        {
            // Moving left
            spriteRenderer.flipX = true;
        }
    }

  
}
