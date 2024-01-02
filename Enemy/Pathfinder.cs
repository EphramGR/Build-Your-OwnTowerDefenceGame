using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{

    private int spawns; 
    public List<GameObject> portals;
    public List<List<int>> paths;
    public List<bool> pathStartingDir;

    void Start()
    {
        portals = new List<GameObject>();
        paths = new List<List<int>>();
        pathStartingDir = new List<bool>();
    }

    public void pathfinder()
    {
        spawns = 0;
        portals.Clear();
        paths.Clear();
        pathStartingDir.Clear();

        var objects = GameObject.FindGameObjectsWithTag("portal");

        foreach (var obj in objects) 
        {
            if (obj.name.Contains("(Clone)") && isAlive(obj))
            {
                portals.Add(obj);

                float rotate = obj.transform.localEulerAngles.z;
                //Debug.Log(rotate);
                if (rotate == 0)
                {
                    initiatePath(-100, 0, obj);
                }
                else if (rotate == 90)
                {
                    initiatePath(0, -100, obj);
                }
                else if(rotate == 180)
                {
                    initiatePath(100, 0, obj);
                }
                else
                {
                    initiatePath(0, 100 ,obj);
                }
                spawns += 1;
            }
        }
        //Debug.Log(printList(paths));
        //Debug.Log(printList(pathStartingDir));
    }
    private string printList(List<GameObject> list)
    {
        string result = "List contents: ";
        foreach (var item in list)
        {
            result += item.ToString() + ", ";
        }
        return result;
    }
    private string printList(List<int> list)
    {
        string result = "List contents: ";
        foreach (var item in list)
        {
            result += item.ToString() + ", ";
        }
        return result;
    }
    private string printList(List<bool> list)
    {
        string result = "List contents: ";
        foreach (var item in list)
        {
            result += item.ToString() + ", ";
        }
        return result;
    }
    private string printList(List<List<int>> list)
    {
        string result = "Multilist contents: ";
        int i = 0;
        foreach (var item in list)
        {
            result += printList(list[i]) + ", ";
            i ++;
        }
        return result;
    }
    private bool isAlive(GameObject obj)
    {
        Portal sc = obj.GetComponent<Portal>();
        if (sc.beingDestroyed == true)
            return false;
        else
            return true;
    }
    private void initiatePath(int x, int y, GameObject portal)
    {
        List<int> path = new List<int>();
        bool horizontal = false;
        GameObject startingPath = null;

        if (x != 0)
        {
            horizontal = true;
            pathStartingDir.Add(true);
        }
        else
            pathStartingDir.Add(false);

        var objects = GameObject.FindGameObjectsWithTag("path");

        foreach (var obj in objects)
        {
            if (portal.transform.position.x + x == obj.transform.position.x && portal.transform.position.y + y == obj.transform.position.y)
            {
                startingPath = obj;
                if (horizontal)
                    path.Add(x);
                else
                    path.Add(y);
            }
        }
        if (startingPath == null)
        {
            Debug.Log("no path found on given side");
        }

        beginPathfind(horizontal, path, startingPath);
    }

    private void beginPathfind(bool horizontal, List<int> path, GameObject startingPath)
    {
        bool end = false;
        GameObject cyclingPath = null;
        if (horizontal)
        {
            int direction = returnDir(startingPath, startingPath.GetComponent<Numberpick>().num);//up right down left

            if (direction == 0)
            {
                path.Add(200);
                horizontal = false;
            }
            else if (direction == 1)
            {
                path[path.Count - 1] += 200;
            }
            else if (direction == 2)
            {
                path.Add(-200);
                horizontal = false;
            }
            else
            {
                path[path.Count - 1] -= 200;
            }

            end = returnAdj(startingPath, startingPath.GetComponent<Numberpick>().num, direction);
            //Debug.Log("ending is " + end);

            if (!end)
                cyclingPath = nextPath(startingPath, direction);
        }
        else
        {
            cyclingPath = startingPath;
        }

        if (!end)
        {
            for(int i = 0; i < 1000; i++)
            {
                
                int direction = returnDir(cyclingPath, cyclingPath.GetComponent<Numberpick>().num);//up right down left
                
                if (horizontal)
                {
                    if (direction == 0)
                    {
                        path.Add(200);
                        horizontal = false;
                    }
                    else if (direction == 1)
                    {
                        path[path.Count - 1] += 200;
                    }
                    else if (direction == 2)
                    {
                        path.Add(-200);
                        horizontal = false;
                    }
                    else
                    {
                        path[path.Count - 1] -= 200;
                    }
                }
                else
                {
                    if (direction == 0)
                    {
                        path[path.Count - 1] += 200;
                    }
                    else if (direction == 1)
                    {
                        path.Add(200);
                        horizontal = true;
                    }
                    else if (direction == 2)
                    {
                        path[path.Count - 1] -= 200;
                    }
                    else
                    {
                        path.Add(-200);
                        horizontal = true;
                    }
                }

                end = returnAdj(cyclingPath, cyclingPath.GetComponent<Numberpick>().num, direction);

                if (end)
                    break;
                else
                    cyclingPath = nextPath(cyclingPath, direction);
            }
        }
        paths.Add(path);
    }

    private int returnDir(GameObject obj, int num)
    {
        int[] d = new int[4];

        if (num == 1)
        {
            d = obj.GetComponent<Path>().direction;
        }
        else if (num == 2)
        {
            d = obj.GetComponent<PathL>().direction;
        }
        else if (num == 3)
        {
            d = obj.GetComponent<PathT>().direction;
        }
        else
        {
            d = obj.GetComponent<PathC>().direction;
        }

        for (int x = 0; x < 4; x++)
        {
            if (d[x] == 1)
                return x;
        }
        Debug.Log("No old connection found");
        return 100;
    }
    private bool returnAdj(GameObject obj, int num, int y)
    {
        int[] a = new int[4];

        if (num == 1)
        {
            a = obj.GetComponent<Path>().adjacent;
        }
        else if (num == 2)
        {
            a = obj.GetComponent<PathL>().adjacent;
        }
        else if (num == 3)
        {
            a = obj.GetComponent<PathT>().adjacent;
        }
        else
        {
            a = obj.GetComponent<PathC>().adjacent;
        }

        for (int x = 0; x < 4; x++)
        {
            //Debug.Log("adjacent is " + a[x] + " and direction is " + y);
            if (a[x] == 5 && y == x)
                return true;
        }
        
        return false;
    }

    private GameObject nextPath(GameObject ob, int direction) //up right down left
    {
        int xx = 0;
        int yy = 0;

        if (direction == 0)
        {
            yy = 200;
        }
        else if (direction == 1)
        {
            xx = 200;
        }
        else if (direction == 2)
        {
            yy = -200;
        }
        else
        {
            xx = -200;
        }

        var objects = GameObject.FindGameObjectsWithTag("path");

        foreach (var obj in objects) 
        {
            if(ob.transform.position.x + xx == obj.transform.position.x && ob.transform.position.y + yy == obj.transform.position.y) 
            {
                return obj;
            }
        }
        Debug.Log("could not find corresponding adjacent path");
        return ob;
    }
}
