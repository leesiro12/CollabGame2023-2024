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

    public int lightDamage = 10;
    public int heavyDamage = 20;

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
        meleeAttack.performed += MeleeInput;

        rangedAttack = playerControls.Player.RangedAttack;
        rangedAttack.Enable();
        rangedAttack.performed += RangedAttackInput;
    }

    private void OnDisable()
    {
        meleeAttack.Disable();
        rangedAttack.Disable();
    }

    private void MeleeInput(InputAction.CallbackContext context)
    {
        Debug.Log("Attack");

        StartCoroutine(InputCheck(context));
    }

    private void MeleeAttack(bool attackIsLight)
    {
        // play attack animation
        // 

        // detect enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, targetLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            if(enemy.CompareTag("Block") == true)
            {
                return;
            }
        }

        // damage enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            switch (attackIsLight)
            {
            case true:
                enemy.GetComponent<PlayerHealth>().TakeDamage(lightDamage);
                break;
            case false:
                enemy.GetComponent<PlayerHealth>().TakeDamage(heavyDamage);
                break;
            }
        }
    }
    
    private void RangedAttackInput(InputAction.CallbackContext context)
    {
        RangedAttack();
    }

    private void RangedAttack()
    {
        GameObject p = Instantiate(projectile, transform.position, Quaternion.identity);

        Rigidbody2D pRB = p.GetComponent<Rigidbody2D>();

        pRB.velocity = new Vector2(1, 0) * projectileSpeed;
    }


    IEnumerator InputCheck(InputAction.CallbackContext context)
    {
        for (int i = 0; i < 6; i++)
        {
            if (!meleeAttack.IsPressed())
            {
                MeleeAttack(true);
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }

        MeleeAttack(false);
        yield break;
    }
}
