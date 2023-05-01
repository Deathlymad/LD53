using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WorldObjects/City")]
public class City : ScriptableObject
{
    public string cityName;
    public List<Artifact> items;
    public List<string> greetingFluff;
    
}
