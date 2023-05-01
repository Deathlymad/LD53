using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionProvider : MonoBehaviour
{
    public class Mission
    {
        public City start;
        public City target;
        public int reward;
        public Artifact goods;
    };

    public City selfCity;
    public List<City> targets;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GetNewMission()
    {
        Mission m = new Mission();
        m.start = selfCity;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
