using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MagicBlade : MonoBehaviour, IBossAttack
{
    CircleCollider2D detectionCollider;
    Collider2D[] results;
    ContactFilter2D contactFilter;

    [SerializeField] Transform firePos;
    [SerializeField] float attackRange = 1.5f;

    private void Start()
    {
        contactFilter.SetLayerMask(LayerMask.GetMask("Player"));
        detectionCollider = GetComponent<CircleCollider2D>();
        results = new Collider2D[1];
    }

    public void PerformAttack()
    {
        StartCoroutine(StartAttack());

        return;
    }

    private bool Teleport()
    {
        results[0] = null;

        if (detectionCollider != null)
        {
            if (Physics2D.OverlapCollider(detectionCollider, contactFilter, results) > 0)
            {
                if (transform.position.x < results[0].transform.position.x)
                {
                    transform.position = new Vector3(results[0].transform.position.x + 1.2f, results[0].transform.position.y, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(results[0].transform.position.x - 1.2f, results[0].transform.position.y, transform.position.z);
                }

                // play appear animation

                return true;
            }
        }

        return false;
    }

    private void MeleeAttack()
    {
        if (results[0] != null)
        {
            firePos.LookAt(results[0].transform.position, transform.forward);
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(firePos.GetChild(0).position, attackRange);
            foreach (Collider2D hitCollider in hitColliders)
            {
                HealthScript healthScript = hitCollider.GetComponent<HealthScript>();
                if (healthScript)
                {
                    healthScript.TakeDamage(1);
                }
            }
        }
    }

    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(0.5f); //play disapear animation

        gameObject.GetComponent<Renderer>().enabled = false;

        if (!Teleport())
        {
            gameObject.GetComponent<Renderer>().enabled = true;
            yield break;
        }

        yield return new WaitForSeconds(0.5f);

        gameObject.GetComponent<Renderer>().enabled = true;

        yield return new WaitForSeconds(0.5f);

        MeleeAttack();
    }
}
