using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class dealDamage : MonoBehaviour
{
    private float cooldown = 1.5f;
    private float elapsedTime = 0.0f;
    private Coroutine coroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<HealthScript>().TakeDamage(10);
        coroutine = StartCoroutine(timedAttack(collision));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        elapsedTime = 0.0f;
        StopCoroutine(coroutine);
    }

    IEnumerator timedAttack(Collider2D collision)
    {
        while(true)
        {
            if (elapsedTime > cooldown)
            {
                elapsedTime = 0.0f;
                collision.GetComponent<HealthScript>().TakeDamage(10);
            }
            else
            {
                elapsedTime += Time.deltaTime;
            }
            yield return null;
        }
    }
}
