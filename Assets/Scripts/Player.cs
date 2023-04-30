using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 0.02f;
    public float rotSpeed = 0.02f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            transform.position -= transform.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            transform.position -= transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            transform.position += transform.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey("q"))
        {
            transform.rotation *= Quaternion.AngleAxis(-rotSpeed*Time.deltaTime, Vector3.up);
        }
        if (Input.GetKey("e"))
        {
            transform.rotation *= Quaternion.AngleAxis(rotSpeed * Time.deltaTime, Vector3.up);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tile")
            Debug.Log("EnteredPlayer");
    }
    public void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Tile")
            Debug.Log("LeftPlayer");
    }
}
