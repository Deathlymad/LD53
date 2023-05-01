using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WorldObjects/City")]
public class City : ScriptableObject
{
    public string cityName;
    public bool visited = false;
    public List<Artifact> items;
    public List<Dialogue> greetingFluff;
    
    public Dialogue GetRandomDialogue()
    {
        if (greetingFluff.Count == 0)
            return null;

        if (!visited)
            return greetingFluff[0];

        return greetingFluff[UnityEngine.Random.Range(1, greetingFluff.Count)];
    }

}
