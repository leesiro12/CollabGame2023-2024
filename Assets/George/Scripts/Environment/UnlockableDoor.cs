using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // find inventory script on player
        InventoryCheck inventoryScript = collision.GetComponent<InventoryCheck>();
        if (inventoryScript != null)
        {
            // check if key has been picked up
            if(inventoryScript.GetKeyStatus())
            {
                // open door
                transform.Rotate(0, 0, 90);
                // destroy script - stop functionality
                Destroy(this);
            }
        }
    }
}
