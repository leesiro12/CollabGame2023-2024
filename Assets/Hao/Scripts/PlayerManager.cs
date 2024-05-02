using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // the default check point is the starting position of the current level
    public static Vector2 defaultCheckPointPos = new Vector2(-19, -22);

    // the last check point is the position of the latest red marker that player touches
    public static Vector2 lastCheckPointPos = new Vector2(-19, -22);

    private void Awake()
    {
        // set player position to the last check point
        GameObject.FindGameObjectWithTag("Player").transform.position = lastCheckPointPos;
    }
}
