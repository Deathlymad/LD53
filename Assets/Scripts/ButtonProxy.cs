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

            if (mousePos.x > transform.position.x - width / 2 && mousePos.x < transform.position.x + width / 2 &&
                mousePos.y > transform.position.y - height / 2 && mousePos.y < transform.position.y + height / 2)
            {
                handler.GetComponent<IClickHandler>().onClick(this.gameObject);
            }
        }
    }

}
