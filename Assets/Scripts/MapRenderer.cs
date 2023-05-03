using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRenderer : MonoBehaviour
{
    public Camera mapCamera;
    public float timePerFrame = 1.0f;
    private float currentDelta = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        mapCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentDelta += Time.deltaTime;
        if (timePerFrame < currentDelta)
        {
            currentDelta -= timePerFrame;
            mapCamera.Render();
        }

    }
}
