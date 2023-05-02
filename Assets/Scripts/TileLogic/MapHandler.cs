using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    private class RowSort : IComparer<GameObject>
    {
        public int Compare(GameObject a, GameObject b)
        {
            return (int)(a.GetComponent<MoveToTarget>().target.x - b.GetComponent<MoveToTarget>().target.x);
        }
    }
    private class ColSort : IComparer<GameObject>
    {
        public int Compare(GameObject a, GameObject b)
        {
            return (int)(a.GetComponent<MoveToTarget>().target.z - b.GetComponent<MoveToTarget>().target.z);
        }
    }
    private class Edge : IComparable<Edge>
    {
        public Edge(int a, int b, float c)
        {
            v1 = a;
            v2 = b;
            f = c;
        }
        public int v1;
        public int v2;
        public float f;
        public int CompareTo(Edge other)
        {
            return (int)(100 * f - other.f);
        }
    }
    private class NavNode : IComparable<NavNode>
    {
        public NavNode(int n, float f, NavNode p = null)
        {
            node = n;
            this.f = f;
            pred = new List<int>();
            if (p != null)
            {
                if (p.pred.Count > 0)
                    pred.AddRange(p.pred);
                pred.Add(p.node);
            }
        }
        public int node;
        public float f;
        public List<int> pred;

        public int CompareTo(NavNode other)
        {
            return (int)(100 * f - other.f);
        }
    }
    private ColSort csrt = new ColSort();
    private RowSort rsrt = new RowSort();

    [Range(5, 300)]
    public int width, height;
    private int city_size = 2;

    public GameObject CityObject;
    public List<City> CityData;
    public List<GameObject> tiles; //TilePrefabs

    private List<GameObject>[] topCols, botCols, rightRows, leftRows;

    public float shiftInterval = 100.0f;
    public Transform player;
    private float intervalCount = 0.0f;

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
    
    public GameObject getTileFromPosition(Vector3 pos)
    {
        if (pos.x >= width / 2.0f)
        {
            rightRows[(int)Math.Ceiling(pos.z)].Sort(rsrt);
            foreach ( var t in rightRows[(int)Math.Ceiling(pos.z)])
                if (Math.Abs(t.GetComponent<MoveToTarget>().target.x - Math.Ceiling(pos.x)) < 0.01f)
                    return t;
        }
        leftRows[(int)Math.Ceiling(pos.z)].Sort(rsrt);
        foreach (var t in leftRows[(int)Math.Ceiling(pos.z)])
            if (Math.Abs(t.GetComponent<MoveToTarget>().target.x - Math.Ceiling(pos.x)) < 0.01f)
                return t;

        throw new ArgumentException("Position Apparently not on map");
    }

    private GameObject spawnTile(Vector3 pos, bool heavy = true)
    {
        GameObject tile = Instantiate(heavy ? tiles[0] : tiles[1]); //TODO: select the correct tile
        tile.transform.parent = transform;
        tile.transform.position = pos - Vector3.up;

        tile.GetComponent<MoveToTarget>().target = pos;
        return tile;
    }

    private void spawnCities()
    {
        //bottom left city
        GameObject city = Instantiate(CityObject);
        city.transform.parent = transform;
        city.transform.Rotate(0, 180, 0);
        city.transform.position = new Vector3(0.5f, 0, 0.5f);
        city.GetComponent<CityBehavior>().data = CityData[0];
        city.GetComponent<MissionProvider>().selfCity = CityData[0];
        //bottom right city
        city = Instantiate(CityObject);
        city.transform.parent = transform;
        city.transform.position = new Vector3(width - 1.5f, 0, 0.5f);
        city.transform.Rotate(0, 90, 0);
        city.GetComponent<CityBehavior>().data = CityData[1];
        city.GetComponent<MissionProvider>().selfCity = CityData[1];
        //top left city
        city = Instantiate(CityObject);
        city.transform.parent = transform;
        city.transform.position = new Vector3(0.5f, 0, height - 1.5f);
        city.transform.Rotate(0, -90, 0);
        city.GetComponent<CityBehavior>().data = CityData[2];
        city.GetComponent<MissionProvider>().selfCity = CityData[2];
        //top right city
        city = Instantiate(CityObject);
        city.transform.parent = transform;
        city.transform.position = new Vector3(width - 1.5f, 0, height - 1.5f);
        city.GetComponent<CityBehavior>().data = CityData[3];
        city.GetComponent<MissionProvider>().selfCity = CityData[3];
    }
    private void generateTiles()
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
        for (int x = 2; x < width - 2; x ++)
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

        for (int i = 0; i < width; i++)
        {
            topCols[i].Sort(csrt);
            botCols[i].Sort(csrt);
        }
        for (int i = 0; i < height; i++)
        {
            leftRows[i].Sort(csrt);
            rightRows[i].Sort(csrt);
        }
    }

    public void generateMap()
    {
        spawnCities();
        generateTiles();
        updateMap();
    }

    //TODO take advantage of sorting
    private void pushRow(int idx, bool right = true)
    {
        if (right)
        {
            //Step 1 update position, delete improper references
            int toDelete = -1;
            ref var lst = ref rightRows[idx];
            for (int i = 0; i < lst.Count; i++)
            {
                var cpn = lst[i].GetComponent<MoveToTarget>();
                //Step 1.1 remove all entities operated on
                if (idx >= height / 2)
                {
                    topCols[(int)cpn.target.x].Remove(lst[i]);
                }
                if (idx <= height / 2)
                {
                    botCols[(int)cpn.target.x].Remove(lst[i]);
                }
                //Step 1.2 Update Position
                cpn.target += Vector3.right;
                //Step 1.3 Remove out of Range Tile
                if (cpn.target.x >= width || ((idx < 2 || idx >= height - 2) && cpn.target.x >= width - 2))
                {
                    toDelete = i;//there is always only one tile to delete
                }
            }
            Destroy(lst[toDelete]);
            lst.RemoveAt(toDelete);
            //Step 2 add new Object
            GameObject tile = spawnTile(new Vector3((float)Math.Ceiling((width + 1) / 2.0), 0, idx));
            lst.Add(tile);
            //Step 3 re add all objects to rows
            for (int i = 0; i < lst.Count; i++)
            {
                var cpn = lst[i].GetComponent<MoveToTarget>();
                if (idx >= height / 2)
                {
                    topCols[(int)cpn.target.x].Add(lst[i]);
                }
                if (idx <= height / 2)
                {
                    botCols[(int)cpn.target.x].Add(lst[i]);
                }
            }
        }
        else
        {
            //Step 1 update position, delete improper references
            int toDelete = -1;
            ref var lst = ref leftRows[idx];
            for (int i = 0; i < lst.Count; i++)
            {
                var cpn = lst[i].GetComponent<MoveToTarget>();
                //Step 1.1 remove all entities operated on
                if (idx >= height / 2)
                {
                    topCols[(int)cpn.target.x].Remove(lst[i]);
                }
                if (idx <= height / 2)
                {
                    botCols[(int)cpn.target.x].Remove(lst[i]);
                }
                //Step 1.2 Update Position
                cpn.target -= Vector3.right;
                //Step 1.3 Remove out of Range Tile
                if (cpn.target.x < 0 || ((idx < 2 || idx >= height - 2) && cpn.target.x < 2))
                {
                    toDelete = i;//there is always only one tile to delete
                }
            }
            Destroy(lst[toDelete]);
            lst.RemoveAt(toDelete);
            //Step 2 add new Object
            GameObject tile = spawnTile(new Vector3((float)Math.Floor((width - 1) / 2.0), 0, idx));
            lst.Add(tile);
            //Step 3 re add all objects to rows
            for (int i = 0; i < lst.Count; i++)
            {
                var cpn = lst[i].GetComponent<MoveToTarget>();
                if (idx >= height / 2)
                {
                    topCols[(int)cpn.target.x].Add(lst[i]);
                }
                if (idx <= height / 2)
                {
                    botCols[(int)cpn.target.x].Add(lst[i]);
                }
            }
        }
    }
    private void pushColumn(int idx, bool up = true)
    {
        if (up)
        {
            //Step 1 update position, delete improper references
            int toDelete = -1;

            ref var lst = ref topCols[idx];

            for (int i = 0; i < lst.Count; i++)
            {
                var cpn = lst[i].GetComponent<MoveToTarget>();

                //Step 1.1 remove all entities operated on
                if (idx >= width / 2)
                {
                    if (!rightRows[(int)cpn.target.z].Remove(lst[i]))
                        throw new IndexOutOfRangeException("Bad computation when updating references");
                }
                if (idx <= width / 2)
                {
                    if (!leftRows[(int)cpn.target.z].Remove(lst[i]))
                        throw new IndexOutOfRangeException("Bad computation when updating references");
                }
                //Step 1.2 Update Position
                cpn.target += Vector3.forward;
                //Step 1.3 Remove out of Range Tile
                if (cpn.target.z >= height || ((idx < 2 || idx >= width - 2) && cpn.target.z >= height - 2))
                {
                    toDelete = i;//there is always only one tile to delete
                }
            }
            Destroy(lst[toDelete]);
            lst.RemoveAt(toDelete);
            //Step 2 add new Object
            GameObject tile = spawnTile(new Vector3(idx, 0, (float)Math.Ceiling(height / 2.0 + 1))); //This floor calculation for some reason breaks when switiching beween updating a single or multiple columns. no clue why
            lst.Add(tile);
            //Step 3 re add all objects to rows
            for (int i = 0; i < lst.Count; i++)
            {
                var cpn = lst[i].GetComponent<MoveToTarget>();

                if (idx >= width / 2.0f)
                {
                    rightRows[(int)cpn.target.z].Add(lst[i]);
                }
                if (idx <= width / 2.0f)
                {
                    leftRows[(int)cpn.target.z].Add(lst[i]);
                }
            }
        }
        else
        {
            ref var lst = ref botCols[idx];
            //Step 1 update position, delete improper references
            int toDelete = -1;
            for (int i = 0; i < lst.Count; i++)
            {
                var cpn = lst[i].GetComponent<MoveToTarget>();
                //Step 1.1 remove all entities operated on
                if (idx >= width / 2)
                {
                    if (!rightRows[(int)cpn.target.z].Remove(lst[i]))
                        throw new IndexOutOfRangeException("Bad computation when updating references");
                }
                if (idx <= width / 2)
                {
                    if (!leftRows[(int)cpn.target.z].Remove(lst[i]))
                        throw new IndexOutOfRangeException("Bad computation when updating references");
                }
                //Step 1.2 Update Position
                cpn.target -= Vector3.forward;
                //Step 1.3 Remove out of Range Tile
                if (cpn.target.z < 0 || ((idx < 2 || idx >= width - 2) && cpn.target.z < 2))
                {
                    toDelete = i;//there is always only one tile to delete
                }
            }
            Destroy(lst[toDelete]);
            lst.RemoveAt(toDelete);
            //Step 2 add new Object
            GameObject tile = spawnTile(new Vector3(idx, 0, (float)Math.Floor(height / 2.0 - 1))); //This floor calculation for some reason breaks when switiching beween updating a single or multiple columns. no clue why
            lst.Add(tile);
            //Step 3 re add all objects to rows
            for (int i = 0; i < lst.Count; i++)
            {
                var cpn = lst[i].GetComponent<MoveToTarget>();
                if (idx >= width / 2.0f)
                {
                    rightRows[(int)cpn.target.z].Add(lst[i]);
                }
                if (idx <= width / 2.0f)
                {
                    leftRows[(int)cpn.target.z].Add(lst[i]);
                }
            }
        }
    }

    private int coordToID(Vector3 obj)
    {
        return (int)(obj.x * height + obj.z);
    }

    private int[] AStar(List<Edge> edges, int[] g)
    {
        int root = 0;
        root += UnityEngine.Random.Range(2, width - 2) * height + UnityEngine.Random.Range(2, height - 2);

        HashSet<int> closedList = new HashSet<int>();
        List<NavNode> openList = new List<NavNode>();
        openList.Add(new NavNode(root, 0.0f, null));

        List<int> goals = new List<int>();
        goals.AddRange(g);

        List<int> res = new List<int>();
        

        while (openList.Count > 0)
        {
            openList.Sort();
            //compute open list
            var node = openList[0];
            openList.RemoveAt(0);
            if (goals.Contains(node.node))
            {
                goals.Remove(node.node);
                //backtrace and build route
                //selectedNodes.AddRange(node.pred);
                res.AddRange(node.pred);
                if (goals.Count <= 0)
                    return res.ToArray();
            }
            closedList.Add(node.node);

            //expand to new open nodes
            foreach (var e in edges)
            {
                if (e.v1 == node.node && !closedList.Contains(e.v2))
                {
                    float wgh = e.f;
                    if (res.Contains(e.v2))
                        wgh -= 100.0f;//handle previous used paths here
                    if (openList.Select(x => x.node).Contains(e.v2))
                    {
                        int toDelete = -1;
                        for (int i = 0; i < openList.Count; i++)
                        {
                            if (openList[i].node == e.v2 && (wgh + node.f) < openList[i].f)
                                toDelete = i;
                        }
                        if (toDelete > 0)
                        {
                            openList.RemoveAt(toDelete);
                            openList.Insert(toDelete, new NavNode(e.v2, wgh + node.f, node));
                        }
                    }
                    else
                    {
                        openList.Insert(openList.Count, new NavNode(e.v2, wgh + node.f, node));
                    }
                }
                else if (e.v2 == node.node && !closedList.Contains(e.v1))
                {
                    float wgh = e.f;
                    if (res.Contains(e.v1))
                        wgh -= 100;//handle previous used paths here
                    if (openList.Select(x => x.node).Contains(e.v1))
                    {
                        int toDelete = -1;
                        for (int i = 0; i < openList.Count; i++)
                        {
                            if (openList[i].node == e.v1 && (wgh + node.f) < openList[i].f)
                                toDelete = i;
                        }
                        if (toDelete > 0)
                        {
                            openList.RemoveAt(toDelete);
                            openList.Insert(toDelete, new NavNode(e.v1, wgh + node.f, node));
                        }
                    }
                    else
                    {
                        openList.Insert(openList.Count, new NavNode(e.v1, wgh + node.f, node));
                    }
                }
            }
        }
        return res.ToArray();
    }

    private void guaranteeSolvability()
    {
        //TODO Kruskal and then remove nonessential paths
        List<Edge> graph = new List<Edge>(); //weight; (v1, v2); -> edge list

        //generate edge list
        foreach (var grp in new []{ topCols, botCols})
        {
            foreach (var col in grp)
            {
                col.Sort(csrt);
                for (int i = 1; i < col.Count; i++)
                {
                    var weight = UnityEngine.Random.Range(0.0f, 10.0f);
                    weight += col[i - 1].GetComponent<TileHandler>().GetPathWeight();
                    weight += col[i].GetComponent<TileHandler>().GetPathWeight();
                    var cpn1 = col[i - 1].GetComponent<MoveToTarget>();
                    var cpn = col[i].GetComponent<MoveToTarget>();
                    graph.Add(new Edge(coordToID(cpn1.target), coordToID(cpn.target), weight));
                    if ((cpn1.target - cpn.target).magnitude > 1.1)
                    {
                        throw new AccessViolationException("Failed to compute graph");
                    }
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            var weight = UnityEngine.Random.Range(0.0f, 10.0f);
            weight += topCols[x][0].GetComponent<TileHandler>().GetPathWeight();
            weight += botCols[x].Last().GetComponent<TileHandler>().GetPathWeight();
            var cpn1 = topCols[x][0].GetComponent<MoveToTarget>();
            var cpn = botCols[x].Last().GetComponent<MoveToTarget>();
            graph.Add(new Edge(coordToID(cpn1.target), coordToID(cpn.target), weight));
            if ((cpn1.target - cpn.target).magnitude > 1.1)
            {
                throw new AccessViolationException("Failed to compute graph");
            }
        }

        foreach (var grp in new[] { leftRows, rightRows })
        {
            foreach (var col in grp)
            {
                col.Sort(rsrt);
                for (int i = 1; i < col.Count; i++)
                {
                    var weight = UnityEngine.Random.Range(0.0f, 10.0f);
                    weight += col[i - 1].GetComponent<TileHandler>().GetPathWeight();
                    weight += col[i].GetComponent<TileHandler>().GetPathWeight();
                    var cpn1 = col[i - 1].GetComponent<MoveToTarget>();
                    var cpn = col[i].GetComponent<MoveToTarget>();
                    graph.Add(new Edge(coordToID(cpn1.target), coordToID(cpn.target), weight));
                    if ((cpn1.target - cpn.target).magnitude > 1.1)
                    {
                        throw new AccessViolationException("Failed to compute graph");
                    }
                }
            }
        }
        for (int y = 0; y < width; y++)
        {
            var weight = UnityEngine.Random.Range(0.0f, 10.0f);
            weight += rightRows[y][0].GetComponent<TileHandler>().GetPathWeight();
            weight += leftRows[y].Last().GetComponent<TileHandler>().GetPathWeight();
            var cpn1 = rightRows[y][0].GetComponent<MoveToTarget>();
            var cpn = leftRows[y].Last().GetComponent<MoveToTarget>();
            graph.Add(new Edge(coordToID(cpn1.target), coordToID(cpn.target), weight));
            if ((cpn1.target - cpn.target).magnitude > 1.1)
            {
                throw new AccessViolationException("Failed to compute graph");
            }
        }

        //Add edges to cities
        int c1 = 0;
        int c2 = width;
        int c3 = (height - 1) * width;
        int c4 = height * width;
        //C0 (bottom left)
        for (int i = 1; i < city_size; i++)
            graph.Add(new Edge(c1, height * i + city_size, 0.0f));
        for (int i = 1; i <= city_size; i++)
            graph.Add(new Edge(c1, height * city_size + i, 0.0f));
        

        //C1 (top left)
        for (int i = 1; i < city_size; i++)
            graph.Add(new Edge(c2, height * i + height - city_size - 1, 0.0f));
        for (int i = 0; i < city_size; i++)
            graph.Add(new Edge(c2, height * city_size + height - city_size - 1 + i, 0.0f));



        //C3 (bottom right)
        for (int i = 1; i <= city_size; i++)
            graph.Add(new Edge(c3, height * (width - city_size - 1) + i, 0.0f));
        for (int i = 1; i < city_size; i++)
            graph.Add(new Edge(c3, height * (width - city_size - 1 + i) + city_size, 0.0f));


        //C4 (top right)
        for (int i = 0; i < city_size; i++)
            graph.Add(new Edge(c4, height * (width - city_size - 1) + height - city_size - 1 + i, 0.0f));
        for (int i = 0; i < city_size; i++)
            graph.Add(new Edge(c4, height * (width - city_size - 1 + i) + height - city_size - 1, 0.0f));


        int[] freeFields = AStar(graph, new int[] { c1, c2, c3, c4});

        HashSet<int> toDestroy = new HashSet<int>();
        HashSet<int> cityBorders = new HashSet<int>();
        //C0 (bottom left)
        for (int i = 0; i < city_size; i++)
            cityBorders.Add(height * i + city_size);
        for (int i = 0; i <= city_size; i++)
            cityBorders.Add(height * city_size + i);
        //C1 (top left)
        for (int i = 0; i < city_size; i++)
            cityBorders.Add(height * i + height - city_size - 1);
        for (int i = 0; i <= city_size; i++)
            cityBorders.Add(height * city_size + height - city_size - 1 + i);
        //C3 (bottom right)
        for (int i = 0; i <= city_size; i++)
            cityBorders.Add(height * (width - city_size - 1) + i);
        for (int i = 0; i < city_size; i++)
            cityBorders.Add(height * (width - city_size + i) + city_size);
        //C4 (top right)
        for (int i = 0; i <= city_size; i++)
            cityBorders.Add(height * (width - city_size - 1) + height - city_size - 1 + i);
        for (int i = 0; i < city_size; i++)
            cityBorders.Add(height * (width - city_size + i) + height - city_size - 1);
        for (int i = 0; i < this.transform.childCount; i++)
        {
            var child = this.transform.GetChild(i);
            if (child.gameObject.tag == "Tile")
                if (freeFields.Contains(coordToID(child.GetComponent<MoveToTarget>().target)) || cityBorders.Contains(coordToID(child.GetComponent<MoveToTarget>().target)))
                {
                    toDestroy.Add(i);
                }
        }
        //clean around towns
        
        
        foreach (int i in toDestroy)
        {
            var child = this.transform.GetChild(i);

            if (child.gameObject.GetComponent<TileHandler>().GetPathWeight() == tiles[1].GetComponent<TileHandler>().GetPathWeight())
            {
                continue;
            }

            var x = (int)child.GetComponent<MoveToTarget>().target.x;
            var y = (int)child.GetComponent<MoveToTarget>().target.z;
            if (
                y >= height || ((x < 2 || x >= width - 2) && y >= height - 2) || 
                x < 0 || ((y < 2 || y >= height - 2) && x < 2) ||
                x >= width || ((y < 2 || y >= height - 2) && x >= width - 2) ||
                y < 0 || ((x < 2 || x >= width - 2) && y < 2)
                )
                continue;
            GameObject tile = spawnTile(child.gameObject.GetComponent<MoveToTarget>().target, false);

            if (x >= width / 2)
            {
                rightRows[y].Remove(child.gameObject);
                rightRows[y].Add(tile);
            }
            if (x <= width / 2)
            {
                leftRows[y].Remove(child.gameObject);
                leftRows[y].Add(tile);
            }
            if (y >= height / 2)
            {
                topCols[x].Remove(child.gameObject);
                topCols[x].Add(tile);
            }
            if (y <= height / 2)
            {
                botCols[x].Remove(child.gameObject);
                botCols[x].Add(tile);
            }
            Destroy(child.gameObject);
        }
    }

    public void updateMap()
    {
        //TODO: make variable
        for (int i = 0; i < 5; i++)
        {
            int val = UnityEngine.Random.Range(0, 5);
            pushRow(val, false);
            pushRow(val, true);
            pushColumn(val, false);
            pushColumn(val, true);
            guaranteeSolvability();
        }
    }

    // Update is called once per frame
    void Update()
    {
        intervalCount += Time.deltaTime;
        if (intervalCount >= shiftInterval && false)
        {
            intervalCount -= shiftInterval;
            Vector3 pos = player.position;
            int X = (int)(Math.Round(pos.x));
            int Y = (int)(Math.Round(pos.z));
            if (X < width / 2)
            {
                pushRow(UnityEngine.Random.Range(0, height - 1), true);
            }
            if (X > width / 2)
            {
                pushRow(UnityEngine.Random.Range(0, height - 1), false);
            }
            if (Y < height / 2)
            {
                pushColumn(UnityEngine.Random.Range(0, width - 1), true);
            }
            if (Y > height / 2)
            {
                pushColumn(UnityEngine.Random.Range(0, width - 1), false);
            }
            guaranteeSolvability();
        }
    }

    void OnDrawGizmos()
    {
        //TODO: maybe realign so these coordinates aren't as whack anymore
        Gizmos.DrawWireCube(transform.TransformPoint(new Vector3(width / 2 - 0.5f, 1, height / 2 - 0.5f)), transform.TransformPoint(new Vector3(width, 2, height)));
        //guaranteeSolvability();

        //TestEdges
        for (int i = 0; i < city_size; i++)
            Gizmos.DrawSphere(new Vector3(i, 0, city_size), 0.4f);
        for (int i = 0; i <= city_size; i++)
            Gizmos.DrawSphere(new Vector3(city_size, 0, i), 0.4f);


        //C1 (top left)
        for (int i = 0; i < city_size; i++)
            Gizmos.DrawSphere(new Vector3(i, 0, height - city_size - 1), 0.4f);
        for (int i = 0; i <= city_size; i++)
            Gizmos.DrawSphere(new Vector3(city_size, 0, height - city_size - 1 + i), 0.4f);



        //C3 (bottom right)
        for (int i = 0; i <= city_size; i++)
            Gizmos.DrawSphere(new Vector3((width - city_size - 1), 0, i), 0.4f);
        for (int i = 0; i < city_size; i++)
            Gizmos.DrawSphere(new Vector3((width - city_size + i), 0, city_size), 0.4f);


        //C4 (top right)
        for (int i = 0; i <= city_size; i++)
            Gizmos.DrawSphere(new Vector3((width - city_size - 1), 0, height - city_size - 1 + i), 0.4f);
        for (int i = 0; i < city_size; i++)
            Gizmos.DrawSphere(new Vector3((width - city_size + i), 0, height - city_size - 1), 0.4f);
    }

}
