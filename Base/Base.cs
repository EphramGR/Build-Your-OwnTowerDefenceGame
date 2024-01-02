using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public int rotate;
    public bool placedYet;
    public bool textRender;
    public bool adj;
    private bool pretest;
    private bool posttest;
    public Sprite spriteSide;
    public Sprite spriteUp;
    public Sprite spriteDown;
    public SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        rotate = 0;

        placedYet = false;
        pretest = false;
        posttest = true;
        adj = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!placedYet)
        {
            if (!textRender)
            {
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (Input.GetKeyDown("r"))
                {
                    rotate += 1;
                    if (rotate % 4 == 0)
                    {
                        spriteRenderer.sprite = spriteDown;
                    }
                    else if (rotate % 4 == 2)
                    {
                        spriteRenderer.sprite = spriteUp;
                    }
                    else
                    {
                        spriteRenderer.sprite = spriteSide;
                    }
                }

                mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                mouseWorldPosition.z = -0.9f;

                transform.position = mouseWorldPosition; 
            }
            if(pretest)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    textRender = false;
                    placedYet = true;
                    posttest = false;
                    GameObject.Find("Selected").GetComponent<Selected>().holding = false;
                }
                else if(Input.inputString == "\b")
                {
                    textRender = false;
                    pretest = false;
                }
            }

            if(Input.GetMouseButtonDown(0) && posttest)
            {
                textRender = true;
                pretest = true;
            }
        }
    }
}
