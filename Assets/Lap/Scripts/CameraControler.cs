using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        Vector3 pos = transform.position;
        pos.z = -1; // lock the z asix.
        transform.position = pos;
    }
    private void FixedUpdate()
    {
        this.transform.position = target.position;
    }
}
