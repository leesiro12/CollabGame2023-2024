using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Elevator : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the elevator
    public Transform targetPosition; // Target position to move to when activated

    private bool isActivated = false;

    void Update()
    {
        // Check if the player presses the 'E' key to activate the elevator using the new Input System
        if (Keyboard.current.eKey.wasPressedThisFrame && !isActivated)
        {
            ActivateElevator();
        }

        // Move the elevator towards the target position if activated
        if (isActivated)
        {
            MoveElevator();
        }
    }

    void ActivateElevator()
    {
        Debug.Log("Elevator activated!");
        isActivated = true;
    }

    void MoveElevator()
    {
        // Move the elevator towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

        // Check if the elevator has reached the target position
        if (Vector3.Distance(transform.position, targetPosition.position) < 0.1f)
        {
            isActivated = false;
        }
    }
}
