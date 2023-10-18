using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SimpleMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public PlayerInputActions playerControls;
    public InputAction HorizontalMove;
    private InputAction JumpAction;

    float moveDirection = 0.0f;

    private float speed = 8f;
    [SerializeField] public float jumpingPower;
    private bool isFacingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {

        JumpAction = playerControls.Player.Jump;
        JumpAction.Enable();
        JumpAction.performed += Jump;

        HorizontalMove.Enable();
    }

    private void OnDisable()
    {
        HorizontalMove.Disable();
        JumpAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = HorizontalMove.ReadValue<float>();

        if (!isFacingRight && moveDirection > 0f)
        {
            Flip();
        }
        else
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
    }


    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower * 0.5f);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}
