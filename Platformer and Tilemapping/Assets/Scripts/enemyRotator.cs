using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyRotator : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(0,0,150) * Time.deltaTime);
    }
}
