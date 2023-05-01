using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject freeRoam;
    public GameObject general;
    public GameObject city;
    public GameObject dialogue;
    public GameObject tooltip;

    private int test = 0;

    // Start is called before the first frame update
    void Start()
    {
        setDialogue();
        freeRoam.SetActive(false);
        general.SetActive(false);
        city.SetActive(false);
        dialogue.SetActive(false);
        tooltip.SetActive(false);
    }

    void setFreeRoam()
    {
        freeRoam.SetActive(true);
        general.SetActive(true);
        city.SetActive(false);
        dialogue.SetActive(false);
        tooltip.SetActive(false);
    }
    void setCity()
    {
        freeRoam.SetActive(false);
        general.SetActive(true);
        city.SetActive(true);
        dialogue.SetActive(false);
        tooltip.SetActive(false);
    }
    void setDialogue()
    {
        freeRoam.SetActive(false);
        general.SetActive(true);
        city.SetActive(false);
        dialogue.SetActive(true);
        tooltip.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            test += 1;
            test = test % 3;
            switch (test)
            {
                case 0:
                    setFreeRoam();
                    break;
                case 1:
                    setCity();
                    break;
                case 2:
                    setDialogue();
                    break;
            }
        }
    }
}
