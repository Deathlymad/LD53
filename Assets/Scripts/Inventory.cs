using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int itemSize = 2;
    public int money = 200;

    public Artifact[] items;

    public void resolveArtifactTransaction(Artifact a, bool direction) //to => true
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        items = new Artifact[itemSize];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
