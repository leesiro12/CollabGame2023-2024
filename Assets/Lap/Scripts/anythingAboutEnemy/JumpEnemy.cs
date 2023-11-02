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
    [SerializeField] private float detectRange = 2f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    


    public GameObject Player;
    private Rigidbody2D rb2d;
    private int currentPatrolIndex = 0;
    private float lastJumpTime = 0f;
    private bool isJumping = false;
    private bool CanPatrol = true;
    private bool isChasing = false;
    private float faceDirection;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, Player.transform.position) < detectRange)
        {
            isChasing = true;
            CanPatrol = false;
        } else isJumping = false;

        if (isChasing)
        {
            if (transform.position.x > Player.transform.position.x)
            {
                transform.position += Vector3.left * chaseSpeed * Time.deltaTime;
            }
            if (transform.position.x < Player.transform.position.x)
            {
                transform.position += Vector3.right * chaseSpeed * Time.deltaTime;
            }
        }

        if (!isJumping && Time.time - lastJumpTime > jumpDelay)
        {
            // Check if player is in jumping range
            if (Vector2.Distance(transform.position, Player.transform.position) <= jumpRange && isJumping == false)
            {
                isChasing = false;
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
        //rb2d.velocity = Vector2.up * jumpForce;
        //rb2d.AddForce(new Vector2 ( , jumpForce) * 3000); Hao commented this line because it caused error
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
        //else if (Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer))
        //{
        //    CanPatrol = true;
        //    ResetJumping();
        //} Hao commented this else if because it caused error
        
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
