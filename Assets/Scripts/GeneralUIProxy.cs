using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GeneralUIProxy : MonoBehaviour, ButtonProxy.IClickHandler
{
    [Serializable]
    public class Slot
    {
        public Image SlotObj;
        public int id;
    }
    public List<Slot> artifactSlots;
    public List<Slot> itemSlots;
    public TextMeshProUGUI currency;

    public UIController controller;

    public Script_UI_Inv_Move inv;

    public GameObject exitButton;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void UpdateImagesOnSlots(Inventory i)
    {
        foreach (Slot s in artifactSlots)
        {
            if (i.items[s.id] == null)
            {
                s.SlotObj.enabled = false;
            }
            else
            {
                s.SlotObj.enabled = true;
                s.SlotObj.sprite = i.items[s.id].sprite;
            }
        }

        foreach (Slot s in itemSlots)
        {
            if (i.items[s.id] == null)
            {
                s.SlotObj.enabled = false;
            }
            else
            {
                s.SlotObj.enabled = true;
                s.SlotObj.sprite = i.items[s.id].sprite;
            }
        }
        currency.text = i.money.ToString();
    }

    public void onClick(GameObject obj)
    {
        if (obj == exitButton)
        {
            controller.quit();
            return;
        }

        int id = -1;
        foreach (Slot s in artifactSlots)
        {
            if (s.SlotObj.gameObject == obj)
            {
                id = s.id;
                break;
            }
        }

        foreach (Slot s in itemSlots)
        {
            if (s.SlotObj.gameObject == obj)
            {
                id = s.id;
                break;
            }
        }

        controller.handleInventoryClick(id, true);
    }

    public void retract()
    {
        inv.setState(false);
    }
    public void pullOut()
    {
        inv.setState(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
