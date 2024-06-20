using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerLevelControl : MonoBehaviour
{
    // to hold references to input actions and map
    public PlayerInputActions playerControls;
    private InputAction pause;
    private InputAction interact;
    //private bool interactTriggered;
    public Transform interactPoint;
    public float interactRange;
    public LayerMask interactLayers;

    public static bool dialogueIsPlayed = false;
    private bool elevatorIsMoving;

    private SimpleMovement movementScript;

    [SerializeField] private Canvas pauseMenu;

    private PauseMenu menuScript;

    private void Awake()
    {
        // get reference to input map
        playerControls = new PlayerInputActions();

        movementScript = GetComponentInParent<SimpleMovement>();
    }

    private void OnEnable()
    {
        pause = playerControls.Player.Pause;
        pause.Enable();
        pause.performed += OnPause;

        interact = playerControls.Player.Interact;
        interact.Enable();
        interact.performed += OnInteract;
    }

    

    private void OnDisable()
    {
        pause.Disable();
        interact.Disable();
    }

    private void Start()
    {
        if (pauseMenu != null)
        {
            menuScript = pauseMenu.GetComponent<PauseMenu>();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (menuScript != null)
        {
            menuScript.OnPause();
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        
        //interactTriggered = true;

        Collider2D[] collideObject = Physics2D.OverlapCircleAll(interactPoint.position, interactRange, interactLayers);

        foreach (Collider2D obj in collideObject)
        {
            //Debug.Log("Interacting with " + obj.name);   
            //if (obj.isTrigger)
            //{
            //    Debug.Log("is triggered");
            //}

            

            if (obj.gameObject.GetComponent<DialogueTrigger>())
            {
                obj.GetComponent<DialogueTrigger>().TriggerDialogue(movementScript);


                //Debug.Log("Can run dialogue!");
                //if (dialogueIsPlayed == false)
                //{
                    
                //    dialogueIsPlayed = true;
                //}
                //else
                //{
                //    obj.GetComponent<DialogueTrigger>().TriggerDialogue(dialogueIsPlayed);                    
                //}                 
            }

            if (obj.gameObject.GetComponent<ElevatorControl>())
            {
                //Debug.Log("Elevator detected");
                
                //if (elevatorIsMoving == false)
                //{
                //    obj.GetComponent<Elevator>().StartElevator();

                //}
                obj.GetComponent<ElevatorControl>().StartMoving();

                //elevatorIsMoving = true;

            }
        }
    }
}
