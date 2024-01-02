using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selected : MonoBehaviour
{
    public bool holding;
    public GameObject holdingObj;
    public GameObject selectedObj;
    public bool selected;

    void Start()
    {
        selected = false;
        holding = true;
        selectedObj = null;
        holdingObj = GameObject.Find("Base");
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !holding)
        {
            var towers = GameObject.FindGameObjectsWithTag("tower");
            float closest = 1000000000;
            GameObject close = null;
            Vector2 mouseWorldPosition = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            foreach(GameObject tower in towers)
            {
                float dist = Vector3.Distance(tower.transform.position, new Vector3 (mouseWorldPosition.x, mouseWorldPosition.y, 0));
                
                if (dist < closest && dist <= 50)
                {
                    closest = dist;
                    close = tower;
                }
            }
            if (closest < 51)
            {
                transform.position = close.transform.position;
                selectedObj = close;
                selected = true;
                transformScale(close, close.name);
                
            }
            else
            {
                selected = false;
            }
        }
        if (holding)
        {
            selected = false;
            if (holdingObj.tag == "tower")
            {
                transform.position = holdingObj.transform.position;
                transformScale(holdingObj, holdingObj.name);
            }
        }
        if (!selected && transform.position.z != -100 && ((holdingObj.tag != "tower" && holding) | !holding))
        {
            transform.position = new Vector3 (transform.position.x, transform.position.y, -100);
        }
    }

    private void transformScale(GameObject close, string name)
    {
        if (name == "ArcherTower(Clone)")
            transform.localScale = new Vector3(close.GetComponent<Archer>().range, close.GetComponent<Archer>().range, 1);
        else if (name == "Cannon(Clone)")
            transform.localScale = new Vector3(close.GetComponent<Cannon>().range, close.GetComponent<Cannon>().range, 1);
        else if (name == "Tesla(Clone)")
            transform.localScale = new Vector3(close.GetComponent<Tesla>().range, close.GetComponent<Tesla>().range, 1);
    }
}
