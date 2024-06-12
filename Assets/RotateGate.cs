using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGate : MonoBehaviour
{
    [SerializeField] private GameObject gateObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gateObject != null)
            {
                gateObject.transform.Rotate(new Vector3(0, 0, 1), -90f);

                Destroy(gameObject);
            }
        }
    }
}
