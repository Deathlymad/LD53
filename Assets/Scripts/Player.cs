using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator playerAnim;

    public GameObject torchPrefab;
    public MapHandler map;

    private bool canMove = true;
    private GameObject item = null;
    public UIController uiController;


    //Called By Other Scripts
    //=======================================================
    public void lockMovement()
    {
        canMove = false;
    }
    public void unlockMovement()
    {
        canMove = true;
    }
    public void enterCity(GameObject city)
    {
        lockMovement();
        Cursor.lockState = CursorLockMode.Confined; //unlock mouse
        uiController.OnCityEnter(city, this.gameObject);
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
        playerAnim.ResetTrigger("Interact");
        //TODO add to inventory
        if (item != null)
        {
            Destroy(item);
        }
    }
    public void OnPlaceTorch()
    {
        GameObject obj = Instantiate(torchPrefab);

        if (map.getTileFromPosition(transform.position))
        {
            obj.transform.parent = map.getTileFromPosition(transform.position).transform;
            obj.transform.position = transform.position;
            obj.transform.localScale *= 0.5f;
            Invoke("unlockMovement", 17.0f / 24.0f); //counted by frames
        }
    }

    public bool IsMovable()
    {
        return canMove;
    }
    public void Walk()
    {
        playerAnim.ResetTrigger("StopRunning");
        playerAnim.SetTrigger("Running");
    }
    public void Stop()
    {
        playerAnim.ResetTrigger("Running");
        playerAnim.SetTrigger("StopRunning");
    }

    //Called By Unity
    //=======================================================
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (Input.GetKeyDown("e"))
            {
                if (item != null && item.GetComponent<Item>() != null)
                {
                    item.GetComponent<Item>().pickUp(this.gameObject);
                    Stop();
                    lockMovement();
                    playerAnim.SetTrigger("Interact");
                    Invoke("OnPickup", 17.0f / 24.0f); //counted by frames
                }
                else
                {
                    Stop();
                    lockMovement();
                    playerAnim.SetTrigger("Interact");
                    Invoke("OnPlaceTorch", 17.0f / 24.0f); //counted by frames
                }
            }
            
        }
        if (Input.GetKey("x"))
        {
            unlockMovement();
        }

    }
}
