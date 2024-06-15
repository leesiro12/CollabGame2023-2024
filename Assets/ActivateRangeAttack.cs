using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRangeAttack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<AttackScript>() != null)
        {
            other.GetComponent<AttackScript>().UnlockRangedAttack();
        }
    }
}
