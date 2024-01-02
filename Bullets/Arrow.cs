using System;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    
    private float time;
    private bool test;
    private float dist;
    private int lifeSpan;
    private bool beingDestroyed;
    private Vector3 height;
    private float lastPass;
    private bool hasPassed;

    private Vector3 start;
    public Vector3 end;
    public float damage;

    public float bulletSpeed;
    public SpriteRenderer spriteRenderer;
    
    void Start()
    {
        if (gameObject.name.Contains("(Clone)"))
        {
            test = true;
            time = 0;
            lifeSpan = 6;
            beingDestroyed = false;
            //height = new Vector3 (0, Vector3.Distance (transform.position, end), 0);
            dist = 0;
            start = transform.position;
            hasPassed = false;
            lastPass = 10000;
        }
        else
            test = false;
    }

    
    void Update()
    {
        if (test)
        {
            heightChange();
            time += Time.deltaTime;
            dist = Vector3.Distance (transform.position, end);
            if (hasPassed)
            {
                transform.position += (end - height - start).normalized * Time.deltaTime * bulletSpeed;
                point(end - height - start);
            }
            else
            {
                transform.position += (end + height - start).normalized * Time.deltaTime * bulletSpeed;
                point(end + height - start);
            }
            
            if (time >= lifeSpan)
            {
               Destroy(gameObject); 
            }
        }
    }

    private void heightChange()
    {
        float totalDist = Vector3.Distance (start, end);
        if (!hasPassed)
        {
            height.y = dist - totalDist/2;

            float pass = Vector3.Distance (transform.position, end);
            if (pass < lastPass)
                lastPass = pass;
            else   
                hasPassed = true;
        }
        else
            height.y = dist + totalDist/2; 
    }

    private void point(Vector3 point)
    {
        float angle = Vector3.Angle(Vector3.right, point.normalized);

        if (point.y < 0)
        {
            angle = 360 - angle;
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (test)
        {
            if (collider.tag == "enemy" && !beingDestroyed)
            {
                collider.GetComponent<Enemy>().health -= damage;
                Destroy(gameObject);
                beingDestroyed = true;
            }
        }
    }
    void LateUpdate () 
    {
        spriteRenderer.sortingOrder = (int)Camera.main.WorldToScreenPoint (spriteRenderer.bounds.min).y * -1;
    }
}
