using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MAudioManager.instance.PlayMusic("BGM1");
    }

    // Update is called once per frame
    void Update()
    {
        MAudioManager.instance.PlaySFX("Jump");
    }
}
