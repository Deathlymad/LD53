using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBehavior : MonoBehaviour
{
    public City data;

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

        if (other.gameObject.GetComponent<Player>() != null)
        {
            Player obj = other.gameObject.GetComponent<Player>();
            obj.enterCity();
        }
        Debug.Log("Entered City");
    }
    public void OnTriggerExit(Collider other)
    {
        anim.ResetTrigger("IsNear");
        anim.SetTrigger("IsNotNear");

        if (other.gameObject.GetComponent<Player>() != null)
        {
            Player obj = other.gameObject.GetComponent<Player>();
            obj.leaveCity();
        }
        Debug.Log("Left City");
    }
}
