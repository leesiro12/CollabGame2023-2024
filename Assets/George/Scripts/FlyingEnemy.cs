using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    private Transform currentPoint;

    private Rigidbody2D rb;
    [SerializeField] private float speed;

    private bool isPatrolling;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentPoint = pointA;

        StartCoroutine(StartPatrol());
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // get health script
        HealthScript script = collider.GetComponent<HealthScript>();
        // if health script found
        if (script != null)
        {
            if (isPatrolling)
            {
                isPatrolling = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        // get health script
        HealthScript script = collider.GetComponent<HealthScript>();
        // if health script found
        if (script != null)
        {
            if(!isPatrolling)
            {
                StartCoroutine(StartPatrol());
            }
        }
    }

    IEnumerator StartPatrol()
    {
        isPatrolling = true;

        while(isPatrolling)
        {
            rb.velocity = speed * Vector3.Normalize(currentPoint.position - transform.position);

            if (Mathf.Abs((currentPoint.position - transform.position).magnitude) <= 0.5f)
            {
                if(currentPoint == pointA)
                {
                    currentPoint = pointB;
                }
                else
                {
                    currentPoint = pointA;
                }
                Flip();
            }

            yield return new WaitForFixedUpdate();
        }

        rb.velocity = new Vector3 (0, 0, 0);
        yield return null;
    }

    private void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        return;
    }
}
