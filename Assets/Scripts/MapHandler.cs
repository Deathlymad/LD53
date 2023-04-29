using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    [Range(5, 30)]
    public int width, height;

    public GameObject CityObject;
    public List<GameObject> tiles;

    private List<GameObject>[] topCols, botCols, rightRows, leftRows;

    // Start is called before the first frame update
    void Start()
    {
        //Note that currently these are not disjunct. Also they need to migrate their own state along rows and columns when they are updated
        topCols = new List<GameObject>[width];
        botCols = new List<GameObject>[width];
        for (int i = 0; i < width; i++)
        {
            topCols[i] = new List<GameObject>();
            botCols[i] = new List<GameObject>();
        }
        rightRows = new List<GameObject>[height];
        leftRows = new List<GameObject>[height];
        for (int i = 0; i < height; i++)
        {
            rightRows[i] = new List<GameObject>();
            leftRows[i] = new List<GameObject>();
        }
        generateMap();
    }
    
    private GameObject spawnTile(Vector3 pos)
    {
        GameObject tile = Instantiate(tiles[0]); //TODO: select the correct tile
        tile.transform.parent = transform;
        tile.transform.position = pos;
        return tile;
    }

    public void spawnCities()
    {
        //bottom left city
        GameObject city = Instantiate(CityObject);
        city.transform.parent = transform;
        city.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.up);
        //bottom right city
        city = Instantiate(CityObject);
        city.transform.parent = transform;
        city.transform.position = new Vector3(width - 2.0f, 0, 0);
        city.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.up);
        //top left city
        city = Instantiate(CityObject);
        city.transform.parent = transform;
        city.transform.position = new Vector3(0, 0, height - 2.0f);
        city.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.up);
        //top right city
        city = Instantiate(CityObject);
        city.transform.parent = transform;
        city.transform.position = new Vector3(width - 2.0f, 0, height - 2.0f);
        city.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.up);
    }
    public void generateTiles()
    {
        for (int x = 0; x < 2; x++)
            for (int y = 2; y < height - 2; y++)
            {
                GameObject tile = spawnTile(new Vector3(x, 0, y));

                if (y >= height / 2)
                {
                    topCols[x].Add(tile);
                }
                if (y <= height / 2)
                {
                    botCols[x].Add(tile);
                }
                leftRows[y].Add(tile);
            }
        for (int x = width - 2; x < width; x++)
            for (int y = 2; y < height - 2; y++)
            {
                GameObject tile = spawnTile(new Vector3(x, 0, y));

                if (y >= height / 2)
                {
                    topCols[x].Add(tile);
                }
                if (y <= height / 2)
                {
                    botCols[x].Add(tile);
                }
                rightRows[y].Add(tile);
            }
        for (int x = 2; x < width - 2; x++)
            for (int y = 0; y < 2; y++)
            {
                GameObject tile = spawnTile(new Vector3(x, 0, y));

                if (x >= width / 2)
                {
                    rightRows[y].Add(tile);
                }
                if (x <= width / 2)
                {
                    leftRows[y].Add(tile);
                }
                botCols[x].Add(tile);
            }
        for (int x = 2; x < width - 2; x++)
            for (int y = height - 2; y < height; y++)
            {
                GameObject tile = spawnTile(new Vector3(x, 0, y));

                if (x >= width / 2)
                {
                    rightRows[y].Add(tile);
                }
                if (x <= width / 2)
                {
                    leftRows[y].Add(tile);
                }
                topCols[x].Add(tile);
            }
        for (int x = 2; x < height - 2; x ++)
            for (int y = 2; y < height - 2; y++)
            {
                GameObject tile = spawnTile(new Vector3(x, 0, y));

                if (x >= width / 2)
                {
                    rightRows[y].Add(tile);
                }
                if (x <= width / 2)
                {
                    leftRows[y].Add(tile);
                }
                if (y >= height / 2)
                {
                    topCols[x].Add(tile);
                }
                if (y <= height / 2)
                {
                    botCols[x].Add(tile);
                }
            }
    }

    public void generateMap()
    {
        spawnCities();
        generateTiles();
    }

    //doesnt update other lists
    public void pushRow(int idx, bool right = true)
    {
        if (right)
        {
            //Step 1 update position, delete improper references
            int toDelete = -1;
            for (int i = 0; i < rightRows[idx].Count; i++)
            {
                //Step 1.1 remove all entities operated on
                if (idx >= height / 2)
                {
                    topCols[(int)rightRows[idx][i].transform.position.x].Remove(rightRows[idx][i]);
                }
                if (idx <= height / 2)
                {
                    botCols[(int)rightRows[idx][i].transform.position.x].Remove(rightRows[idx][i]);
                }
                //Step 1.2 Update Position
                rightRows[idx][i].transform.position += Vector3.right;
                //Step 1.3 Remove out of Range Tile
                if (rightRows[idx][i].transform.position.x >= width || ((idx < 2 || idx >= height - 2) && rightRows[idx][i].transform.position.x >= width - 2))
                {
                    Destroy(rightRows[idx][i]);
                    toDelete = i;//there is always only one tile to delete
                }
            }
            rightRows[idx].RemoveAt(toDelete);
            //Step 3 add new Object
            GameObject tile = spawnTile(new Vector3((float)Math.Ceiling((width + 1) / 2.0), 0, idx));
            rightRows[idx].Add(tile);
            //Step 4 re add all objects to rows
            for (int i = 0; i < rightRows[idx].Count; i++)
            {
                if (idx >= height / 2)
                {
                    topCols[(int)rightRows[idx][i].transform.position.x].Add(rightRows[idx][i]);
                }
                if (idx <= height / 2)
                {
                    botCols[(int)rightRows[idx][i].transform.position.x].Add(rightRows[idx][i]);
                }
            }
        }
        else
        {
            //Step 1 update position, delete improper references
            int toDelete = -1;
            for (int i = 0; i < leftRows[idx].Count; i++)
            {
                //Step 1.1 remove all entities operated on
                if (idx >= height / 2)
                {
                    topCols[(int)leftRows[idx][i].transform.position.x].Remove(leftRows[idx][i]);
                }
                if (idx <= height / 2)
                {
                    botCols[(int)leftRows[idx][i].transform.position.x].Remove(leftRows[idx][i]);
                }
                //Step 1.2 Update Position
                leftRows[idx][i].transform.position -= Vector3.right;
                //Step 1.3 Remove out of Range Tile
                if (leftRows[idx][i].transform.position.x < 0 || ((idx < 2 || idx >= height - 2) && leftRows[idx][i].transform.position.x < 2))
                {
                    Destroy(leftRows[idx][i]);
                    toDelete = i;//there is always only one tile to delete
                }
            }
            leftRows[idx].RemoveAt(toDelete);
            //Step 3 add new Object
            GameObject tile = spawnTile(new Vector3((float)Math.Floor((width - 1) / 2.0), 0, idx));
            leftRows[idx].Add(tile);
            //Step 4 re add all objects to rows
            for (int i = 0; i < leftRows[idx].Count; i++)
            {
                if (idx >= height / 2)
                {
                    topCols[(int)leftRows[idx][i].transform.position.x].Add(leftRows[idx][i]);
                }
                if (idx <= height / 2)
                {
                    botCols[(int)leftRows[idx][i].transform.position.x].Add(leftRows[idx][i]);
                }
            }
        }
    }
    public void pushColumn(int idx, bool up = true)
    {
        if (up)
        {
            //Step 1 update position, delete improper references
            int toDelete = -1;
            for (int i = 0; i < topCols[idx].Count; i++)
            {
                //Step 1.1 remove all entities operated on
                if (idx >= width / 2)
                {
                    if (!rightRows[(int)topCols[idx][i].transform.position.z].Remove(topCols[idx][i]))
                        throw new IndexOutOfRangeException("Bad computation when updating references");
                }
                if (idx <= width / 2)
                {
                    if (!leftRows[(int)topCols[idx][i].transform.position.z].Remove(topCols[idx][i]))
                        throw new IndexOutOfRangeException("Bad computation when updating references");
                }
                //Step 1.2 Update Position
                topCols[idx][i].transform.position += Vector3.forward;
                //Step 1.3 Remove out of Range Tile
                if (topCols[idx][i].transform.position.z >= height || ((idx < 2 || idx >= width - 2) && topCols[idx][i].transform.position.z >= height - 2))
                {
                    Destroy(topCols[idx][i]);
                    toDelete = i;//there is always only one tile to delete
                }
            }
            topCols[idx].RemoveAt(toDelete);
            //Step 3 add new Object
            GameObject tile = spawnTile(new Vector3(idx, 0, (float)Math.Ceiling((height + 1) / 2.0)));
            topCols[idx].Add(tile);
            //Step 4 re add all objects to rows
            for (int i = 0; i < topCols[idx].Count; i++)
            {
                if (idx >= width / 2.0f)
                {
                    rightRows[(int)topCols[idx][i].transform.position.z].Add(topCols[idx][i]);
                }
                if (idx <= width / 2.0f)
                {
                    leftRows[(int)topCols[idx][i].transform.position.z].Add(topCols[idx][i]);
                }
            }
        }
        else
        {
            //Step 1 update position, delete improper references
            int toDelete = -1;
            for (int i = 0; i < botCols[idx].Count; i++)
            {
                //Step 1.1 remove all entities operated on
                if (idx >= width / 2)
                {
                    if (!rightRows[(int)botCols[idx][i].transform.position.z].Remove(botCols[idx][i]))
                        throw new IndexOutOfRangeException("Bad computation when updating references");
                }
                if (idx <= width / 2)
                {
                    if (!leftRows[(int)botCols[idx][i].transform.position.z].Remove(botCols[idx][i]))
                        throw new IndexOutOfRangeException("Bad computation when updating references");
                }
                //Step 1.2 Update Position
                botCols[idx][i].transform.position -= Vector3.forward;
                //Step 1.3 Remove out of Range Tile
                if (botCols[idx][i].transform.position.z < 0 || ((idx < 2 || idx >= width - 2) && botCols[idx][i].transform.position.z < 2))
                {
                    Destroy(botCols[idx][i]);
                    toDelete = i;//there is always only one tile to delete
                }
            }
            botCols[idx].RemoveAt(toDelete);
            //Step 3 add new Object
            GameObject tile = spawnTile(new Vector3(idx, 0, (float)Math.Floor((height - 1) / 2.0)));
            botCols[idx].Add(tile);
            //Step 4 re add all objects to rows
            for (int i = 0; i < botCols[idx].Count; i++)
            {
                if (idx >= width / 2.0f)
                {
                    rightRows[(int)botCols[idx][i].transform.position.z].Add(botCols[idx][i]);
                }
                if (idx <= width / 2.0f)
                {
                    leftRows[(int)botCols[idx][i].transform.position.z].Add(botCols[idx][i]);
                }
            }
        }
    }

    public void updateMap()
    {
        for (int i = 0; i < 5; i++)
        {
            int val = UnityEngine.Random.Range(0, width);
            pushRow(val, false);
            pushRow(val, true);
            pushColumn(val, false);
            pushColumn(val, true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("x")) //TODO: Test Code
        {
            updateMap();
        }
    }

    void OnDrawGizmos()
    {
        //TODO: maybe realign so these coordinates aren't as whack anymore
        Gizmos.DrawWireCube(transform.TransformPoint(new Vector3(width / 2 - 0.5f, 1, height / 2 - 0.5f)), transform.TransformPoint(new Vector3(width, 2, height)));
    }

}
