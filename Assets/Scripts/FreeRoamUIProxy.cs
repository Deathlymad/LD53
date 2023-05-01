using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class FreeRoamUIProxy : MonoBehaviour
{
    public RectTransform slider;
    public List<Image> torches;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void setTorchCount(int i)
    {
        foreach(var v in torches)
        {
            if (i > 0)
            {
                v.enabled = true;
            }
            else
                v.enabled = false;
            i--;
        }
    }

    public void setTorchLevel(float i)
    {
        slider.sizeDelta = new Vector2(-(131 - 250 * i), slider.sizeDelta.y);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
