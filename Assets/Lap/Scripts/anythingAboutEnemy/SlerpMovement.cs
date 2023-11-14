using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlerpMovement : MonoBehaviour
{
    [Header("Enemy Info")]    
    [SerializeField] private float walkingSpeed;
    [SerializeField] public GameObject leftLimit;
    [SerializeField] public GameObject rightLimit;
    [SerializeField] public Vector3 leftLandingPoint;
    [SerializeField] public Vector3 rightLandingPoint;
    [SerializeField] public Vector3 leftUpperPoint;
    [SerializeField] public Vector3 rightUpperPoint;
    private Rigidbody2D rb;
    private Transform currentPoint; // show the next patrol point that enemy is moving to
    private bool isFacingRight = true;


    //Player informations
    [Header("Player")]
    public Transform playerTransform;
    public bool isChasing;
    public float detectRange;
    public float chaseSpeed;

    public float attackRange;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = rightLimit.transform;
        //playerTransform = GameObject.FindGameObjectsWithTag("Player");
    }

    private void Update()
    {
        //check for facing direction
        if (transform.localScale.x > 0)
        {
            // Character is facing right
            isFacingRight = true;
            Debug.Log("Character is facing right");

            leftLandingPoint = transform.position;
            leftUpperPoint = new Vector3(transform.position.x + 1f, transform.position.y + 3.5f, transform.position.z);
            rightUpperPoint = new Vector3(transform.position.x + 3f, transform.position.y + 3.5f, transform.position.z);
            rightLandingPoint = new Vector3(transform.position.x + attackRange, transform.position.y, transform.position.z);
        }
        else if (transform.localScale.x < 0)
        {
            // Character is facing left
            isFacingRight = false;
            Debug.Log("Character is facing left");

            leftLandingPoint = new Vector3(transform.position.x - attackRange, transform.position.y, transform.position.z);
            leftUpperPoint = new Vector3(transform.position.x - 3f, transform.position.y + 3.5f, transform.position.z);
            rightUpperPoint = new Vector3(transform.position.x -1f, transform.position.y + 3.5f, transform.position.z);
            rightLandingPoint = transform.position;
        }
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, playerTransform.position) < attackRange)
        {
            isChasing = false;

            if (isFacingRight)
                StartCoroutine(Attack(leftLandingPoint, leftUpperPoint, rightUpperPoint, rightLandingPoint, 1f));
            else StartCoroutine(Attack(rightLandingPoint, rightUpperPoint, leftUpperPoint, leftLandingPoint, 1f));
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
        if (Vector2.Distance(transform.position, playerTransform.position) < detectRange)
        {
            isChasing = true;
        }       
        else
        {
            isChasing = false;
            if (currentPoint == rightLimit.transform)
            {
                rb.velocity = new Vector2(walkingSpeed, 0);
            }
            else
            {
                rb.velocity = new Vector2(-walkingSpeed, 0);
            }

            if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == rightLimit.transform)
            {
                Flip();
                currentPoint = leftLimit.transform;
            }

            if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == leftLimit.transform)
            {
                Flip();
                currentPoint = rightLimit.transform;
            }
        }

        
    }

    IEnumerator Attack(Vector3 p0, Vector2 p1, Vector2 p2, Vector3 p3, float t)
    {
        yield return new WaitForSeconds(1f);

        Vector3 a = Vector3.Lerp(p0, p1, t *Time.deltaTime);
        Vector3 b = Vector3.Lerp(p1, p2, t * Time.deltaTime);
        Vector3 c = Vector3.Lerp(p2, p3, t * Time.deltaTime);
        Vector3 d = Vector3.Lerp(a, b, t * Time.deltaTime);
        Vector3 e = Vector3.Lerp(b, c, t * Time.deltaTime);
        transform.position = Vector3.Lerp(d, e, t * Time.deltaTime);        
    }
    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(leftLimit.transform.position, 0.5f);
        Gizmos.DrawWireSphere(rightLimit.transform.position, 0.5f);
        Gizmos.DrawLine(leftLimit.transform.position, rightLimit.transform.position);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, attackRange);

        Gizmos.color = Color.cyan;        
        Gizmos.DrawWireSphere(leftLandingPoint, 0.5f);
        Gizmos.DrawWireSphere(rightLandingPoint, 0.5f);
        Gizmos.DrawLine(leftLandingPoint, rightLandingPoint);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(leftUpperPoint, 0.5f);
        Gizmos.DrawWireSphere(rightUpperPoint, 0.5f);
        Gizmos.DrawWireSphere(Vector3.left, 0.5f);
    }
}
