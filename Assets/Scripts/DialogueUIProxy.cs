using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class DialogueUIProxy : MonoBehaviour, ButtonProxy.IClickHandler
{
    public Image img;
    public TextMeshProUGUI nameBox;
    public TextMeshProUGUI textBox;

    //TODO put text animation here
    public void SetDialogue(Dialogue.Line text)
    {
        nameBox.SetText(text.speaker);
        textBox.SetText(text.text);
        if (text.speakerImg != null)
        {
            Debug.Log("Tesxt");
            img.sprite = text.speakerImg;
        }
    }

    public void onClick(GameObject obj)
    {
        transform.parent.gameObject.GetComponent<UIController>().CompleteDialogue();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
