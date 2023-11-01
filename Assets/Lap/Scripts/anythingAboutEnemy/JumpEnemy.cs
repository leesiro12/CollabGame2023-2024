using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEnemy : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float chaseSpeed = 20f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpDelay = 2f;
    [SerializeField] private float jumpRange = 2f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    public GameObject Player;
    private Rigidbody2D rb2d;
    private int currentPatrolIndex = 0;
    private float lastJumpTime = 0f;
    private bool isJumping = false;
    private bool CanPatrol = true;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (!isJumping && Time.time - lastJumpTime > jumpDelay)
        {
            // Check if player is in jumping range
            if (Vector2.Distance(transform.position, Player.transform.position) <= jumpRange && isJumping == false)            {
                CanPatrol = false;
                Jump();
            }
        }

        // Patrol
        if (!isJumping)
        {
            CanPatrol = true;
            Patrolling();
            Vector2 movementDirection = (patrolPoints[currentPatrolIndex].position - transform.position).normalized;
            rb2d.velocity = movementDirection * moveSpeed;
        }
    }

    private void Jump()
    {
        isJumping = true;
        rb2d.velocity = Vector2.up * jumpForce;
        rb2d.AddForce(transform.forward * 30);
        lastJumpTime = Time.time;
        Invoke("ResetJumping", 1f);
    }

    private void ResetJumping()
    {
        isJumping = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Player.TakeDamage();
        }
        else if (Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer))
        {
            CanPatrol = true;
            ResetJumping();
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position,jumpRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, groundCheckDistance);
    }

    private void Patrolling()
    {
        if (Vector2.Distance(transform.position, patrolPoints[currentPatrolIndex].position) <= 0.5f && CanPatrol == true)
        {
            currentPatrolIndex++;
            if (currentPatrolIndex >= patrolPoints.Length)
            {
                currentPatrolIndex = 0;
            }
        }
    }

}
