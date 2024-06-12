using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCheck : MonoBehaviour
{
    // track if key is held
    public static bool hasKey = false;

    // check value
    public bool GetKeyStatus()
    {
        return hasKey;
    }

    // change value
    public void SetKeyStatus(bool value)
    {
        hasKey = value;
    }
}
