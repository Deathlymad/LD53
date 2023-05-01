using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButtonProxy : MonoBehaviour
{
    public interface IClickHandler
    {
        public void onClick(GameObject obj);
    }

    public GameObject handler;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;

            var rectTransform = GetComponent<RectTransform>();
            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;

            if (mousePos.x > transform.position.x - width && mousePos.x < transform.position.x &&
                mousePos.y > transform.position.y && mousePos.y < transform.position.y + height)
            {
                handler.GetComponent<IClickHandler>().onClick(this.gameObject);
            }
        }
    }

}
