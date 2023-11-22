using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : MonoBehaviour
{
    public float detectionRange = 10f; // Range to detect the player
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

        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Check if player is within detection range
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            if (!isChasing)
            {
                StartChase();
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

    void StartChase()
    {
        Debug.Log("Chasing player!");
        isChasing = true;

        // Activate shield periodically during the chase
        if (!isShieldActive && Time.time > shieldCooldownTimer)
        {
            ActivateShield();
        }
    }

    void StopChase()
    {
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

    void Patrol()
    {
        // Move towards the patrol target
        transform.position = Vector3.MoveTowards(transform.position, patrolTarget, idleSpeed * Time.deltaTime);

        // If reached the patrol target, get a new random target
        if (Vector3.Distance(transform.position, patrolTarget) < 0.1f)
        {
            patrolTarget = GetRandomPointInPatrolArea();
        }
    }

    void FlipSprite(Vector3 direction)
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
