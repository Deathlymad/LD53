using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CityUIProxy : MonoBehaviour, ButtonProxy.IClickHandler
{
    [Serializable]
    public class Slot
    {
        public Image SlotObj;
        public int id;
    }
    public List<Slot> artifactSlots;
    public List<Slot> itemSlots;
    public Image keepContainer;

    public UIController controller;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setShopImage(Sprite s)
    {
        keepContainer.sprite = s;
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
    }

    public void onClick(GameObject obj)
    {
        int id = -1;
        foreach (Slot s in artifactSlots)
        {
            if (s.SlotObj.gameObject == obj)
                id = s.id;
        }

        foreach (Slot s in itemSlots)
        {
            if (s.SlotObj.gameObject == obj)
                id = s.id;
        }
        if (id != -1)
            controller.handleInventoryClick(id, false);
        else
            Debug.Log(id);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
