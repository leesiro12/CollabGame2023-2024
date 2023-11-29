using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // find inventory script on player
        InventoryCheck inventoryScript = collision.GetComponent<InventoryCheck>();

        if (inventoryScript != null)
        {
            // set status to show key held
            inventoryScript.SetKeyStatus(true);
            // destroy key
            Destroy(gameObject);
        }
    }
}
