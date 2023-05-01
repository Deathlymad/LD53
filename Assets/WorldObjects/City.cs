using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WorldObjects/City")]
public class City : ScriptableObject
{
    public string cityName;
    public List<Artifact> items;
    public List<string> greetingFluff;
    
    public string GetRandomGreeting()
    {
        if (greetingFluff.Count == 0)
            return "";

        return greetingFluff[UnityEngine.Random.Range(0, greetingFluff.Count)];
    }

}
