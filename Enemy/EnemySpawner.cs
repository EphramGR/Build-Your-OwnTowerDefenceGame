using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    private List<GameObject> portals;
    public List<List<int>> paths;
    public List<bool> pathStartingDir;
    public GameObject objToSpawn;
    private int spawnNum;
    public int spawnSelection;

    // Start is called before the first frame update
    void Start()
    {
        portals = new List<GameObject>();
        paths = new List<List<int>>();
        pathStartingDir = new List<bool>();
        spawnNum = 0;
        spawnSelection = 0;
    }

    public void Spawn()
    {
        if (GameObject.Find("Base").GetComponent<Base>().adj && GameObject.Find("Selected").GetComponent<Selected>().holding == false)
        {
            GameObject Pathfinder = GameObject.Find("Pathfinder");
            portals = Pathfinder.GetComponent<Pathfinder>().portals;
            paths = Pathfinder.GetComponent<Pathfinder>().paths;
            pathStartingDir = Pathfinder.GetComponent<Pathfinder>().pathStartingDir;

            spawnSelection = spawnNum % portals.Count;

            transform.position = portals[spawnSelection].transform.position;

            transform.position = new Vector3(transform.position.x, transform.position.y, -0.6f - spawnNum/10000f);

            Instantiate(objToSpawn, transform.position, Quaternion.identity);

            spawnNum ++;
        }
        
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
}
