using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objToSpawn;
    private bool test;
    
    public void Spawn()
    {
        test = GameObject.Find("Selected").GetComponent<Selected>().holding;

        if (!test)

        {
        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;

        transform.position = mouseWorldPosition;

        Instantiate(objToSpawn, transform.position, Quaternion.identity);
        }
    }
}
