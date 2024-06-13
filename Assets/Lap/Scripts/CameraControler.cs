using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public Transform target;
    public bool positionIsFixed;
    public float zoomMultiplier;

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if (positionIsFixed)
        {
            return;
        }


        Vector3 pos = target.position;
        pos.z = -1; // lock the z asix.
        transform.position = pos;
    }

    public void SetFixedPosition(Transform cameraPos)
    {
        positionIsFixed = true;
        transform.position = cameraPos.position;
        this.GetComponent<Camera>().orthographicSize *= zoomMultiplier;
    }

    public void ReleaseFixedPosition()
    {
        positionIsFixed = false;
        this.GetComponent<Camera>().orthographicSize /= zoomMultiplier;
    }
}
