using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderGenerator : MonoBehaviour
{
    public GameObject borderPrefab;
    public Vector3 offset = Vector3.zero;
    public int thickness = 1;
    public int width = 1, height = 1;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = -1; x >= -thickness; x--)
            for (int y = -thickness; y < height + thickness; y++)
                spawnTile(offset + new Vector3(x, 0.0f, y));
        for (int x = width; x < width + thickness; x++)
            for (int y = -thickness; y < height + thickness; y++)
                spawnTile(offset + new Vector3(x, 0.0f, y));


        for (int x = 0; x < width; x++)
            for (int y = -1; y >= -thickness; y--)
                spawnTile(offset + new Vector3(x, 0.0f, y));
        for (int x = 0; x < width; x++)
            for (int y = height; y < height + thickness; y++)
                spawnTile(offset + new Vector3(x, 0.0f, y));
    }

    private GameObject spawnTile(Vector3 pos, bool heavy = true)
    {
        GameObject tile = Instantiate(borderPrefab); //TODO: select the correct tile
        tile.transform.parent = transform;
        tile.transform.position = pos - Vector3.up;

        tile.GetComponent<MoveToTarget>().target = pos;
        tile.GetComponent<PreOcclusion>().refTransform = player;
        return tile;
    }

    void OnDrawGizmos()
    {
        if (borderPrefab != null)
        {
            Gizmos.color = Color.green;
            //TestEdges
            for (int x = -1; x >= -thickness; x--)
                for (int y = -thickness; y < height + thickness; y++)
                    Gizmos.DrawSphere(offset + new Vector3(x, 0.0f, y), 0.4f);
            for (int x = width; x < width + thickness; x++)
                for (int y = -thickness; y < height + thickness; y++)
                    Gizmos.DrawSphere(offset + new Vector3(x, 0.0f, y), 0.4f);


            for (int x = -thickness; x < width + thickness; x++)
                for (int y = -1; y >= -thickness; y--)
                    Gizmos.DrawSphere(offset + new Vector3(x, 0.0f, y), 0.4f);
            for (int x = -thickness; x < width + thickness; x++)
                for (int y = height; y < height + thickness; y++)
                    Gizmos.DrawSphere(offset + new Vector3(x, 0.0f, y), 0.4f);
        }
    }

}
