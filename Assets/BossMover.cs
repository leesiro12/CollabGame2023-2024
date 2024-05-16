using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossMover : MonoBehaviour
{
    // define movement positions and assocaited attacks
    [SerializeField] Transform[] movePositions;
    [SerializeReference] IBossAttack[] attackScripts;

    [SerializeField] float attackCooldown = 2;
    [SerializeField] float speed = 3;
    [SerializeField] bool canMove = true;

    // track movement
    [SerializeField] private int currentPointIndex = 0;

    private void Update()
    {
        // if point reached
        if ((movePositions[currentPointIndex].position - transform.position).magnitude < 0.1f)
        {
            // call attack function
            if (currentPointIndex < attackScripts.Length && attackScripts[currentPointIndex] is IBossAttack)
            {
                attackScripts[currentPointIndex].PerformAttack();
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
            transform.position = Vector2.MoveTowards(transform.position, movePositions[currentPointIndex].position, speed * 0.01f);
        }
    }


    IEnumerator performCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canMove = true;
    }
}
