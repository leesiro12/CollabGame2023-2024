using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HealthScript : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int playerHealth;


    public PlayerInputActions playerControls;
    private InputAction deflect;
    private float deflectCooldown = 2.0f;
    private float timeSinceDeflect = 2.0f;
    private bool deflectActive = false;

    private void OnEnable()
    {
        deflect = playerControls.Player.Deflect;
        deflect.Enable();
        deflect.performed += Deflect;
    }

    private void Deflect(InputAction.CallbackContext context)
    {
        if(timeSinceDeflect >= deflectCooldown)
        {
            timeSinceDeflect = 0;
            StartCoroutine(deflection());
        }
    }

    private void OnDisable()
    {
        deflect.Disable();
    }

    void Awake()
    {
        playerControls = new PlayerInputActions();
        playerHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (!deflectActive)
        {
            playerHealth -= damage;
        }
    }

    public void Heal(int amount)
    {
        playerHealth += amount;
    }

    IEnumerator deflection()
    {
        deflectActive = true;
        while(timeSinceDeflect < deflectCooldown)
        {
            yield return new WaitForSeconds(1.0f);
            timeSinceDeflect += 1.0f;
        }
        deflectActive = false;
        yield return null;
    }
}
