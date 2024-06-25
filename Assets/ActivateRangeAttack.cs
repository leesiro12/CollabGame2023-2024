using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRangeAttack : MonoBehaviour
{
    public void Activate(SimpleMovement playerMovement)
    {
        if (playerMovement.transform.GetComponent<AttackScript>() != null)
        {
            playerMovement.transform.GetComponent<AttackScript>().UnlockRangedAttack();
        }
    }
}
