using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int itemSize = 2;
    public int artifactSize = 10;
    public int money = 200;

    public Artifact[] items;
    public Artifact[] artifacts;

    // Start is called before the first frame update
    void Start()
    {
        items = new Artifact[itemSize];
        artifacts = new Artifact[artifactSize];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
