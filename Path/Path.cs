using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    private bool placed;
    public bool horizontal;
    private bool locked;
    private int side;
    private GameObject closeobj;
    public int collnumb;
    private SpriteRenderer path;
    public int[] adjacent = new int[4] {0, 0, 0, 0}; //stores what paths are adj. 0 nothing, 1 path, 2 corner, 3 t, 4 cross, 5 base. order: up right down left
    public int[] direction = new int[4]; // stores what side is linked to adj and what is going out. 0 nothing/not connected, 1 old (connected from), 2 new (open to connect). order: up right down left
    public int num;
    private bool firstTime;

    private List<GameObject> collObj;

    public GameObject objToSpawn;

    void Start()
    {
        placed = false;
        horizontal = false;
        locked = false;
        closeobj = GameObject.Find("Path");
        collnumb = 0;
        path = gameObject.GetComponent<SpriteRenderer>();
        num = 1;
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

            if (Input.inputString == "\b")
            {
                Destroy(gameObject);
                allowSpawns();
            }
            
            if(!baseadj)
            {
                int baseRotate = cs.rotate;

                if(baseRotate % 4 == 0)//down
                {
                    Vector3 pos = new Vector3(go.transform.position.x, go.transform.position.y - 200, 0f);
                    transform.position = pos;
                    cs.adj = true;
                    adjacent[0] = 5;
                    placing();
                }

                else if(baseRotate % 4 == 1)//right
                {
                    Vector3 pos = new Vector3(go.transform.position.x + 200, go.transform.position.y, 0f);
                    transform.position = pos;
                    transform.Rotate( new Vector3( 0, 0, 90 ));
                    cs.adj = true;
                    horizontal = true;
                    adjacent[3] = 5;
                    side = 1;
                    placing();
                }

                else if(baseRotate % 4 == 2)//up
                {
                    Vector3 pos = new Vector3(go.transform.position.x, go.transform.position.y + 200, 0f);
                    transform.position = pos;
                    cs.adj = true;
                    adjacent[2] = 5;
                    side = 2;
                    placing();
                }

                else if(baseRotate % 4 == 3)//left
                {
                    Vector3 pos = new Vector3(go.transform.position.x - 200, go.transform.position.y, 0f);
                    transform.position = pos;
                    transform.Rotate( new Vector3( 0, 0, 90 ));
                    cs.adj = true;
                    horizontal = true;
                    adjacent[1] = 5;
                    side = 3;
                    placing();
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
                            transform.Rotate( new Vector3( 0, 0, 90 ));
                            horizontal = false;
                        }

                        else if(side % 4 == 1)//right
                        {
                            Vector3 pos = new Vector3(closeobj.transform.position.x + 200, closeobj.transform.position.y, -1);
                            transform.position = pos;
                            transform.Rotate( new Vector3( 0, 0, 90 ));
                            horizontal = true;
                        }

                        else if(side % 4 == 2)//up
                        {
                            Vector3 pos = new Vector3(closeobj.transform.position.x, closeobj.transform.position.y + 200, -1);
                            transform.position = pos;
                            transform.Rotate( new Vector3( 0, 0, 90 ));
                            horizontal = false;
                        }

                        else if(side % 4 == 3)//left
                        {
                            Vector3 pos = new Vector3(closeobj.transform.position.x - 200, closeobj.transform.position.y, -1);
                            transform.position = pos;
                            transform.Rotate( new Vector3( 0, 0, 90 ));
                            horizontal = true;
                        }
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (closeobj.name.Contains("PathCross"))
                        {
                            //
                            if(collnumb == 0)
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
                            if (((side % 4 == 1 && closerotate % 4 != 2) | (side % 4 == 3 && closerotate % 4 != 0)) && collnumb == 0)
                            {
                                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                                placing();
                            }
                            else if (((side % 4 == 0 && closerotate % 4 != 1) | (side % 4 == 2 && closerotate % 4 != 3)) && collnumb == 0)
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
                            if (horizontal && ((side % 4 == 1 && (closerotate % 4 == 1 | closerotate % 4 == 2)) | (side % 4 == 3 && (closerotate % 4 == 0 | closerotate % 4 == 3))) && collnumb == 0)
                            {
                                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                                placing();
                            }
                            else if (!horizontal && ((side % 4 == 0 && (closerotate % 4 == 0 | closerotate % 4 == 1)) | (side % 4 == 2 && (closerotate % 4 == 3 | closerotate % 4 == 2))) && collnumb == 0)
                            {
                                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                                placing();
                            }
                        }

                        else
                        {
                            Path script = closeobj.GetComponent<Path>();
                            bool closerotate = script.horizontal;
                            //Debug.Log("Stationary is " + closerotate + ", Rotating is " + horizontal);
                            
                            //
                            if (closerotate && horizontal && collnumb == 0)
                            {
                                transform.position = new Vector3(transform.position.x, transform.position.y, 0); 
                                placing();
                            }
                            else if (!closerotate && !horizontal && collnumb == 0)
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

        //doing what way each side goes (first thing in if statement same for all) //side: down right up left, direction: up right down left// 1 con from 2 con to 0 null
        //Debug.Log(side%4);
        if (side % 4 == 0)
        {
            direction[0] = 1;

            direction[2] = 2;
            direction[1] = 0;
            direction[3] = 0;
        }
        else if (side % 4 == 1)
        {
            direction[3] = 1;

            direction[1] = 2;
            direction[2] = 0;
            direction[0] = 0;
        }
        else if (side % 4 == 2)
        {
            direction[2] = 1;
            
            direction[0] = 2;
            direction[1] = 0;
            direction[3] = 0;
        }
        else
        {
            direction[1] = 1;
            
            direction[3] = 2;
            direction[2] = 0;
            direction[0] = 0;
        }
        if (firstTime)
        {
            for (int x = 0; x < 4; x++)
            {
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
