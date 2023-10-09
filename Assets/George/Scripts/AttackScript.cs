using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackScript : MonoBehaviour
{
    public Rigidbody2D rb;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public PlayerInputActions playerControls;
    private InputAction meleeAttack;

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
    }

    private void OnDisable()
    {
        meleeAttack.Disable();
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

        // play attack animation
        //

        // detect enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // damage enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("enemy hit: " + enemy.name);
        }
    }
}
