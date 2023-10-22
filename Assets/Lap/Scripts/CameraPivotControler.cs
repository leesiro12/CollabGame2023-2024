using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPivotControler : MonoBehaviour
{
    private int speed = 10;

    Vector2 pivotMovement;
    //[SerializeField] bool FreeCamIsOn;
    //GameObject player;
    Rigidbody2D rb;

    private void Awake()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        //FreeCamIsOn = false;
        rb= GetComponent<Rigidbody2D>();
    }


    
    public void OnMovement(InputValue value)
    {
        //FreeCamIsOn = true;
        pivotMovement = value.Get<Vector2>();
        Debug.Log("move check");
    }

    private void FixedUpdate()
    {

        //pivotMovement = transform.position;
        //if (!FreeCamIsOn) 
        //{
        //    this.transform.position = player.transform.position;
        //}
        rb.MovePosition(rb.position + pivotMovement * speed * Time.deltaTime);
    }


}
