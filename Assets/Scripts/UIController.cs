using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject freeRoam;
    public GameObject general;
    public GameObject city;
    public GameObject dialogue;
    public GameObject tooltip;

    public enum UIState
    {
        FreeRoam,
        City,
        Dialogue
    }
    private UIState currState = UIState.FreeRoam;

    private GameObject currentCity;
    private MissionProvider cityProvider;

    public GameObject player;
    private Player playerController;
    private MissionReceiver playerReceiver;

    private Mission mission;

    private Dialogue currentDialogue;
    private int dialogueState = -1;

    // Start is called before the first frame update
    void Start()
    {
        setFreeRoam();
        Invoke("setupPlayer", 1.0f);
    }

    void setupPlayer()
    {
        playerController = player.GetComponent<Player>();
        playerReceiver = player.GetComponent<MissionReceiver>();
        general.GetComponent<GeneralUIProxy>().UpdateImagesOnSlots(player.GetComponent<Inventory>());
    }

    void setFreeRoam()
    {
        freeRoam.SetActive(true);
        general.SetActive(true);
        city.SetActive(false);
        dialogue.SetActive(false);
        tooltip.SetActive(false);
        currState = UIState.FreeRoam;
        general.GetComponent<GeneralUIProxy>().retract();
    }
    void setCity()
    {
        freeRoam.SetActive(false);
        general.SetActive(true);
        city.SetActive(true);
        dialogue.SetActive(false);
        tooltip.SetActive(false);
        currState = UIState.City;

        if (currentCity != null)
        {
            city.GetComponent<CityUIProxy>().setShopImage(cityProvider.selfCity.cityMascot);
            city.GetComponent<CityUIProxy>().UpdateImagesOnSlots(currentCity.GetComponent<Inventory>());
        }

        general.GetComponent<GeneralUIProxy>().pullOut();
    }
    void setDialogue()
    {
        freeRoam.SetActive(false);
        general.SetActive(true);
        city.SetActive(false);
        dialogue.SetActive(true);
        tooltip.SetActive(false);
        currState = UIState.Dialogue;
        general.GetComponent<GeneralUIProxy>().retract();
    }

    public void OnCityEnter(GameObject city)
    {
        currentCity = city;
        cityProvider = currentCity.GetComponent<MissionProvider>();
        setDialogue();
        if (currentCity != null)
        {
            if (!cityProvider.selfCity.visited)
                currentCity.GetComponent<Inventory>().SetItem(0, cityProvider.selfCity.startArtifact);


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
        Debug.Log(dialogueState);
        if (dialogueState >= 0)
            setupCityDialogue(dialogueState);
        else
        {
            setCity();
            cityProvider.selfCity.visited = true;
        }
    }

    public void handleInventoryClick(int id, bool p)
    {
        if (currState != UIState.City)
        {
            if (player)
                throw new ArgumentException("Can't deal with CityUI without it being active");

            playerController.useItem(id);
        }
        else
        {
            if (p)
            {
                //Player to City
                Debug.Log(id);
                Artifact a = player.GetComponent<Inventory>().items[id];
                if (a == null)
                    return;
                player.GetComponent<Inventory>().items[id] = null;
                bool hasTransferred = false;
                for (int i = 0; i < currentCity.GetComponent<Inventory>().items.Length; i++)
                    if (currentCity.GetComponent<Inventory>().items[i] == null)
                    {
                        currentCity.GetComponent<Inventory>().SetItem(i, a);
                        hasTransferred = true;
                        break;
                    }
                if (hasTransferred)
                {
                    if (cityProvider.selfCity.name == a.target.name)
                    {
                        currentDialogue = a.sellDialogue;
                        player.GetComponent<Inventory>().resolveArtifactTransaction(a, false);
                    }
                    else
                    {
                        currentDialogue = a.wrongSellDialogue;
                    }
                    if (currentDialogue != null)
                        setupCityDialogue(0);
                }
                else
                {
                    player.GetComponent<Inventory>().SetItem(id, a);
                }
            }
            else
            {
                //City to Player
                Debug.Log(id);
                Artifact a = currentCity.GetComponent<Inventory>().items[id];
                if (a == null)
                    return;
                currentCity.GetComponent<Inventory>().items[id] = null;
                bool hasTransferred = false;
                for (int i = 0; i < player.GetComponent<Inventory>().items.Length; i++)
                    if (player.GetComponent<Inventory>().items[i] == null)
                    {
                        player.GetComponent<Inventory>().SetItem(i, a);
                        hasTransferred = true;
                        break;
                    }
                if (hasTransferred)
                {
                    player.GetComponent<Inventory>().resolveArtifactTransaction(a, true);
                    currentDialogue = a.purchaseDialogue;
                    if (currentDialogue != null)
                        setupCityDialogue(0);
                }
                else
                {
                    currentCity.GetComponent<Inventory>().SetItem(id, a);
                }
            }
        }

        //update inventory rendering
        if (currentCity != null)
            city.GetComponent<CityUIProxy>().UpdateImagesOnSlots(currentCity.GetComponent<Inventory>());
        if (player != null)
            general.GetComponent<GeneralUIProxy>().UpdateImagesOnSlots(player.GetComponent<Inventory>());
    }

    public void quit()
    {
        if (currState == UIState.City || currState == UIState.Dialogue)
        {
            setFreeRoam();
            playerController.unlockMovement();
            currentCity = null;
            currentDialogue = null;
            cityProvider = null;
            dialogueState = -1;
        }
        else
        {
            SceneManager.LoadScene("LoseState");
        }
    }

    public void setTorchCount(int i)
    {
        if (i < 0)
            SceneManager.LoadScene("LoseState");
        freeRoam.GetComponent<FreeRoamUIProxy>().setTorchCount(i);
    }

    public void setTorchLevel(float i)
    {
        freeRoam.GetComponent<FreeRoamUIProxy>().setTorchLevel(i);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
