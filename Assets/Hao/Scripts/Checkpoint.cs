using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // update check point and highlight check point marker
            PlayerManager.lastCheckPointPos = transform.position;
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
