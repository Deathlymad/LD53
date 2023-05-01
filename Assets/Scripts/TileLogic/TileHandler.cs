using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHandler : MonoBehaviour
{
    public float pathWeight;

    [Serializable]
    public class Decorator
    {
        public GameObject prefab;
        public float weight;
        public bool passable;
    }
    public bool isPassable;

    public float min = 0.8f;
    public float max = 1.2f;

    public float freeWeight;
    public List<Decorator> decorators;

    [Range(5,10)]
    public int x = 5, y = 5;

    private float totalPassableWeight = 0.0f;
    private float totalImpassableWeight = 0.0f;

    public float GetPathWeight()
    {
        return pathWeight;
    }

    private void decorate(Vector3 offset, float xScale, float yScale, bool isPassable)
    {
        float w = UnityEngine.Random.Range(0, isPassable ? totalPassableWeight : totalImpassableWeight);
        w -= freeWeight;
        if (w <= 0.01f)
        {
            return;
        }

        foreach(var v in decorators)
        {
            if (v.passable != isPassable)
                continue;

            w -= v.weight;
            if (w <= 0.01f && v.prefab != null)
            {
                float scaleFactor = UnityEngine.Random.Range(min, max);
                GameObject obj = Instantiate(v.prefab);
                obj.transform.SetParent(transform, false);
                obj.transform.position += offset + new Vector3(UnityEngine.Random.Range(-xScale / 4.0f, xScale / 4.0f), Math.Min(xScale, yScale) * scaleFactor * 0.5f, UnityEngine.Random.Range(-yScale / 4.0f, yScale / 4.0f));
                obj.transform.rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 340), Vector3.up);
                //obj.transform.localScale += Vector3.up;
                obj.transform.localScale *= Math.Min(xScale, yScale) * scaleFactor;
                
                return;
            }
        }
    }

    private void generateDecoration()
    {
        float x_fac = (x + 1);
        float y_fac = (y + 1);
        for (float _x = 1.5f; _x < x_fac - 1; _x ++)
            for (float _z = 1.5f; _z < y_fac - 1; _z++)
                decorate(new Vector3(_x / x_fac - 0.5f, 0.05f, _z / y_fac - 0.5f), 0.5f / x_fac, 0.5f / y_fac, UnityEngine.Random.Range(0.0f, 1.0f) < (totalPassableWeight / (totalPassableWeight + totalImpassableWeight)));

        //generate Borders
        if (!isPassable)
        {
            for (float _x = 0.5f; _x < x_fac; _x++)
            {
                decorate(new Vector3(_x / x_fac - 0.5f, 0.05f, 0.5f / y_fac - 0.5f), 0.5f / y_fac, 0.5f / y_fac, false);
                decorate(new Vector3(_x / x_fac - 0.5f, 0.05f, (y + 0.5f) / y_fac - 0.5f), 0.5f / y_fac, 0.5f / y_fac, false);
            }

            for (float _z = 0.5f; _z < y_fac; _z++)
            {
                decorate(new Vector3(0.5f / x_fac - 0.5f, 0.05f, _z / y_fac - 0.5f), 0.5f / y_fac, 0.5f / y_fac, false);
                decorate(new Vector3((x + 0.5f) / x_fac - 0.5f, 0.05f, _z / y_fac - 0.5f), 0.5f / y_fac, 0.5f / y_fac, false);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        totalPassableWeight = freeWeight;
        totalImpassableWeight = freeWeight;
        foreach (var v in decorators)
        {
            if (v.passable)
                totalPassableWeight += v.weight;
            else
                totalImpassableWeight += v.weight;
        }

        generateDecoration();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnDrawGizmos()
    {
        float x_fac = (float)(x + 1);
        float y_fac = (float)(y + 1);
        float size = Math.Min(0.5f / x_fac, 0.5f / y_fac);

        Gizmos.color = Color.yellow;
        for (float x = 1.5f; x < x_fac - 1; x ++)
            for (float z = 1.5f; z < y_fac - 1; z ++)
            {
                Gizmos.DrawWireSphere(transform.position + new Vector3(x / x_fac - 0.5f, 0.2f, z / y_fac - 0.5f), size);
            }

        Gizmos.color = Color.red;

        //generate Borders
        if (!isPassable)
        {
            for (float x = 0.5f; x < x_fac; x ++)
            {
                Gizmos.DrawWireSphere(transform.position + new Vector3(x / x_fac - 0.5f, 0.2f, 0.5f / y_fac - 0.5f), size);
                Gizmos.DrawWireSphere(transform.position + new Vector3(x / x_fac - 0.5f, 0.2f, (y + 0.5f) / y_fac - 0.5f), size);
            }

            for (float z = 0.5f; z < y_fac; z ++)
            {
                Gizmos.DrawWireSphere(transform.position + new Vector3(0.5f / x_fac - 0.5f, 0.2f, z / y_fac - 0.5f), size);
                Gizmos.DrawWireSphere(transform.position + new Vector3((x + 0.5f) / x_fac - 0.5f, 0.2f, z / y_fac - 0.5f), size);
            }
        }
        //guaranteeSolvability();
    }

}
