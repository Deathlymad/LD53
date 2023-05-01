using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WorldObjects/City")]
public class City : ScriptableObject
{
    public string cityName;
    public List<Artifact> items;
    public List<Dialogue> greetingFluff;
    
    public Dialogue GetRandomDialogue()
    {
        if (greetingFluff.Count == 0)
            return null;

        return greetingFluff[UnityEngine.Random.Range(0, greetingFluff.Count)];
    }

}
