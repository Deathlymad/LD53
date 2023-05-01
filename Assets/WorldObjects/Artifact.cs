using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WorldObjects/City")]
public class Artifact : ScriptableObject
{
    public string name;
    public City target;
    public List<string> descriptionFluff;

}
