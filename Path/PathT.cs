using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathT : MonoBehaviour
{
    private bool placed;
    public int rotate;
    private bool startPlace;
    private bool locked;
    private int side;
    private GameObject closeobj;
    public int collnumb;
    private SpriteRenderer path;
    public int num;
    public int[] adjacent = new int[4] {0, 0, 0, 0}; //stores what paths are adj. 0 nothing, 1 path, 2 corner, 3 t, 4 cross, 5 base. order: up right down left
    public int[] direction = new int[4]; // stores what side is linked to adj and what is going out. 0 nothing/not connected, 1 old (connected from), 2 new (open to connect). order: up right down left
    private bool firstTime;

    private List<GameObject> collObj;
    
    public GameObject objToSpawn;

    void Start()
    {
        placed = false;
        startPlace = false;
        rotate = 0;
        locked = false;
        closeobj = GameObject.Find("PathT");
        collnumb = 0;
        path = gameObject.GetComponent<SpriteRenderer>();
        num = 3;
        collObj = new List<GameObject>();
        firstTime = true;

        if (gameObject.name.Contains("(Clone)"))
        {
            GameObject.Find("Selected").GetComponent<Selected>().holding = true;
            GameObject.Find("Selected").GetComponent<Selected>().holdingObj = gameObject;
        }
    }

    void Update()
    {
        if(!placed && gameObject.name.Contains("(Clone)"))
        {
            GameObject go = GameObject.Find("Base");
            Base cs = go.GetComponent<Base>();
            bool baseadj = cs.adj;
            
            if (Input.GetKeyDown("r"))
            {
                rotate += 1;
                transform.Rotate( new Vector3( 0, 0, 90 ));
            }
            
            if (Input.inputString == "\b")
            {
                Destroy(gameObject);
                allowSpawns();
            }

            if(!baseadj)
            {
                int baseRotate = cs.rotate;

                if(!startPlace)
                {
                    if(baseRotate % 4 == 0)//down
                    {
                        Vector3 pos = new Vector3(go.transform.position.x, go.transform.position.y - 200, 0f);
                        transform.position = pos;
                        startPlace = true;
                    }

                    else if(baseRotate % 4 == 1)//right
                    {
                        Vector3 pos = new Vector3(go.transform.position.x + 200, go.transform.position.y, 0f);
                        transform.position = pos;
                        startPlace = true;
                    }

                    else if(baseRotate % 4 == 2)//up
                    {
                        Vector3 pos = new Vector3(go.transform.position.x, go.transform.position.y + 200, 0f);
                        transform.position = pos;
                        startPlace = true;
                    }

                    else if(baseRotate % 4 == 3)//left
                    {
                        Vector3 pos = new Vector3(go.transform.position.x - 200, go.transform.position.y, 0f);
                        transform.position = pos;
                        startPlace = true;
                    }
                }

                if(Input.GetMouseButtonDown(0))
                {
                    if(rotate % 4 == 0 && (baseRotate % 4 == 3 | baseRotate % 4 == 2 | baseRotate % 4 == 0)) //hole left
                    {
                        cs.adj = true;
                        if(baseRotate % 4 == 0)
                        {
                            adjacent[0] = 5;
                            side = 0;
                        }
                        else if(baseRotate % 4 == 1) 
                        {
                            adjacent[3] = 5;
                            side = 1;
                        }
                        else if(baseRotate % 4 == 2)
                        {
                            adjacent[2] = 5;
                            side = 2;
                        }
                        else
                        {
                            adjacent[1] = 5;
                            side = 3;
                        }
                        placing();
                    }
                    else if(rotate % 4 == 1 && (baseRotate % 4 == 1 | baseRotate % 4 == 3 | baseRotate % 4 == 0)) //hole down
                    {
                        cs.adj = true;
                        if(baseRotate % 4 == 0)
                        {
                            adjacent[0] = 5;
                            side = 0;
                        }
                        else if(baseRotate % 4 == 1) 
                        {
                            adjacent[3] = 5;
                            side = 1;
                        }
                        else if(baseRotate % 4 == 2)
                        {
                            adjacent[2] = 5;
                            side = 2;
                        }
                        else
                        {
                            adjacent[1] = 5;
                            side = 3;
                        }
                        placing();
                    }
                    else if(rotate % 4 == 2 && (baseRotate % 4 == 1 | baseRotate % 4 == 2 | baseRotate % 4 == 0)) //hole right
                    {
                        cs.adj = true;
                        if(baseRotate % 4 == 0)
                        {
                            adjacent[0] = 5;
                            side = 0;
                        }
                        else if(baseRotate % 4 == 1) 
                        {
                            adjacent[3] = 5;
                            side = 1;
                        }
                        else if(baseRotate % 4 == 2)
                        {
                            adjacent[2] = 5;
                            side = 2;
                        }
                        else
                        {
                            adjacent[1] = 5;
                            side = 3;
                        }
                        placing();
                    }
                    else if(rotate % 4 == 3 && (baseRotate % 4 == 1 | baseRotate % 4 == 3 | baseRotate % 4 == 2)) //hole up
                    {
                        cs.adj = true;
                        if(baseRotate % 4 == 0)
                        {
                            adjacent[0] = 5;
                            side = 0;
                        }
                        else if(baseRotate % 4 == 1) 
                        {
                            adjacent[3] = 5;
                            side = 1;
                        }
                        else if(baseRotate % 4 == 2)
                        {
                            adjacent[2] = 5;
                            side = 2;
                        }
                        else
                        {
                            adjacent[1] = 5;
                            side = 3;
                        }
                        placing();
                    }
                }
            }

            else
            {
                if (!locked)
                {
                    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    mouseWorldPosition.z = -1;

                    transform.position = mouseWorldPosition; 
                    
                    if (Input.GetMouseButtonDown(0))
                    {
                        var objects = GameObject.FindGameObjectsWithTag("path");
                        float closest = 1000000000;

                        foreach (var obj in objects) 
                        {
                            float dist = Vector3.Distance(obj.transform.position, transform.position);
                            if (dist < closest && dist != 0)
                            {
                                closest = dist;
                                closeobj = obj;
                            }
                        }
                        
                        Vector3 pos = new Vector3(closeobj.transform.position.x, closeobj.transform.position.y - 200, -1);
                        transform.position = pos;
                        locked = true;
                        side = 0;//down
                    }
                }

                else
                {
                    if(collObj.Contains(closeobj))
                    {
                        collObj.Remove(closeobj);
                        collnumb --;
                        if (collnumb == 0)
                        {
                            path.color = new Color(1,1,1);
                        }
                    }
                    if (Input.GetKeyDown("t"))
                    {
                        side += 1;

                        if(side % 4 == 0)//down
                        {
                            Vector3 pos = new Vector3(closeobj.transform.position.x, closeobj.transform.position.y - 200, -1);
                            transform.position = pos;
                        }

                        else if(side % 4 == 1)//right
                        {
                            Vector3 pos = new Vector3(closeobj.transform.position.x + 200, closeobj.transform.position.y, -1);
                            transform.position = pos;
                        }

                        else if(side % 4 == 2)//up
                        {
                            Vector3 pos = new Vector3(closeobj.transform.position.x, closeobj.transform.position.y + 200, -1);
                            transform.position = pos;
                        }

                        else if(side % 4 == 3)//left
                        {
                            Vector3 pos = new Vector3(closeobj.transform.position.x - 200, closeobj.transform.position.y, -1);
                            transform.position = pos;
                        }
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (closeobj.name.Contains("PathCross"))
                        {
                            //
                            if(rotate % 4 == 0 && (side % 4 == 3 | side % 4 == 2 | side % 4 == 0) && collnumb == 0)
                            {
                                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                                placing();
                            }
                            else if(rotate % 4 == 1 && (side % 4 == 1 | side % 4 == 3 | side % 4 == 0) && collnumb == 0)
                            {
                                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                                placing();
                            }
                            else if(rotate % 4 == 2 && (side % 4 == 1 | side % 4 == 2 | side % 4 == 0) && collnumb == 0)
                            {
                                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                                placing();
                            }
                            else if(rotate % 4 == 3 && (side % 4 == 1 | side % 4 == 3 | side % 4 == 2) && collnumb == 0)
                            {
                                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                                placing();
                            }
                        }

                        else if (closeobj.name.Contains("PathT"))
                        {
                            PathT script = closeobj.GetComponent<PathT>();
                            int closerotate = script.rotate;
                            //
                            if (side % 4 == 0 && rotate % 4 != 3 && closerotate % 4 != 1 && collnumb == 0)//down, not hole up, and other not hole down
                            {
                                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                                placing();
                            }
                            else if (side % 4 == 1 && rotate % 4 != 0 && closerotate % 4 != 2 && collnumb == 0)//right, not hole left, and other not right
                            {
                                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                                placing();
                            }
                            else if (side % 4 == 2 && rotate % 4 != 1 && closerotate % 4 != 3 && collnumb == 0)//up, not hole down, and other not hole up
                            {
                                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                                placing();
                            }
                            else if (side % 4 == 3 && rotate % 4 != 2 && closerotate % 4 != 0 && collnumb == 0)//left, not hole right, and other not left
                            {
                                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                                placing();
                            }
                        }

                        else if (closeobj.name.Contains("PathL"))
                        {
                            PathL script = closeobj.GetComponent<PathL>();
                            int closerotate = script.rotate;
                            //
                            if (side % 4 == 0 && rotate % 4 != 3 && (closerotate % 4 == 1 | closerotate % 4 == 0) && collnumb == 0)//down, and other not hole down
                            {
                                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                                placing();
                            }
                            else if (side % 4 == 1 && rotate % 4 != 0 && (closerotate % 4 == 1 | closerotate % 4 == 2) && collnumb == 0)//right, and other not right
                            {
                                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                                placing();
                            }
                            else if (side % 4 == 2 && rotate % 4 != 1 && (closerotate % 4 == 2 | closerotate % 4 == 3) && collnumb == 0)//up, and other not hole up
                            {
                                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                                placing();
                            }
                            else if (side % 4 == 3 && rotate % 4 != 2 && (closerotate % 4 == 0 | closerotate % 4 == 3) && collnumb == 0)//left, and other not left
                            {
                                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                                placing();
                            }
                        }

                        else
                        {
                            Path script = closeobj.GetComponent<Path>();
                            bool closerotate = script.horizontal;
                            //
                            if (closerotate && ((side % 4 == 1 && rotate % 4 != 0) | (side % 4 == 3 && rotate % 4 != 2)) && collnumb == 0)
                            {
                                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                                placing();
                            }
                            else if (!closerotate && ((side % 4 == 0 && rotate % 4 != 3) | (side % 4 == 2 && rotate % 4 != 1)) && collnumb == 0)
                            {
                                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                                placing();
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!placed)
        {
            if (collider.tag != "bullet" && collider.tag != "enemy")
            {
                GameObject col = collider.gameObject;

                if (col != closeobj)
                {
                    collnumb ++;
                    path.color = new Color(1,0,0);
                    collObj.Add(col);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (!placed)
        {
            if (collider.tag != "bullet" && collider.tag != "enemy")
            {
                GameObject col = collider.gameObject;
                
                if (col != closeobj)
                {
                    collnumb --;
                    collObj.Remove(col);
                }

                if (collnumb == 0)
                {
                    path.color = new Color(1,1,1);
                }
            }
        }
    }

    public void storeLocal()
    {
        for (int x = 0; x < 4; x++)
        {
            var objects = GameObject.FindGameObjectsWithTag("path");

            int xx, yy;

            if (x == 0)
            {
                xx = 0;
                yy = 200;
            }
            else if (x == 1)
            {
                xx = 200;
                yy = 0; 
            }
            else if (x == 2)
            {
                xx = 0;
                yy = -200; 
            }
            else
            {
                xx = -200;
                yy = 0;
            }

            //Debug.Log(yy + " " + xx);

            foreach (var obj in objects) 
            {
                if(gameObject.transform.position.x + xx == obj.transform.position.x && gameObject.transform.position.y + yy == obj.transform.position.y) 
                {
                    //Debug.Log(obj + " is horizontal " + xx + "and vertical " + yy);

                    Numberpick num = obj.GetComponent<Numberpick>();
                    adjacent[x] = num.num;
                }
            }
        }
        storeDirection();
    }

    private void storeDirection() //side: down right up left, direction: up right down left// 1 con from 2 con to 0 null
    {
        //Debug.Log("rotate: " + rotate % 4 + "side: " + side % 4);
        if (rotate % 4 == 0) //⊢
        {
            if (side % 4 == 0)
            {
                direction[0] = 1;
                
                direction[3] = 0;
                direction[1] = 2;
                direction[2] = 2;
            }
            else if (side % 4 == 2)
            {
                direction[2] = 1;
                
                direction[3] = 0;
                direction[1] = 2;
                direction[0] = 2;
            }
            else //3
            {
                direction[1] = 1;
                
                direction[3] = 0;
                direction[0] = 2;
                direction[2] = 2;
            }
        }
        else if (rotate % 4 == 1) //⊥
        {
            if (side % 4 == 0)
            {
                direction[0] = 1;
                
                direction[1] = 2;
                direction[2] = 0;
                direction[3] = 2;
            }
            else if (side % 4 == 1)
            {
                direction[3] = 1;
                
                direction[2] = 0;
                direction[1] = 2;
                direction[0] = 2;
            }
            else//3
            {
                direction[1] = 1;
                
                direction[2] = 0;
                direction[0] = 2;
                direction[3] = 2;
            }
        }
        else if (rotate % 4 == 2) //⫞
        {
            if (side % 4 == 0)
            {
                direction[0] = 1;

                
                direction[1] = 0;
                direction[2] = 2;
                direction[3] = 2;
            }
            else if (side % 4 == 1)
            {
                direction[3] = 1;
                
                direction[2] = 2;
                direction[1] = 0;
                direction[0] = 2;
            }
            else//2
            {
                direction[2] = 1;

                
                direction[0] = 2;
                direction[1] = 0;
                direction[3] = 2;
            }
        }
        else // T
        {
            if (side % 4 == 1)
            {
                direction[3] = 1;

                
                direction[0] = 0;
                direction[1] = 2;
                direction[2] = 2;
            }
            else if (side % 4 == 2)
            {
                direction[2] = 1;
                
                direction[3] = 2;
                direction[1] = 2;
                direction[0] = 0;
            }
            else//3
            {
                direction[1] = 1;

                
                direction[3] = 2;
                direction[0] = 0;
                direction[2] = 2;
            }
        }
        if (firstTime)
        {
            for (int x = 0; x < 4; x++)
            {
                //Debug.Log("x" + x + "direction x:" + direction[x]);
                if (direction[x] == 2)
                {
                    spawnPortal(x);
                }
            }
            firstTime = false;
        }
    }

    private void storeAdj()
    {
        var objects = GameObject.FindGameObjectsWithTag("path");
        foreach (var obj in objects) 
        {
            if((gameObject.transform.position.x + 200 == obj.transform.position.x && gameObject.transform.position.y == obj.transform.position.y) | (gameObject.transform.position.x - 200 == obj.transform.position.x && gameObject.transform.position.y == obj.transform.position.y) | (gameObject.transform.position.x == obj.transform.position.x && gameObject.transform.position.y + 200 == obj.transform.position.y) | (gameObject.transform.position.x == obj.transform.position.x && gameObject.transform.position.y - 200 == obj.transform.position.y)) 
            {
                Numberpick num = obj.GetComponent<Numberpick>();
                if (num.num == 1)
                {
                    obj.GetComponent<Path>().storeLocal();
                }
                else if (num.num == 2)
                {
                    obj.GetComponent<PathL>().storeLocal();
                }
                else if (num.num == 3)
                {
                    obj.GetComponent<PathT>().storeLocal();
                }
                else
                {
                    obj.GetComponent<PathC>().storeLocal();
                }
            }
        }
    }
    private void spawnPortal(int portSide)
    {
        //Debug.Log(portSide);
        GameObject portal = GameObject.Find("Portal");

        if (portSide == 0)
        {
            var pos = new Vector3(transform.position.x, transform.position.y + 100, -0.5f - portal.GetComponent<Portal>().overlap());
            GameObject g = Instantiate(objToSpawn, pos, Quaternion.identity);
            g.transform.Rotate( new Vector3( 0, 0, 90 ));
        }
        else if (portSide == 1)
        {
            var pos = new Vector3(transform.position.x + 100, transform.position.y, -0.5f - portal.GetComponent<Portal>().overlap());
            GameObject g = Instantiate(objToSpawn, pos, Quaternion.identity);
        }
        else if (portSide == 2)
        {
            var pos = new Vector3(transform.position.x, transform.position.y - 100, -0.5f - portal.GetComponent<Portal>().overlap());
            GameObject g = Instantiate(objToSpawn, pos, Quaternion.identity);
            g.transform.Rotate( new Vector3( 0, 0, 270 ));
        }
        else
        {
            var pos = new Vector3(transform.position.x - 100, transform.position.y, -0.5f - portal.GetComponent<Portal>().overlap());
            GameObject g = Instantiate(objToSpawn, pos, Quaternion.identity);
            g.transform.Rotate( new Vector3( 0, 0, 180 ));
        }
    }
    private void allowSpawns()
    {
        GameObject.Find("Selected").GetComponent<Selected>().holding = false;
    }
    private void deletePort()
    {
        if (side % 4 == 0)
        {
            var objects = GameObject.FindGameObjectsWithTag("portal");
            foreach (var obj in objects) 
            {
                if(gameObject.transform.position.x == obj.transform.position.x && gameObject.transform.position.y + 100 == obj.transform.position.y)
                {
                    Destroy(obj);
                    Portal sc = obj.GetComponent<Portal>();
                    sc.beingDestroyed = true;
                }
            }
        }
        else if (side % 4 == 1)
        {
            var objects = GameObject.FindGameObjectsWithTag("portal");
            foreach (var obj in objects) 
            {
                if(gameObject.transform.position.x - 100 == obj.transform.position.x && gameObject.transform.position.y == obj.transform.position.y)
                {
                    Destroy(obj);
                    Portal sc = obj.GetComponent<Portal>();
                    sc.beingDestroyed = true;
                }
            }
        }
        else if (side % 4 == 2)
        {
            var objects = GameObject.FindGameObjectsWithTag("portal");
            foreach (var obj in objects) 
            {
                if(gameObject.transform.position.x == obj.transform.position.x && gameObject.transform.position.y - 100 == obj.transform.position.y)
                {
                    Destroy(obj);
                    Portal sc = obj.GetComponent<Portal>();
                    sc.beingDestroyed = true;
                }
            }
        }
        else
        {
            var objects = GameObject.FindGameObjectsWithTag("portal");
            foreach (var obj in objects) 
            {
                if(gameObject.transform.position.x + 100 == obj.transform.position.x && gameObject.transform.position.y == obj.transform.position.y)
                {
                    Destroy(obj);
                    Portal sc = obj.GetComponent<Portal>();
                    sc.beingDestroyed = true;
                }
            }
        }
    }
    private void placing()
    {
        placed = true;
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        storeLocal();
        storeAdj();
        allowSpawns();
        deletePort();
        GameObject Pathfinder = GameObject.Find("Pathfinder");
        Pathfinder.GetComponent<Pathfinder>().pathfinder();
    }
}
