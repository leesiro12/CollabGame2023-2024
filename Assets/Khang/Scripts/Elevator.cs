using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    //vars
    public Transform player;
    public Transform elevatorSwitch;
    public Transform downpos;
    public Transform upperpos;

    public float speed;
    bool isDown;



    void Start()
    {
        
    }


    void Update()
    {
        
    }

    void StartElevator()
    {
        if(Vector2.Distance(player.position, elevatorSwitch.position)<0.5f) //input and interact
        {
            if(transform.position.y <= downpos.position.y)
            {
                isDown = true;
            }
            else if(transform.position.y >= upperpos.position.y)
            {
                isDown = false;
            }
        }

        if(isDown)
        {
            transform.position = Vector2.MoveTowards(transform.position, upperpos.position, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, downpos.position, speed * Time.deltaTime);
        }
    }
}
