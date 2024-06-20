using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDialogue : MonoBehaviour
{
    public GameObject oldNPC;
    public GameObject newNPC;

    private void Start()
    {
        if ( newNPC != null)
        {
            newNPC.gameObject.SetActive(false);
        }
    }

    public void switchDialogue()
    {
        oldNPC.gameObject.SetActive(false);
        newNPC.gameObject.SetActive(true);
    }
}
