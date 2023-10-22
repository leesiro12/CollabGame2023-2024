using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public Transform Target;
    public Vector3 offset;
    public float damping;

    private Vector3 velocity = Vector3.zero;

    private void FixedUpdate()
    {
        Vector3 MovePosition = Target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, MovePosition, ref velocity, damping);
    }
}
