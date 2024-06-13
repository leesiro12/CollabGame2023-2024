using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMisile : MonoBehaviour, IBossAttack
{
    CircleCollider2D detectionCollider;
    Collider2D[] results;
    ContactFilter2D contactFilter;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePos;
    [SerializeField] float projectileSpeed = 20;
    public Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        contactFilter.SetLayerMask(LayerMask.GetMask("Player"));
        detectionCollider = GetComponent<CircleCollider2D>();
        results = new Collider2D[1];

        Debug.Log(firePos.transform.right);
    }

    public void PerformAttack()
    {
        anim.Play("FireBallAttackAnim");
        if (detectionCollider != null)
        {
            if (Physics2D.OverlapCollider(detectionCollider, contactFilter, results) > 0)
            {
                if (results[0].gameObject.GetComponent<SimpleMovement>() != null)
                {
                    firePos.LookAt(results[0].transform.position, transform.forward);

                    for (int i = 0; i < firePos.childCount; i++)
                    {
                        GameObject spawnedProjectile = Instantiate(projectilePrefab, firePos.GetChild(i).position, firePos.GetChild(i).rotation * Quaternion.Euler(0,0,90));
                        spawnedProjectile.GetComponent<Rigidbody2D>().velocity = spawnedProjectile.transform.right * projectileSpeed;
                    }
                }
            }
        }

        return;
    }
}
