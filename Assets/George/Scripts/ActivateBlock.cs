using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActivateBlock : MonoBehaviour
{
    private Rigidbody2D rb;

    public Transform blockPoint;

    public PlayerInputActions playerControls;
    private InputAction block;

    public GameObject blockPrefab;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        block = playerControls.Player.Block;
        block.Enable();
        block.performed += instBlock;
    }

    private void OnDisable()
    {
        block.Disable();
    }

    private void instBlock(InputAction.CallbackContext context)
    {
        Instantiate(blockPrefab, blockPoint.position, Quaternion.identity);
    }

}
