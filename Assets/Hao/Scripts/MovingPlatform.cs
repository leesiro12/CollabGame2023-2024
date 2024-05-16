using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public int startingPoint;
    public Transform[] points;

    private int i;

    private void Start()
    {
        // choose from which point the platform will start
        transform.position = points[startingPoint].position;
    }

    private void Update()
    {
        // check the distance between the platform and point position
        if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            // increase the index to move to the next point
            i++;

            // check if the platform was in the last point position
            if (i == points.Length)
            {
                // reset the index
                i = 0;
            }
        }

        // move the transform of the platform
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // when player collides with the platform, move player with the platform
        if (collision.transform.tag == "Player")
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // when player exits the platform, detach player from the platform
        if (collision.transform.tag == "Player")
        {
            collision.transform.SetParent(null);
        }
    }
}
