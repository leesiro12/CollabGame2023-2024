using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActivateBlock : MonoBehaviour
{
    private Rigidbody2D rb;

    // where to spawn
    public Transform blockPoint;

    public PlayerInputActions playerControls;
    private InputAction block;

    // prefab of blocking object
    public GameObject blockPrefab;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerControls = new PlayerInputActions();
    }

    // activating the input and assigning it to the instBlock method
    private void OnEnable()
    {
        block = playerControls.Player.Block;
        block.Enable();
        block.performed += instBlock;
    }

    // deactivating the input
    private void OnDisable()
    {
        block.Disable();
    }

    // create instance of prefab at block point
    private void instBlock(InputAction.CallbackContext context)
    {
        Instantiate(blockPrefab, blockPoint.position, Quaternion.identity);
    }

}
