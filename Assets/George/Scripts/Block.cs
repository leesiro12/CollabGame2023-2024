using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private float elapsedTime = 0.0f;
    private float lifespan = 2.0f;

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > lifespan)
        {
            Destroy(gameObject);
        }
    }
}
