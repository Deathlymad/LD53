using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class DialogueUIProxy : MonoBehaviour, ButtonProxy.IClickHandler
{
    public TextMeshProUGUI nameBox;
    public TextMeshProUGUI textBox;

    //TODO put text animation here
    public void SetDialogueName(string text)
    {
        nameBox.SetText(text);
    }
    public void SetDialogueText(string text)
    {
        textBox.SetText(text);
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
