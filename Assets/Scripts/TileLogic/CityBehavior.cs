using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBehavior : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        anim.ResetTrigger("IsNotNear");
        anim.SetTrigger("IsNear");
        Debug.Log("Entered City");
    }
    public void OnTriggerExit(Collider collision)
    {
        anim.ResetTrigger("IsNear");
        anim.SetTrigger("IsNotNear");
        Debug.Log("Left City");
    }
}
