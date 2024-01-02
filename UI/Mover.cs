using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{

    public bool onLeft;
    private bool isOpened;
    private bool openingLeft;
    private bool openingRight;
    private bool closingLeft;
    private bool closingRight;
    private float distMoved;
    private int speed;
    private float speedTurn;
    private float rotate;

    void Start()
    {
        isOpened = false;
        openingLeft = false;
        openingRight= false;
        closingLeft= false;
        closingRight = false;
        distMoved = 0;
        speed = 750;
        speedTurn = 200;
        rotate = 0;
    }

    void Update()
    {
        //Debug.Log("2");
        if (openingLeft)
        {
            moving("left", 1);
        }
        else if(openingRight)
        {
            moving("right", -1);
        }
        else if(closingLeft)
        {
            moving("left", -1);
        }
        else if(closingRight)
        {
            moving("right", 1);
        }

        if(GameObject.Find("Selected").GetComponent<Selected>().holding == true)
        {
            if (!openingLeft && !openingRight && !closingLeft && !closingRight)
            {
                if (onLeft)
                {
                    if (isOpened)
                    {
                        closingLeft = true;
                        isOpened = false;
                    }
                }
                else
                {
                    //Debug.Log("1");
                    if (isOpened)
                    {
                        closingRight = true;
                        isOpened = false;
                    }
                }
            }
        }
    }


    public void move()
    {
        if (!openingLeft && !openingRight && !closingLeft && !closingRight && GameObject.Find("Selected").GetComponent<Selected>().holding == false)
        {
            if (onLeft)
            {
                if (isOpened)
                {
                    closingLeft = true;
                    isOpened = false;
                }
                else
                {
                    openingLeft = true;
                    isOpened = true;
                }
            }
            else
            {
                //Debug.Log("1");
                if (isOpened)
                {
                    closingRight = true;
                    isOpened = false;
                }
                else
                {
                    openingRight = true;
                    isOpened = true;
                }
            }
        }
    }

    private void moving(string side, int dir)
    {
        //Debug.Log("suc");
        var objects = GameObject.FindGameObjectsWithTag(side);

        foreach(var obj in objects)
        {
            RectTransform pos = obj.GetComponent<RectTransform>();
            pos.anchoredPosition = new Vector2(pos.anchoredPosition.x + speed * Time.deltaTime * dir, pos.anchoredPosition.y);
        }
        distMoved += speed * Time.deltaTime;
        speed += 5;

        if (rotate < 180)
        {
            GameObject arrow = GameObject.Find("Arrow"+side);
            arrow.transform.Rotate( new Vector3( 0, 0, speedTurn * Time.deltaTime ));
            rotate += speedTurn * Time.deltaTime;
        }
        speedTurn += 0.7f;
        if (distMoved >= 1250)
        {
            openingLeft = false;
            openingRight= false;
            closingLeft= false;
            closingRight = false;
            distMoved = 0;
            rotate = 0;
            speed = 750;
            speedTurn = 200;
        }
        
    }
}
