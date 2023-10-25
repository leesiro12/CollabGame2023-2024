using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPivotControler : MonoBehaviour
{
    public float moveSpeed = 1f;

    private Vector2 moveInput;
    private Rigidbody2D rb;

    public Transform target;
    public Vector3 offset;
    public float damping;

    private Vector2 velocity = Vector2.zero;

    public bool FreeCamIsOn;

    private void Start()
    {
        FreeCamIsOn = false;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {

        checkForFreeCam();
        

        if (FreeCamIsOn == true)
        {
            rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            Vector2 movePosition = target.position + offset;
            transform.position = Vector2.SmoothDamp(transform.position, movePosition, ref velocity, damping);
        }


    }

    private void OnFly(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void checkForFreeCam()
    {

        if (target.GetComponent<Rigidbody2D>().velocity == new Vector2(0, 0) && moveInput != new Vector2(0, 0))
        {
            FreeCamIsOn = true;
        }
        else
        {
            FreeCamIsOn = false;
        }
    }
}
