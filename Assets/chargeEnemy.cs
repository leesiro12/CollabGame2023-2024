using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chargeEnemy : MonoBehaviour
{
    private Rigidbody2D rb;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
