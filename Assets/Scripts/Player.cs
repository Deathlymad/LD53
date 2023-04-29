using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 0.02f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            transform.position += transform.forward * moveSpeed;
        }
        if (Input.GetKey("a"))
        {
            transform.position -= transform.right * moveSpeed;
        }
        if (Input.GetKey("s"))
        {
            transform.position -= transform.forward * moveSpeed;
        }
        if (Input.GetKey("d"))
        {
            transform.position += transform.right * moveSpeed;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("EnteredPlayer");
    }
    public void OnTriggerExit(Collider collision)
    {
        Debug.Log("LeftPlayer");
    }
}
