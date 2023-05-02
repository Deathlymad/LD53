using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WorldObjects/City")]
public class City : ScriptableObject
{
    public string cityName;
    public Sprite cityMascot;
    public bool visited = false;
    public Artifact startArtifact;
    public List<Artifact> acceptedArtifacts;
    public List<Dialogue> greetingFluff;
    
    public void init()
    {
        visited = false;
    }

    public Dialogue GetRandomDialogue()
    {
        if (greetingFluff.Count == 0)
            return null;

        if (!visited)
            return greetingFluff[0];

        return greetingFluff[UnityEngine.Random.Range(1, greetingFluff.Count)];
    }

}
