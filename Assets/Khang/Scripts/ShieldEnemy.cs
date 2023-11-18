using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : MonoBehaviour
{
    [SerializeField] private float walkingDistance;
    [SerializeField] private float walkingSpeed;
    [SerializeField] public GameObject pointA;
    [SerializeField] public GameObject pointB;
    private Rigidbody2D rb;
    private Transform currentPoint;


    //player related vars

    public Transform playerTransform;
    public bool isChasing;
    public float detectRange;
    public float chaseSpeed;

    //shield related vars
    private bool shieldUp = false;
    public float shieldCooldown = 20.0f;

    private void Start() //start stuff
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
    }

    private void Update() //patrol and chase
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

            StartCoroutine("ActivateShield");
        }
        else
        {


            if (currentPoint == pointB.transform)
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
    }

    private void Flip() //flips the sprite
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos() //draw range around the object in editor
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, detectRange);
    }

    private void OnCollisionEnter2D(Collision2D collision) //apply damage to player when they touch the guy
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<PlayerHealth>().TakeDamage(10);
        }
    }

    private IEnumerator ActivateShield() //shield function
    {
        shieldUp = true;

        if(shieldUp)
        {
            GetComponent<EnemyHealth>().addShield();
        }

        yield return new WaitForSeconds(10);
        
        shieldUp = false;

        
        yield return new WaitForSeconds(shieldCooldown);
    }
}
