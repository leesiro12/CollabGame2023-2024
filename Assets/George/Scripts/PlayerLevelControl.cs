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

    [SerializeField] private Canvas pauseMenu;

    private PauseMenu menuScript;

    private void Awake()
    {
        // get reference to input map
        playerControls = new PlayerInputActions();
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
        //interaction happened
    }
}
