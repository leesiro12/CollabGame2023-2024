using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossMover : MonoBehaviour
{
    // define movement positions and assocaited attacks
    [SerializeField] Transform[] movePositions;
    [SerializeReference] MonoBehaviour[] attackScripts;

    [SerializeField] float attackCooldown = 2;
    [SerializeField] float speed = 3;
    [SerializeField] bool canMove = true;

    // track movement
    [SerializeField] private int currentPointIndex = 0;

    private void Update()
    {
        if (canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, movePositions[currentPointIndex].position, speed * Time.deltaTime);

            // if point reached
            if ((movePositions[currentPointIndex].position - transform.position).magnitude < 0.1f)
            {
                transform.position = movePositions[currentPointIndex].position;

                // call attack function
                if (currentPointIndex < attackScripts.Length && attackScripts[currentPointIndex] is IBossAttack)
                {
                    (attackScripts[currentPointIndex] as IBossAttack).PerformAttack();
                }

                canMove = false;

                StartCoroutine(performCooldown(attackCooldown));

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
        }

       

        //if (canMove)
        //{
        //    transform.position = Vector2.MoveTowards(transform.position, movePositions[currentPointIndex].position, speed * Time.deltaTime);
        //}
    }

    public void delayMovement(float cooldownTime)
    {
        canMove = false;
        performCooldown(cooldownTime);
    }

    IEnumerator performCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canMove = true;
    }
}