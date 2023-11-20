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
    private bool isAttacking = false;
    private bool isCharging = false;
    private bool enemyPresent = true;
    private Coroutine attackCoroutine;

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


    private void OnTriggerEnter2D(Collider2D other)
    {
        HealthScript script = other.GetComponent<HealthScript>();

        if (script != null)
        {
            enemyPresent = true;

            if(attackCoroutine == null)
            {

                leftLandingPoint = transform.position * transform.localScale.x;
                leftUpperPoint = new Vector3(transform.position.x + 1f, transform.position.y + 3.5f, transform.position.z) * transform.localScale.x;
                rightUpperPoint = new Vector3(transform.position.x + 3f, transform.position.y + 3.5f, transform.position.z) * transform.localScale.x;
                rightLandingPoint = new Vector3(transform.position.x + attackRange, transform.position.y, transform.position.z) * transform.localScale.x; 

                attackCoroutine = StartCoroutine(ChargePlayer(leftLandingPoint, leftUpperPoint, rightUpperPoint, rightLandingPoint, 1f));
            }

            UpdateDirection(other);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        HealthScript script = GetComponent<HealthScript>();

        if (script != null)
        {
            UpdateDirection(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        HealthScript script = collision.gameObject.GetComponent<HealthScript>();
        // if health script found
        if (script != null)
        {
            // marks player is not in trigger area
            enemyPresent = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (attackCoroutine != null)
        {
            // stop the coroutine form running
            StopCoroutine(attackCoroutine);
        }

        //warning.SetActive(false);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // get health script
        HealthScript script = collision.gameObject.GetComponent<HealthScript>();
        // if health script found
        if (script != null)
        {

            if (isCharging)
            {
                // apply double damage
                script.TakeDamage(20);
                Debug.Log("dealt 20 damage");
            }
            else
            {
                // apply damage
                script.TakeDamage(10);
                Debug.Log("dealt 10 damage");
            }

            // start coroutine for timed attack and hold reference in coroutine variable
            attackCoroutine = StartCoroutine(TimedAttack(collision));
        }
    }

    void UpdateDirection(Collider2D collider)
    {
        if ((collider.transform.position.x > transform.position.x && transform.localScale.x < 0) || (collider.transform.position.x < transform.position.x && transform.localScale.x > 0))
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }        
    }
    

   

    IEnumerator TimedAttack(Collision2D collision)
    {

        HealthScript script = collision.gameObject.GetComponent<HealthScript>();

        if (script != null)
        {
            while (true)
            {
                yield return new WaitForSeconds(1.5f);

                script.TakeDamage(10);

                Debug.Log("dealt 10 damage (repeat)");
            } 
        }
               
    }
    
    IEnumerator ChargePlayer(Vector3 p0, Vector2 p1, Vector2 p2, Vector3 p3, float t)
    {
        while (enemyPresent)
        {
            yield return new WaitForSeconds(1f);

            Vector3 a = Vector3.Lerp(p0, p1, t * Time.deltaTime);
            Vector3 b = Vector3.Lerp(p1, p2, t * Time.deltaTime);
            Vector3 c = Vector3.Lerp(p2, p3, t * Time.deltaTime);
            Vector3 d = Vector3.Lerp(a, b, t * Time.deltaTime);
            Vector3 e = Vector3.Lerp(b, c, t * Time.deltaTime);
            Vector3 updatePos = Vector3.Lerp(d, e, t * Time.deltaTime);

            transform.position = updatePos;
            Debug.Log("Is charging");

            isCharging = true;
            yield return new WaitForSeconds(3f);
            isCharging = false;
        }

        attackCoroutine = null;
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
