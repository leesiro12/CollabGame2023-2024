using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorControl : MonoBehaviour
{
    public Transform[] points;
    public float speed = 2.0f;
    private int currentPointIndex = 1;
    private bool reverse = false;
    private bool isMoving = false;
    Transform targetPoint;

    [SerializeField] private GameObject colliders;

    private void Awake()
    {
        transform.position = points[0].position;        //Set platform to first point position

        if (colliders != null)
        {
            colliders.SetActive(false);
        }
    }

    void Update()
    {
        if (isMoving)
        {
            MovePlatform();
        }
    }

    public void StartMoving()       //player will interact thourgh this bool
    {
        if (!isMoving && points.Length > 0)
        {
            isMoving = true;
            if (colliders != null)
            {
                colliders.SetActive(true);
            }
        }
    }

    private void MovePlatform()
    {
        if (points.Length == 0)
            return;
        targetPoint = points[currentPointIndex];    //Set target Position
        float step = speed * Time.deltaTime;        //Moving speed
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, step);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.001f)    //Check To update next target
        {
            isMoving = false;
            UpdateNextPointIndex();

            if (colliders != null)
            {
                colliders.SetActive(false);
            }
        }
    }

    private void UpdateNextPointIndex()
    {
        if (!reverse)
        {
            if (currentPointIndex < points.Length - 1)
            {
                currentPointIndex++;
            }
            else
            {
                reverse = true;
                currentPointIndex--;
            }
        }
        else
        {
            if (currentPointIndex > 0)
            {
                currentPointIndex--;
            }
            else
            {
                reverse = false;
                currentPointIndex++;
            }
        }
    }
}
