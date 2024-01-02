using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public bool test;
    public int collnumb;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.name.Contains("(Clone)"))
        {
            test = true;


            GameObject.Find("Selected").GetComponent<Selected>().holding = true;
        }
        else
            test = false;
    }

    // Update is called once per frame
    void Update()
    { 

        if(test)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            mouseWorldPosition.z = 0;

            transform.position = mouseWorldPosition; 

            if(Input.GetMouseButtonDown(0) && gameObject.GetComponent<Renderer>().material.color == Color.white)
            {
                test = false;
                /*Ri detection = gameObject.GetComponent<Ri>();
                detection.DisableCollision();*/
                Destroy(gameObject.GetComponent<Rigidbody2D>());

                GameObject.Find("Selected").GetComponent<Selected>().holding = false;
            }

            if (Input.inputString == "\b")
            {
                Destroy(gameObject);
                GameObject.Find("Selected").GetComponent<Selected>().holding = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        collnumb ++;
        if(test)
            gameObject.GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        
        //GameObject col = collider.gameObject;
        //Debug.Log("Collided with: " + col);
    }

    void OnTriggerExit2D(Collider2D collider)
     {
        collnumb --;
        if(test && collnumb == 0)
            gameObject.GetComponent<Renderer>().material.color = Color.white;
     }
}
