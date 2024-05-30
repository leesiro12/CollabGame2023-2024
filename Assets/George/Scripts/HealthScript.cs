using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HealthScript : MonoBehaviour
{
    // set up health and max health
    public int maxHealth = 5;
    public int playerHealth;

    // set up inputs that will be used
    public PlayerInputActions playerControls;
    private InputAction deflect;

    // how long the deflect should last
    private float activeTime = 1.0f;
    // how long between uses
    private float cooldown = 2.0f;
    // current time since last deflect
    private float timeSinceDeflect = 2.0f;
    // is the delfection active
    private bool deflectActive = false;

    // event to handle health change functions
    public delegate void OnHealthChange(int change);
    public static OnHealthChange onHealthChange;

    // event to handle player death functions
    public delegate void OnPlayerDeath();
    public static OnPlayerDeath onPlayerDeath;

    // activating the input and assigning it to the Deflect method
    private void OnEnable()
    {
        deflect = playerControls.Player.Deflect;
        deflect.Enable();
        deflect.performed += Deflect;
    }

    // deactivating the input
    private void OnDisable()
    {
        deflect.Disable();
    }

    // when the scirpt is first loaded
    void Awake()
    {
        // get reference to input action map
        playerControls = new PlayerInputActions();
        //set starting health
        playerHealth = maxHealth;
        // allows player to use deflect for the frist time
        timeSinceDeflect = cooldown;
    }

    private void Start()
    {
        // setup UI
        onHealthChange?.Invoke(playerHealth);
    }

    // each frame
    private void Update()
    {
        // if the cooldown time not yet reached and the ability isn't currently active
        if (timeSinceDeflect < cooldown && !deflectActive)
        {
            // continue to count cooldown time
            timeSinceDeflect += Time.deltaTime;
        }
    }

    // when input received
    private void Deflect(InputAction.CallbackContext context)
    {
        // has the cooldown time been completed
        if (timeSinceDeflect >= cooldown)
        {
            // prevent activation recurring
            timeSinceDeflect = 0.0f;
            // start deflection process
            StartCoroutine(deflection());
        }
    }

    // function to apply damage to player
    public void TakeDamage(int damage)
    {
        // will not take damage unless deflectActive is false
        if (!deflectActive && playerHealth > damage)
        {
            playerHealth -= damage;
            onHealthChange?.Invoke(playerHealth);
        }

        if (playerHealth <= damage)
        {
            onPlayerDeath?.Invoke();
        }
    }

    // functino to increase player health
    public void Heal(int amount)
    {
        if (playerHealth < maxHealth)
        {
            playerHealth += amount;
            onHealthChange?.Invoke(playerHealth);
        }
    }

    // coroutine to apply deflection ability
    IEnumerator deflection()
    {
        // value true so player can't take damage
        deflectActive = true;
        // delay by activeTime to allow invulnerability
        yield return new WaitForSeconds(activeTime);
        // value true so player can take damage
        deflectActive = false;
    }
}
