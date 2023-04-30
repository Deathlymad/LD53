using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 0.02f;
    public float rotSpeed = 0.02f;
    public Animator playerAnim;

    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        bool isDown = false;

        if (Input.GetKey("w"))
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            isDown |= true;
        }
        if (Input.GetKey("a"))
        {
            transform.position -= transform.right * moveSpeed * Time.deltaTime;
            isDown |= true;
        }
        if (Input.GetKey("s"))
        {
            transform.position -= transform.forward * moveSpeed * Time.deltaTime;
            isDown |= true;
        }
        if (Input.GetKey("d"))
        {
            transform.position += transform.right * moveSpeed * Time.deltaTime;
            isDown |= true;
        }
        if (Input.GetKey("q"))
        {
            transform.rotation *= Quaternion.AngleAxis(-rotSpeed*Time.deltaTime, Vector3.up);
            isDown |= true;
        }
        if (Input.GetKey("e"))
        {
            transform.rotation *= Quaternion.AngleAxis(rotSpeed * Time.deltaTime, Vector3.up);
            isDown |= true;
        }

        if (isDown)
        {
            playerAnim.ResetTrigger("StopRunning");
            playerAnim.SetTrigger("Running");
        }
        else
        {
            playerAnim.ResetTrigger("Running");
            playerAnim.SetTrigger("StopRunning");
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
