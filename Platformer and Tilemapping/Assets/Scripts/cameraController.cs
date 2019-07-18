using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    //public Transform cameraTargetA;
    //public Transform cameraTargetB;
    public Transform Player;

    private Vector3 offset;

    void Start()
    {
        //ffset = transform.position - cameraTarget.transform.position;
        offset = transform.position - Player.transform.position;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        //transform.position = cameraTarget.transform.position + offset;

        transform.position = Player.transform.position + offset;
        //transform.position = offset;
    }
    

    /*
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Camera.main.transform.position = cameraTargetA.position;
        }

        else
        {
            Camera.main.transform.position = cameraTargetB.position;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Camera.main.transform.position = transform.position + new Vector3(0,0,-10);
    }
    */
}
