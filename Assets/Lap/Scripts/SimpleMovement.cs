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
    public InputAction VerticalMove;
    private InputAction JumpAction;
    private InputAction DashAction;

    private bool canDash = true;
    private bool isDashing = false;
    private bool isKnocked = false;
    private float dashingPower = 18f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;

    private bool hasDoubleJumped;

    float moveDirection = 0.0f;
    [SerializeField] private float speed = 8f;
    [SerializeField] public float jumpingPower;
    private bool isFacingRight = true;

    [SerializeField] private GameObject currentOneWayPlatform;
    [SerializeField] private BoxCollider2D playerCollider;

    public Animator anim;
    public HealthScript health;

    public float footstepDelay = 0.35f;

    private float nextfootstep = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerControls = new PlayerInputActions();
        tr = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        tr.emitting = false;

        JumpAction = playerControls.Player.Jump;
        JumpAction.Enable();
        JumpAction.performed += Jump;

        HorizontalMove.Enable();

        VerticalMove = playerControls.Player.OneWayPlat;
        VerticalMove.Enable();
        VerticalMove.performed += OneWayDown;


        DashAction = playerControls.Player.Dash;
        DashAction.Enable();
        DashAction.performed += Dash;



    }

    private void OnDisable()
    {
        HorizontalMove.Disable();
        JumpAction.Disable();
        DashAction.Disable();
        VerticalMove.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = HorizontalMove.ReadValue<float>();

        if (health && health.playerHealth <= 0)
        {
            Debug.Log("Dying Movement");
            anim.Play("Dead");
        }
        else if (!isDashing && health && health.playerHealth > 0)
        {
            Flipping();
            Jumping();
            Running();
        }
    }

    public void Flipping()
    {
        if (moveDirection < 0f && isFacingRight)
        {
            Flip();
        }
        else if (moveDirection > 0f && !isFacingRight)
        {
            Flip();
        }
    }

    public void Jumping()
    {

        if (!IsGrounded() && rb.velocity.y > 0)
        {
            // anim.Play("Dead");

            anim.Play("Jump");
        }
        else if (!IsGrounded() && rb.velocity.y < 0)
        {
            anim.Play("Fall");
        }
    }

    public void Running()
    {
        if (moveDirection == 0f && IsGrounded())
        {
            anim.Play("Idle");
        }
        else if (moveDirection < 0f && IsGrounded() || moveDirection > 0f && IsGrounded())
        {
            PlayFootStep();
            anim.Play("Run");
        }
    }

    public void PlayFootStep()
    {
        nextfootstep -= Time.deltaTime;

        if (nextfootstep < 0)
        {
            MAudioManager.instance.PlaySFX("Footsteps");
            nextfootstep = footstepDelay;
        }
    }

    public void OneWayDown(InputAction.CallbackContext context)
    {
        if (currentOneWayPlatform != null)
        {
            StartCoroutine(DisableCollision());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
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
            MAudioManager.instance.PlaySFX("Jump");
            hasDoubleJumped = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            anim.Play("Jump");
        }
        else if (context.performed && !IsGrounded() && !hasDoubleJumped)
        {
            MAudioManager.instance.PlaySFX("Jump");
            hasDoubleJumped = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            anim.Play("Jump");
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
            anim.Play("Dash");
            StartCoroutine(ActivateDash());           

        }
    }

    private IEnumerator ActivateDash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, rb.velocity.y);
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
