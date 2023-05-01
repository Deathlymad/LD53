using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WorldObjects/Artifact")]
public class Artifact : ScriptableObject
{
    public string artifactName;
    public Sprite sprite;
    public City target;
    public List<string> descriptionFluff;

}
