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

    public Mission GetNewMission()
    {
        Mission m = new Mission();
        m.start = selfCity;
        m.reward = UnityEngine.Random.Range(10, 100); //this should not be as random
        m.target = targets[UnityEngine.Random.Range(0, targets.Count)];
        m.goods = m.target.items[UnityEngine.Random.Range(0, m.target.items.Count)];

        return m;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
