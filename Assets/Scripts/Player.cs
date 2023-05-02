using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator playerAnim;
    public PlayerTorchHandler bearer;

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
        if (bearer != null)
            bearer.PauseAnimation();
    }
    public void unlockMovement()
    {
        canMove = true;
        bearer.PlayAnimation();
    }
    public void enterCity(GameObject city)
    {
        lockMovement();
        Cursor.lockState = CursorLockMode.Confined; //unlock mouse
        uiController.OnCityEnter(city);

        int j = -1;
        for (int i = 0; i < GetComponent<Inventory>().items.Length; i++)
        {
            if (GetComponent<Inventory>().items[i] == null)
                continue;
            if (GetComponent<Inventory>().items[i].target.name == city.GetComponent<MissionProvider>().selfCity.name)
                j = i;
        }
        if (j >= 0)
        {
            GetComponent<Inventory>().items[j] = null;
        }

        bearer.obj.torches = 9;
        uiController.setTorchCount(bearer.obj.torches);
        bearer.ForceUpdate();
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

    public void useItem(int id) { } //TODO

    //Called By Animations
    //=======================================================
    public void OnPickup()
    {
        playerAnim.ResetTrigger("Interact");
        unlockMovement();
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
        if (bearer.obj.torches > 0)
        {
            bearer.obj.torches -= 1;
            uiController.setTorchCount(bearer.obj.torches);
            bearer.ForceUpdate();
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
