using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : EnemyHealth
{
    // event to handle boss death functions
    public delegate void OnBossDeath();
    public OnBossDeath onBossDeath;

    // Update is called once per frame
    protected override void Death()
    {
        if (currentHealth <= 0)
        {
            onBossDeath?.Invoke();
        }
    }
}
