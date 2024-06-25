using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShieldEnemy : MonoBehaviour
{
    //public float detectionRange = 10f; // Range to detect the player
    public float chaseSpeed; // Speed during chase
    public float patrolSpeed; // Speed while patrolling
    //public float patrolDistance; // Distance to patrol from the starting position
    public float shieldDuration = 2f;
    private float shieldDurationTimer = 0;


    private bool isShieldActive = false;
    private bool isChasing = false;
    private bool isPatrolling;

    private Transform player;
    // transforms for patrol points and variable hold current point enemy is moving to
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Transform currentPoint;

    //[SerializeField] private float patrolSpeed;
    private Rigidbody2D rb;
    private EnemyHealth healthScript;
    private Coroutine activateShieldCoroutine;
    private Coroutine attackCoroutine;

    public Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        healthScript = GetComponent<EnemyHealth>();

        Transform[] childrenTransforms = transform.parent.GetComponentsInChildren<Transform>();

        for (int i = 0; i < childrenTransforms.Length; i++)
        {
            
            if (childrenTransforms[i].gameObject.layer != LayerMask.NameToLayer("Enemies"))
            {
                if (pointA == null)
                {
                    pointA = childrenTransforms[i];
                }
                else if (pointB == null)
                {
                    pointB = childrenTransforms[i];
                }
            }
        }

        currentPoint = pointA;

        // makes enemy start patrolling at beginning of game
        StartCoroutine(Patrol());
    }

    void Update()
    {
        if (isChasing && player)
        {
            if (isShieldActive && activateShieldCoroutine == null)
            {
                if (attackCoroutine == null)
                {
                    Debug.Log("attack starting");
                    shieldDurationTimer = 0;
                    attackCoroutine = StartCoroutine(Attack());
                }
            }
            else if (!isShieldActive && Mathf.Abs(transform.position.x - player.transform.position.x) < 1 && attackCoroutine == null)
            {
                Debug.Log("shield starting");

                isShieldActive = true;

                if (activateShieldCoroutine == null)
                {
                    activateShieldCoroutine = StartCoroutine(ActivateShield());
                }
            }
            else if (!isShieldActive && transform.position.x > player.transform.position.x)
            {
                Debug.Log("chasing");

                transform.position += Vector3.left * chaseSpeed * Time.deltaTime;
                UpdateDirection(player);
            }
            else if (!isShieldActive && transform.position.x < player.transform.position.x)
            {
                Debug.Log("chasing");
                transform.position += Vector3.right * chaseSpeed * Time.deltaTime;
                UpdateDirection(player);
            }

        }
        else if (isChasing && !player)
        {
            if (activateShieldCoroutine != null)
            {
                StopCoroutine(activateShieldCoroutine);
                activateShieldCoroutine = null;
                isShieldActive = false;
                healthScript.setIsShielding(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        HealthScript script = collision.GetComponent<HealthScript>();

        if (script != null)
        {
            player = collision.transform;  //NEW
            if (isPatrolling)
            {
                isPatrolling = false;

                if (!isChasing)
                {
                    isChasing = true;
                }

                // make sure enemy is facing player
                UpdateDirection(collision.transform);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // get health script
        HealthScript script = collision.gameObject.GetComponent<HealthScript>();
        // if health script found
        if (script != null)
        {
            player = null;
            if (isChasing)
            {
                isChasing = false;
            }

            if (activateShieldCoroutine != null)
            {
                StopCoroutine(activateShieldCoroutine);
                activateShieldCoroutine = null;
                isShieldActive = false;
                healthScript.setIsShielding(false);
            }

            // if not patrolling, start
            if (!isPatrolling)
            {
                StartCoroutine(Patrol());
            }
        }
    }

    IEnumerator ActivateShield()
    {
        //Debug.Log("Shield activated!");
        //shield animation
        isShieldActive = true;
        healthScript.setIsShielding(true);

        while (shieldDurationTimer < shieldDuration)
        {
            yield return new WaitForSeconds(0.01f);
            shieldDurationTimer += Time.deltaTime;
        }
        
        activateShieldCoroutine = null;
    }
   

    IEnumerator Patrol()
    {
        Debug.Log("start patrolling");
        isPatrolling = true;

        UpdateDirection(currentPoint);

        while (isPatrolling)
        {
            anim.Play("Walk");
            //Debug.Log("Shield Enemy Patrolling");

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

            }
            else if (Mathf.Abs(currentPoint.position.x - transform.position.x) < 0.5f && currentPoint == pointB)
            {
                currentPoint = pointA;
            }

            UpdateDirection(currentPoint);

            yield return new WaitForFixedUpdate();
        }
    }
    
    private void UpdateDirection(Transform playerTransform)
    {
        // if facing the wrong way
        if ((playerTransform.position.x > transform.position.x && transform.localScale.x < 0) || (playerTransform.position.x < transform.position.x && transform.localScale.x > 0))
        {
            Flip();
        }
    }

    void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        return;
    }

    IEnumerator Attack()
    {
        //play anim
        //Debug.Log("anim start");
        yield return new WaitForSeconds(1f);
        //Debug.Log("anim end");
        
        if (player && transform.position.x - player.transform.position.x < 1)
        {
            if (player.GetComponent<HealthScript>().playerHealth <= 1)
            {
                //Debug.Log("damage and kill");
                player.GetComponent<HealthScript>().TakeDamage(1);
                player = null;
            }
            else
            {
                //Debug.Log("damage");
                player.GetComponent<HealthScript>().TakeDamage(1);
            }
        }

        isShieldActive = false;
        healthScript.setIsShielding(false);

        attackCoroutine = null;
    }
}
