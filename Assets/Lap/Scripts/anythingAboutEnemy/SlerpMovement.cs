using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlerpMovement : MonoBehaviour
{
    [Header("Enemy Info")]    
    //[SerializeField] private float walkingSpeed;
    
    private Rigidbody2D rb;    
    private bool isCharging = false;
    private bool enemyPresent = true;
    private Coroutine attackCoroutine;  


    [Header("Bezier Curve Points")]
    public Transform startPoint;
    public Transform controlPoint1;
    public Transform controlPoint2;
    public Transform endPoint;


    //Player informations
    [Header("Player")]
    public Transform playerTransform;
    public bool isChasing;
    public float chaseSpeed;

    public float attackRange;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        HealthScript script = other.GetComponent<HealthScript>();

        if (script != null)
        {
            enemyPresent = true;

            if (attackCoroutine == null)
            {
                Debug.Log("Attacking");

                // Update Bezier curve points
                startPoint.position = transform.position;
                controlPoint1.position = transform.position + Vector3.up * 2f; // Adjust as needed
                controlPoint2.position = other.transform.position + Vector3.up * 2f; // Adjust as needed
                endPoint.position = other.transform.position;

                attackCoroutine = StartCoroutine(ChargePlayer(startPoint.position, controlPoint1.position, controlPoint2.position, endPoint.position, 3f));
            }

            UpdateDirection(other);
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        HealthScript script = GetComponent<HealthScript>();

        if (script != null)
        {
            Debug.Log("Staying");
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
            Debug.Log("enemy stop presenting");
            //StopCoroutine(attackCoroutine);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
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

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (attackCoroutine != null)
        {
            Debug.Log("Player detected");
            // stop the coroutine form running
            StopCoroutine(attackCoroutine);
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
        else
        {
            yield return null;
        }
               
    }
    
    IEnumerator ChargePlayer(Vector3 p0, Vector2 p1, Vector2 p2, Vector3 p3, float duration)
    {
        //while (enemyPresent)
        //{
        //    yield return new WaitForSeconds(1f);

        //    Vector3 a = Vector3.Lerp(p0, p1, t * Time.deltaTime);
        //    Vector3 b = Vector3.Lerp(p1, p2, t * Time.deltaTime);
        //    Vector3 c = Vector3.Lerp(p2, p3, t * Time.deltaTime);
        //    Vector3 d = Vector3.Lerp(a, b, t * Time.deltaTime);
        //    Vector3 e = Vector3.Lerp(b, c, t * Time.deltaTime);
        //    transform.position = Vector3.Lerp(d, e, t * Time.deltaTime);



       
        //    Debug.Log("Is charging");

        //    isCharging = true;
        //    yield return new WaitForSeconds(3f);
        //    isCharging = false;
        //}

        //attackCoroutine = null;

        float timeElapsed = 0f;

        while (timeElapsed < duration && enemyPresent == true)
        {
            float t = timeElapsed / duration;

            // Calculate position on the Bezier curve using Bernstein polynomial
            Vector3 newPosition = CalculateBezierPoint(p0, p1, p2, p3, t);

            // Move the enemy to the new position
            transform.position = newPosition;

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        attackCoroutine = null;
    }

    Vector3 CalculateBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        // Bezier curve equation
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(leftLimit.transform.position, 0.5f);
        //Gizmos.DrawWireSphere(rightLimit.transform.position, 0.5f);
        //Gizmos.DrawLine(leftLimit.transform.position, rightLimit.transform.position);

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(this.transform.position, detectRange);

        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(this.transform.position, attackRange);

        //Gizmos.color = Color.cyan;        
        //Gizmos.DrawWireSphere(leftLandingPoint, 0.5f);
        //Gizmos.DrawWireSphere(rightLandingPoint, 0.5f);
        //Gizmos.DrawLine(leftLandingPoint, rightLandingPoint);

        Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(leftUpperPoint, 0.5f);
        //Gizmos.DrawWireSphere(rightUpperPoint, 0.5f);
        Gizmos.DrawWireSphere(Vector3.left, 0.5f);
    }
}
