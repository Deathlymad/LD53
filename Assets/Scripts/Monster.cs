using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Vector3 stop;
    public float speed;
    public GameObject animationWrapper;

    // Start is called before the first frame update
    void Start()
    {
        animationWrapper.GetComponent<Player>().enterCity(); //disable inputs
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 delta = (stop - transform.position);

        if (delta.magnitude < speed * Time.deltaTime)
        {
            transform.position = stop;
            Destroy(this); //cleanup
        }
        else
        {
            transform.position += delta.normalized * speed * Time.deltaTime;
            animationWrapper.GetComponent<Player>().Walk();
        }
    }
}
