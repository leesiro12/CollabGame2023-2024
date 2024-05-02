using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float walkingDistance;
    [SerializeField] private float walkingSpeed;
    [SerializeField] public GameObject pointA;
    [SerializeField] public GameObject pointB;
    private Rigidbody2D rb;
    private Transform currentPoint;
    

    //Player informations

    public Transform playerTransform;
    public bool isChasing;
    public float detectRange;
    public float chaseSpeed;

    public float attackRange;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
    }

    private void Update()
    {
        //chaseDistance = Mathf.Abs(Vector2.Distance(transform.position, playerTransform.position));
        
        if ((Vector2.Distance(transform.position, playerTransform.position) > detectRange))
            {
                isChasing = false;
            }
        if (Vector2.Distance(transform.position, playerTransform.position) < detectRange)
            {
                isChasing = true;
            } 
        if (isChasing)
        {
            if (transform.position.x > playerTransform.position.x)
            {
                transform.position += Vector3.left * chaseSpeed * Time.deltaTime;
            }
            if (transform.position.x < playerTransform.position.x)
            {
                transform.position += Vector3.right * chaseSpeed * Time.deltaTime;
            }
        }
        else
        {
            // move towards next point
            if ((currentPoint.position - transform.position).x < 0.0f)
            {
                rb.velocity = new Vector2(-1 * walkingSpeed, rb.velocity.y);

            }
            else
            {
                rb.velocity = new Vector2(walkingSpeed, rb.velocity.y);

            }

            if (Mathf.Abs(currentPoint.position.x - transform.position.x) < 0.5f && currentPoint == pointB.transform)
            {
                Flip();
                currentPoint = pointA.transform;
            }

            if (Mathf.Abs(currentPoint.position.x - transform.position.x) < 0.5f && currentPoint == pointA.transform)
            {
                Flip();
                currentPoint = pointB.transform;
            }
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;  
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, attackRange);
    }
}
