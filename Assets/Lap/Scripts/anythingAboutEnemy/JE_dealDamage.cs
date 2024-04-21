using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    bool isAttacking = false;
    float AtkLenght;
    public HealthScript HSRef;
    public GameObject damageZonePrefab;

    void StartAtkMove(int damage)
    {
        Instantiate(damageZonePrefab,transform.position,Quaternion.identity);
        HSRef.TakeDamage(damage);
    }

}
