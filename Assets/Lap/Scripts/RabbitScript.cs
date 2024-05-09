using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitScript : MonoBehaviour
{
    [SerializeField] private float detectRange = 5f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float walkingSpeed = 3f;
    [SerializeField] public GameObject pointA;
    [SerializeField] public GameObject pointB;
    [SerializeField] public Transform aimPos;

    private Transform currentPoint;
    private Transform player;
    private bool isJumping = false;
    private bool isPatrolling = true;
    private Vector3 jumpStart;
    private float jumpDuration;
    private Rigidbody2D rb;

    Coroutine jumpCoroutine;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = this.transform.GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        while (distanceToPlayer <= detectRange)
        {
            StopCoroutine(PatrolAfterDelay());
            if (!isJumping)
            {
                isJumping = true;
                jumpCoroutine = StartCoroutine(JumpAfterDelay());
            }
        }
        
         if (jumpCoroutine != null)
            {
                StopCoroutine(jumpCoroutine);
            }
         StartCoroutine(PatrolAfterDelay());
        
    }

    private IEnumerator JumpAfterDelay()
    {
        aimPos = player.transform;
        yield return new WaitForSeconds(1f);
        JumpToPlayer();
    }

    private IEnumerator PatrolAfterDelay()
    {
        if (isPatrolling == true && isJumping == false)
        {
            yield return new WaitForSeconds(5f);
            Patrol();
        }
        else if (isPatrolling == false && isJumping == false)
        {
            isPatrolling = true;
        }

    }

    private void JumpToPlayer()
    {
        isJumping = true;
        isPatrolling = false;
        jumpStart = transform.position;

        jumpDuration = 2f * jumpHeight / jumpSpeed;
        float horizontalDistance = aimPos.position.x - jumpStart.x;
        float horizontalSpeed = horizontalDistance / jumpDuration;
        float verticalSpeed = Mathf.Sqrt(2f * jumpHeight * Physics2D.gravity.magnitude);

        GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalSpeed, 6);
        isJumping = false;
    }

    //private void FinishJumping()
    //{
    //    isJumping = false;
    //}

    private void Patrol()
    {
        Vector2 point = currentPoint.position - transform.position;

        if (currentPoint == pointB.transform )
        {
            rb.velocity = new Vector2(walkingSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-walkingSpeed, 0);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            Flip();
            currentPoint = pointA.transform;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            Flip();
            currentPoint = pointB.transform;
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pointB.transform.position, 2f);
        Gizmos.DrawWireSphere(pointA.transform.position, 2f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}
