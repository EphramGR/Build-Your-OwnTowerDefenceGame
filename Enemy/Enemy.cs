using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private List<int> path;
    private bool horizontal;
    private int pathPos;
    public int speed;
    private float distMoved;
    private bool test;
    public float health;

    void Start()
    {
        path = new List<int>();
        pathPos = 0;
        distMoved = 0;
        if (gameObject.name.Contains("(Clone)"))
        {
            test = true;

            GameObject enemySpawner = GameObject.Find("EnemySpawner");

            horizontal = enemySpawner.GetComponent<EnemySpawner>().pathStartingDir[enemySpawner.GetComponent<EnemySpawner>().spawnSelection];
            path = enemySpawner.GetComponent<EnemySpawner>().paths[enemySpawner.GetComponent<EnemySpawner>().spawnSelection];
            health = 100;
        }
        else
        {
            test = false;
            path.Add(1);
            //Debug.Log("das");
        }
        //Debug.Log(printList(path));
        //Debug.Log(horizontal);
    }


    void Update()
    {
        //Debug.Log(printList(path));
        //Debug.Log(horizontal + ", " + pathPos + ", " + path.Count + ", " + test);
        if (horizontal && pathPos < path.Count && test)
        {
            //Debug.Log("so far so goodL");
            if (pathPos % 2 == 0)
            {
                transform.position = new Vector3 (transform.position.x  + (speed * Time.deltaTime * Mathf.Sign(path[pathPos])), transform.position.y, transform.position.z);
                distMoved += speed * Time.deltaTime;
                //Debug.Log("so far so good");
                if (distMoved >= Mathf.Abs(path[pathPos]))
                {
                    pathPos ++;
                    distMoved = 0;
                }
            }
            else
            {
                transform.position = new Vector3 (transform.position.x, transform.position.y  + (speed * Time.deltaTime * Mathf.Sign(path[pathPos])), transform.position.z);
                distMoved += speed * Time.deltaTime;
                //Debug.Log("so far so good");
                if (distMoved >= Mathf.Abs(path[pathPos]))
                {
                    pathPos ++;
                    distMoved = 0;
                }
            }
        }
        else if (pathPos < path.Count && test)
        {
            if (pathPos % 2 == 0)
            {
                transform.position = new Vector3 (transform.position.x, transform.position.y  + (speed * Time.deltaTime * Mathf.Sign(path[pathPos])), transform.position.z);
                distMoved += speed * Time.deltaTime;
                //Debug.Log("so far so good");
                if (distMoved >= Mathf.Abs(path[pathPos]))
                {
                    pathPos ++;
                    distMoved = 0;
                }
            }
            else
            {
                transform.position = new Vector3 (transform.position.x  + (speed * Time.deltaTime * Mathf.Sign(path[pathPos])), transform.position.y, transform.position.z);
                distMoved += speed * Time.deltaTime;
                //Debug.Log("so far so good");
                if (distMoved >= Mathf.Abs(path[pathPos]))
                {
                    pathPos ++;
                    distMoved = 0;
                }
            }
        }
        else if (pathPos >= path.Count && test)
        {
            Destroy(gameObject);
        }
        if (health <= 0 && test)
        {
            Destroy(gameObject);
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

    public float getDistRemaining()
    {
        float totalDistance = 0;
        if(test)
        {
            for(int i = 0; i < path.Count; i++)
            {
                if (i == pathPos)
                {
                    totalDistance += Mathf.Abs(path[i]) - Mathf.Abs(distMoved);
                }
                else if (i > pathPos)
                {
                    totalDistance += Mathf.Abs(path[i]);
                }
            }
            //Debug.Log(totalDistance);
            return totalDistance;
        }
        else
        {
            return 100000001;
        }
    }
}
