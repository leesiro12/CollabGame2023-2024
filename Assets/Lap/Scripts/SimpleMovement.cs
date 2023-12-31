using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SimpleMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public PlayerInputActions playerControls;
    public InputAction HorizontalMove;
    private InputAction JumpAction;
    private InputAction DashAction;

    private bool canDash = true;
    private bool isDashing = false;
    private bool isKnocked = false;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;

    private bool hasDoubleJumped;

    float moveDirection = 0.0f;
    [SerializeField] private float speed = 8f;
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

        DashAction = playerControls.Player.Dash;
        DashAction.Enable();
        DashAction.performed += Dash;
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
        else if (isFacingRight && moveDirection < 0f)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing && !isKnocked)
        {
            rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
        }
    }


    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            hasDoubleJumped = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        else if (context.performed && !IsGrounded() && !hasDoubleJumped)
        {
            hasDoubleJumped = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
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

    public void Dash(InputAction.CallbackContext context)
    {
        if (canDash)
        {
            StartCoroutine(ActivateDash());
        }
    }

    private IEnumerator ActivateDash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    public void SetKnocked(bool knocked)
    {
        isKnocked = knocked;
    }
}
