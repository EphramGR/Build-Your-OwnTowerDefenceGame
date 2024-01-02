using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public bool test;
    public int collnumb;
    private bool placed;
    public int range;

    public Sprite cannon1;
    public Sprite cannon2;
    public Sprite cannon3;
    public Sprite cannon4;
    public Sprite cannon5;
    public Sprite cannon6;
    public Sprite cannon7;
    public Sprite cannon8;
    public Sprite cannon9;
    public Sprite cannon10;
    public Sprite cannon11;
    public Sprite cannon12;
    public Sprite cannon13;
    public Sprite cannon14;

    public SpriteRenderer spriteRenderer;

    private List<GameObject> inRange;

    public GameObject bullet;

    private float time;
    private float firerate;
    private float bulletSpeed;
    private float damage;


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

            damage = 100;

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
                
                    if (7.5f <= angle && angle < 30) //180, 220, 260, 290, 305, 345, 25, 35, 50, 70, 80, 95, 120, 135
                    {
                        spriteRenderer.sprite = cannon7;
                    }
                    else if (30 <= angle && angle < 42.5f) 
                    {
                        spriteRenderer.sprite = cannon8;
                    }
                    else if (42.5f <= angle && angle < 60) 
                    {
                        spriteRenderer.sprite = cannon9;
                    }  
                    else if (60 <= angle && angle < 75) 
                    {
                        spriteRenderer.sprite = cannon10;
                    }
                    else if (75 <= angle && angle < 87.5f) 
                    {
                        spriteRenderer.sprite = cannon11;
                    }
                    else if (87.5f <= angle && angle < 107.5f) 
                    {
                        spriteRenderer.sprite = cannon12;
                    }
                    else if (107.5f <= angle && angle < 127.5f) 
                    {
                        spriteRenderer.sprite = cannon13;
                    }
                    else if (127.5f <= angle && angle < 157.5f) 
                    {
                        spriteRenderer.sprite = cannon14;
                    }
                    else if (157.5f <= angle && angle < 200) 
                    {
                        spriteRenderer.sprite = cannon1;
                    }
                    else if (200 <= angle && angle < 240) 
                    {
                        spriteRenderer.sprite = cannon2;
                    }
                    else if (240 <= angle && angle < 275) 
                    {
                        spriteRenderer.sprite = cannon3;
                    }
                    else if (275 <= angle && angle < 297.5f) 
                    {
                        spriteRenderer.sprite = cannon4;
                    }
                    else if (297.5f <= angle && angle < 325) 
                    {
                        spriteRenderer.sprite = cannon5;
                    }
                    else if ((325 <= angle && 360 >= angle) | (angle >= 0 && angle < 7.5f)) 
                    {
                        spriteRenderer.sprite = cannon6;
                    }

                    if (time >= firerate)
                    {
                        time = 0.0f;

                        GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
                        b.GetComponent<CannonBall>().velocity = (farthest.transform.position - transform.position).normalized * bulletSpeed;
                        b.GetComponent<CannonBall>().damage = damage;
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
