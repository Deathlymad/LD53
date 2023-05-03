using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreOcclusion : MonoBehaviour
{
    public Transform refTransform;
    public float distance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (refTransform!=null)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                var child = this.transform.GetChild(i).gameObject;
                child.SetActive((refTransform.position - transform.position).magnitude < distance);
            }
        }
    }
}
