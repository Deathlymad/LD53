using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    public float speed;
    public Vector3 target = Vector3.positiveInfinity; //add delegate and generate curve

    // Start is called before the first frame update
    void Start()
    {
        if (target == Vector3.positiveInfinity)
            target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 delta = (target - transform.position);
        float mag = delta.magnitude;
        delta = delta.normalized;

        if (mag < speed * Time.deltaTime)
            transform.position = target;
        else
            transform.position += delta * speed * Time.deltaTime;
    }
}
