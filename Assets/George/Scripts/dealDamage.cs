using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class DealDamage : MonoBehaviour
{
    // defines time between attacks
    private float cooldown = 1.5f;
    // will hold reference to coroutine
    private Coroutine coroutine;

    // when overlap begins
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // start coroutine for timed attack and hold reference in coroutine variable
        coroutine = StartCoroutine(timedAttack(collision));
    }

    // when overlap ends
    private void OnTriggerExit2D(Collider2D collision)
    {
        // stop the coroutine form running
        StopCoroutine(coroutine);
    }

    // apply damage at regular interval
    IEnumerator timedAttack(Collider2D collision)
    {
        // keep looping while routine is active
        while(true)
        {
            // apply damage
            collision.GetComponent<HealthScript>().TakeDamage(10);
            // delay
            yield return new WaitForSeconds(cooldown);
        }
    }
}
