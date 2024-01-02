using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{

    private GameObject b;

    private Base cs;
    
    void Start()
    {
        b = GameObject.Find("Base");
        cs = b.GetComponent<Base>();
    }

   
    void Update()
    {
        transform.position = b.transform.position;
        if (cs.rotate % 4 == 0)
        {
            transform.position = new Vector3 (transform.position.x, transform.position.y - 200, transform.position.z);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 270);
        }
        else if (cs.rotate % 4 == 1)
        {
            transform.position = new Vector3 (transform.position.x + 200, transform.position.y, transform.position.z);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        }
        else if (cs.rotate % 4 == 2)
        {
            transform.position = new Vector3 (transform.position.x, transform.position.y + 200, transform.position.z);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90);
        }
        else
        {
            transform.position = new Vector3 (transform.position.x - 200, transform.position.y, transform.position.z);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180);
        }

        if (cs.placedYet)
        {
            Destroy(gameObject);
        }
    }
}
