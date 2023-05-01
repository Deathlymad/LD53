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

    private GameObject currentCity;
    private MissionProvider cityProvider;

    private GameObject player;
    private Player playerController;
    private MissionReceiver playerReceiver;

    private Mission mission;

    private Dialogue currentDialogue;
    private int dialogueState = -1;

    // Start is called before the first frame update
    void Start()
    {
        setFreeRoam();
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

    public void OnCityEnter(GameObject city, GameObject player)
    {
        currentCity = city;
        cityProvider = currentCity.GetComponent<MissionProvider>();
        this.player = player;
        playerController = player.GetComponent<Player>();
        playerReceiver = player.GetComponent<MissionReceiver>();
        setDialogue();

        if (currentCity != null)
        {
            currentDialogue = cityProvider.selfCity.GetRandomDialogue();
            if (currentDialogue != null)
                setupCityDialogue(0);
        }
    }

    void setupCityDialogue(int id)
    {
        Dialogue.Line diag = currentDialogue.GetLine(id);
        dialogue.GetComponent<DialogueUIProxy>().SetDialogue(diag);
        dialogueState = diag.GetNextLine();
    }

    public void CompleteDialogue()
    {
        if (dialogueState >= 0)
            setupCityDialogue(dialogueState);
        else
        {
            setCity();
        }
        //TODO continue;
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
