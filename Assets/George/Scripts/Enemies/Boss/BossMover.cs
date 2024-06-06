using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossMover : MonoBehaviour
{
    // define movement positions and assocaited attacks
    [SerializeField] Transform[] movePositions;
    [SerializeReference] MonoBehaviour[] attackScripts;
    //[SerializeField] List<IBossAttack> bossAttacks;

    [SerializeField] float attackCooldown = 2;
    [SerializeField] float speed = 3;
    [SerializeField] bool canMove = true;

    // track movement
    [SerializeField] private int currentPointIndex = 0;

    //private void Awake()
    //{
    //    foreach (MonoBehaviour script in attackScripts)
    //    {
    //        if (script is IBossAttack)
    //        {
    //            bossAttacks.Add(script as IBossAttack);
    //        }
    //    }
    //}

    private void Update()
    {
        // if point reached
        if ((movePositions[currentPointIndex].position - transform.position).magnitude < 0.1f)
        {
            // call attack function
            if (currentPointIndex < attackScripts.Length && attackScripts[currentPointIndex] is IBossAttack)
            {
                (attackScripts[currentPointIndex] as IBossAttack).PerformAttack();
            }

            canMove = false;

            StartCoroutine(performCooldown());

            // update movement point
            if (currentPointIndex + 1 >= movePositions.Length)
            {
                currentPointIndex = 0;
            }
            else
            {
                currentPointIndex++;
            }
        }
        
        if(canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, movePositions[currentPointIndex].position, speed * Time.deltaTime);
        }
    }


    IEnumerator performCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canMove = true;
    }

    public void SetPhase(int phase)
    {
        if (phase == 1)
        {
            attackScripts[3] = attackScripts[0];
        }
    }
}
