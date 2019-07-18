using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMover : MonoBehaviour
{
    public float speed;
    public Vector3 waypointOne;
    public Vector3 waypointTwo;
    
    void Update()
    {
        transform.position = Vector3.Lerp (waypointOne, waypointTwo, Mathf.PingPong(Time.time*speed, 1.0f));
    }
}
