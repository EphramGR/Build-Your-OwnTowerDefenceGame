using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    public bool test;
    public int collnumb;
    private bool placed;
    public int range;

    private List<GameObject> inRange;

    public GameObject bullet;

    private float time;
    private float firerate;
    private float bulletSpeed;
    private float damage;

    public SpriteRenderer spriteRenderer;
    
    void Start()
    {
        placed = false;

        if (gameObject.name.Contains("(Clone)"))
        {
            test = true;

            time = 10;

            inRange = new List<GameObject>();

            firerate = 1.5f;
            bulletSpeed = 750;

            damage = 34;

            GameObject.Find("Selected").GetComponent<Selected>().holding = true;
            GameObject.Find("Selected").GetComponent<Selected>().holdingObj = gameObject;
        }
        else
            test = false;
    }

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
                placed = true;
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
        if (placed)
        {
            time += Time.deltaTime;

            inRange.Clear();
            var objects = GameObject.FindGameObjectsWithTag("enemy");

            foreach (GameObject obj in objects)
            {
                if (Vector2.Distance(new Vector2(obj.transform.position.x, obj.transform.position.y), new Vector2(transform.position.x, transform.position.y)) <= range)
                {
                    inRange.Add(obj);
                }
            }

            if(inRange.Count > 0)
            {
                float big = 100000000;
                GameObject farthest = gameObject;
                float angle = 361;
                foreach(GameObject obj in inRange)
                {
                    float current = obj.GetComponent<Enemy>().getDistRemaining();
                    if (current < big)
                    {
                        //Debug.Log(current + " is less then " + big + "so it is the new target");
                        big = current;
                        farthest = obj;
                    }
                }
                
                if (farthest != gameObject)
                {
                    angle = Vector3.Angle(transform.up, farthest.transform.position - transform.position);

                    if (farthest.transform.position.x - transform.position.x < 0)
                    {
                        angle = 360 - angle;
                    }

                    if (time >= firerate)
                    {
                        time = 0.0f;

                        GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
                        b.GetComponent<Arrow>().end = farthest.transform.position;
                        b.GetComponent<Arrow>().damage = damage;
                        b.GetComponent<Arrow>().bulletSpeed = bulletSpeed;
                    }
                }   
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
    void LateUpdate () 
    {
        spriteRenderer.sortingOrder = (int)Camera.main.WorldToScreenPoint (spriteRenderer.bounds.min).y * -1;
    }
}
