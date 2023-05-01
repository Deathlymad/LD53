using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionProvider : MonoBehaviour
{

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
        m.goods = m.target.acceptedArtifacts[UnityEngine.Random.Range(0, m.target.acceptedArtifacts.Count)];

        return m;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
