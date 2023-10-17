using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class AttackScript : MonoBehaviour
{
    public Rigidbody2D rb;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask targetLayers;

    public PlayerInputActions playerControls;
    private InputAction meleeAttack;
    private InputAction rangedAttack;

    public GameObject projectile;
    private float projectileSpeed = 5.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        meleeAttack = playerControls.Player.Attack;
        meleeAttack.Enable();
        meleeAttack.performed += Attack;

        rangedAttack = playerControls.Player.RangedAttack;
        rangedAttack.Enable();
        rangedAttack.performed += RangedAttack;
    }

    private void OnDisable()
    {
        meleeAttack.Disable();
        rangedAttack.Disable();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void Attack(InputAction.CallbackContext context)
    {
        Debug.Log("Attack");

        StartCoroutine(InputCheck(context));

        // play attack animation
        //

        if(context.started)
        {
            Debug.Log("started true");
        }

        if(context.performed)
        {
            Debug.Log("performed true");
        }

        if(context.canceled)
        {
            Debug.Log("canceled true");
        }

        

        // detect enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, targetLayers);

        // damage enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("enemy hit: " + enemy.name);

            enemy.GetComponent<PlayerHealth>().TakeDamage(10);
        }
    } 
    
    private void RangedAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Ranged Attack");

        GameObject p = Instantiate(projectile, this.transform.position, Quaternion.identity);

        Rigidbody2D pRB = p.GetComponent<Rigidbody2D>();

        pRB.velocity = new Vector2(1,0) * projectileSpeed;

        Debug.Log(pRB.velocity);

    }

    IEnumerator InputCheck(InputAction.CallbackContext context)
    {
        Debug.Log("coroutine running");

        yield return new WaitForSeconds(1);

        Debug.Log("isPressed: ", meleeAttack.IsPressed());

        if (context.started)
        {
            Debug.Log("started true");
        }

        if (context.performed)
        {
            Debug.Log("performed true");
        }

        if (context.canceled)
        {
            Debug.Log("canceled true");
        }
    }
}
