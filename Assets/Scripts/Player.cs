using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 0.02f;
    public float rotSpeed = 0.02f;
    public Animator playerAnim;

    private bool canMove = true;
    private GameObject item = null;


    //Called By Other Scripts
    //=======================================================
    public void enterCity()
    {
        canMove = false;
        //TODO open UI
    }
    public void leaveCity()
    {
        canMove = true;
    }
    public void nearItem(GameObject itm) //store items you can collect
    {
        item = itm;
    }
    public void leaveItem(GameObject itm) //drop item you can collect
    {
        if (item == itm)
            item = null;
    }


    //Called By Animations
    //=======================================================
    public void OnPickup()
    {
        //TODO add to inventory
        if (item != null)
        {
            Destroy(item);
        }
    }

    //Called By Unity
    //=======================================================
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {
        bool isDown = false;
        float turnFactor = 1.0f;
        if (canMove)
        {
            if (Input.GetAxis("Mouse X") > 0.01 || Input.GetAxis("Mouse X") < -0.01)
            {
                transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime, Vector3.up);
                isDown |= true;
                turnFactor = 0.8f;
            }
            else
            {
                turnFactor = 1.0f;
            }
            if (Input.GetKey("w"))
            {
                transform.position += transform.forward * turnFactor * moveSpeed * Time.deltaTime;
                isDown |= true;
            }
            if (Input.GetKey("a"))
            {
                transform.position -= transform.right * turnFactor * moveSpeed * Time.deltaTime;
                isDown |= true;
            }
            if (Input.GetKey("s"))
            {
                transform.position -= transform.forward * turnFactor * moveSpeed * Time.deltaTime;
                isDown |= true;
            }
            if (Input.GetKey("d"))
            {
                transform.position += transform.right * turnFactor * moveSpeed * Time.deltaTime;
                isDown |= true;
            }


            if (Input.GetKey("e") && item != null)
            {
                if (item.GetComponent<Item>() != null)
                {
                    item.GetComponent<Item>().pickUp(this.gameObject);
                    playerAnim.ResetTrigger("Running");
                    playerAnim.SetTrigger("StopRunning");
                    playerAnim.SetTrigger("Pickup");
                }
            }
        }
        if (Input.GetKey("x"))
        {
            leaveCity();
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
}
