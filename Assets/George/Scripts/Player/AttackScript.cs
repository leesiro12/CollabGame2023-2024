using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static UnityEditor.Timeline.TimelinePlaybackControls;


public class AttackScript : MonoBehaviour
{
    private Rigidbody2D rb;

    // where to search from on attack
    public Transform attackPoint;
    // how far to search
    public float attackRange = 1.5f;
    // which layers to search
    public LayerMask targetLayers;

    // attack damage for light and heavy attacks
    [SerializeField] int lightDamage = 10;
    [SerializeField] int heavyDamage = 20;

    // define how quickly the player can apply heavy attack (keep as multiple of 0.1)
    private float timeForHeavy = 0.5f;

    // to hold references to input actions and map
    public PlayerInputActions playerControls;
    private InputAction meleeAttack;
    private InputAction rangedAttack;

    public static bool canRangeAttack = false;

    // prefab of projectile
    public GameObject projectile;
    // speed at which we will instantiate the projectile
    [SerializeField] private float projectileSpeed = 15.0f;
    public Animator attackAnim;
    private void Awake()
    {
        // get reference to input map
        playerControls = new PlayerInputActions();
        // get reference to object's rb
        rb = GetComponent<Rigidbody2D>();
    }

    // activating the inputs and assigning them to the appropriate methods
    private void OnEnable()
    {
        meleeAttack = playerControls.Player.Attack;
        meleeAttack.Enable();
        meleeAttack.performed += MeleeInput;

        rangedAttack = playerControls.Player.RangedAttack;
        rangedAttack.Enable();
        if (canRangeAttack)
        {
            rangedAttack.performed += RangedAttackInput;
        }
    }

    // deactivating the input
    private void OnDisable()
    {
        meleeAttack.Disable();
        rangedAttack.Disable();
    }

    // when input received
    private void MeleeInput(InputAction.CallbackContext context)
    {
        // start coroutine to check for input hold duration
        StartCoroutine(InputCheck(context));
    }

    // when attack input received
    private void MeleeAttack(bool attackIsLight)
    {
        // play attack animation

        if (GetComponent<SimpleMovement>().m_attack == false )
        {
            StartCoroutine(MeleeAttackAnim());
        }

        // detect enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, targetLayers);
        Debug.DrawLine(attackPoint.position, new Vector3(attackPoint.position.x + attackRange, attackPoint.position.y, attackPoint.position.z), Color.red, 5);

        // damage enemies
        foreach (Collider2D enemy in hitEnemies)
        {

            if (enemy.isTrigger)
            {
                break;
            }

            if (enemy.GetComponent<EnemyHealth>())
            {
                if ((transform.position - enemy.transform.position).magnitude <= attackRange)
                {
                    switch (attackIsLight)
                    {
                        // true refers to light attack
                        case true:
                            enemy.GetComponent<EnemyHealth>().TakeDamage(lightDamage);
                            break;
                        // false refers to a heavy attack
                        case false:
                            enemy.GetComponent<EnemyHealth>().TakeDamage(heavyDamage);
                            break;
                    }
                }
            }
        }
    }
    
    // when ranged attack input is received
    private void RangedAttackInput(InputAction.CallbackContext context)
    {
        // run the Ranged Attack method
        RangedAttack();
    }

    // fire a projectile
    private void RangedAttack()
    {
        if (GetComponent<SimpleMovement>().m_attack == false)
        {
            StartCoroutine(RangedAttackAnim());
        }
        // create an instance of the projectile prefab
        GameObject p = Instantiate(projectile, transform.position, Quaternion.identity);
        // get the object's rb
        Rigidbody2D rbP = p.GetComponent<Rigidbody2D>();
        // give the rb a velocity based on the projectile speed and local scale (direction)
        rbP.velocity = new Vector2(1, 0) * projectileSpeed * rb.transform.localScale;
    }

    public void UnlockRangedAttack()
    {
        if (!canRangeAttack)
        {
            rangedAttack.performed += RangedAttackInput;
            canRangeAttack = true;
        }
    }

    // checks for held down input
    IEnumerator InputCheck(InputAction.CallbackContext context)
    {
        // loop each 0.1 seconds, until heacy attack time is reached
        for (int i = 0; i < (timeForHeavy/0.1f); i++)
        {
            // if button has been released
            if (!meleeAttack.IsPressed())
            {
                // run script for light damage
                MeleeAttack(true);
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }

        // if for loop complete then run script for heavy attack
        MeleeAttack(false);

        yield break;
    }

    IEnumerator MeleeAttackAnim()
    {
        Debug.Log("Attacking");
        GetComponent<SimpleMovement>().m_attack = true;
        attackAnim.Play("Melee");
        yield return new WaitForSeconds(0.5f);
        GetComponent<SimpleMovement>().m_attack = false;
        yield break;
    }

    IEnumerator RangedAttackAnim()
    {
        Debug.Log("Attacking");
        GetComponent<SimpleMovement>().m_attack = true;
        attackAnim.Play("RangedAttack");
        yield return new WaitForSeconds(0.5f);
        GetComponent<SimpleMovement>().m_attack = false;
        yield break;
    }

}
