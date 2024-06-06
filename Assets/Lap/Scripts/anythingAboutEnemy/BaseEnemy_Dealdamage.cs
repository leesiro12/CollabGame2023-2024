using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy_Dealdamage : MonoBehaviour
{

    private bool knockRight;
    private int KBForce = 3;
    
    // Start is called before the first frame update
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (collision.gameObject.GetComponent<PlayerHealth>())
        {
            if (this.transform.position.x < collision.transform.position.x)
            {
                playerRb.velocity = new Vector2(-KBForce, KBForce);
            }
            if (this.transform.position.x > collision.transform.position.x)
            {
                playerRb.velocity = new Vector2(KBForce, KBForce);
            }
        }
    }
}
