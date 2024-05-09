using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    //vars
    public Transform player;
    public Transform elevatorSwitch;
    public Transform pos1;
    public Transform pos2;
    public Transform pos3;
    public int nextStop;
    public float speed;

    public static List<Transform> stopList;

    bool bottomUnlocked;



    void Start()
    {
        nextStop = 1;
        stopList = new List<Transform>()
                    { pos1,
                      pos2};
    }


    void Update()
    {
        
    }

    public void StartElevator()
    {
        //if(Vector2.Distance(player.position, elevatorSwitch.position)<0.5f) //input and interact
        //{
        //    if(transform.position.y <= downpos.position.y)
        //    {
        //        isDown = true;
        //    }
        //    else if(transform.position.y >= upperpos.position.y)
        //    {
        //        isDown = false;
        //    }
        //}
        Debug.Log("running");

        StartCoroutine(moveTo());
        

        //while (transform.position.y - nextStop.position.y > 0.5f)
        //{
        //    Debug.Log(transform.position);
        //    transform.position = Vector2.Lerp(StartPosition, nextStop.position, timeElapsed/2);
        //    timeElapsed += Time.deltaTime;
        //    if(timeElapsed >= 2)
        //    {
        //        break;
        //    }
        //}
       

    }

    IEnumerator moveTo()
    {
        Debug.Log("RUNNING COROUTINE");

        Vector3 StartPosition = transform.position;
        float timeElapsed = 0;
        while (Mathf.Abs(transform.position.y - stopList[nextStop].position.y) > 0.5f)
        {
            Debug.Log(transform.position);
            transform.position = Vector2.Lerp(StartPosition, stopList[nextStop].position, timeElapsed / (Mathf.Abs(StartPosition.y - stopList[nextStop].position.y) / speed));
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= (Mathf.Abs(StartPosition.y - stopList[nextStop].position.y) / speed))
            {
                break;
            }

            yield return new WaitForEndOfFrame();

        }
        if (nextStop >= stopList.Count - 1)
        {
            nextStop = 0;
        }
        else nextStop++;

        UnlockBottom();
    }

    public void UnlockBottom()
    {
        stopList.Add(pos3);
        stopList.Add(pos2);
    }
}
