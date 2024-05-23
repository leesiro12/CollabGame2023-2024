using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAttack : MonoBehaviour, IBossAttack
{
    CircleCollider2D detectionCollider;
    ContactFilter2D contactFilter;
    Collider2D[] results;
    //[SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform[] firePos;
    [SerializeField] GameObject bombPrefab;
    [SerializeField] float launchForce = 15;
    // Start is called before the first frame update
    void Start()
    {
        contactFilter.SetLayerMask(LayerMask.GetMask("Player"));
        detectionCollider = GetComponent<CircleCollider2D>();
        results = new Collider2D[1];
        
        //Debug.Log(firePos.transform.right);
    }

    

    public void PerformAttack()
    {
        if (detectionCollider != null)
        {
            if (Physics2D.OverlapCollider(detectionCollider, contactFilter, results) > 0)
            {
                for (int i = 0; i < firePos.Length; i++)
                {
                    Debug.Log("Bomb" + i + "spawning");
                    Vector3 launchDir = firePos[i].position;
                    Instantiate(bombPrefab, launchDir, Quaternion.identity);
                    bombPrefab.GetComponent<Rigidbody2D>().AddForce(launchDir * launchForce);
                }
            }
                
        }

        return;
            
    }
}
