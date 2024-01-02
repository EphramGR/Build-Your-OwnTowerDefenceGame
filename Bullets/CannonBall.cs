using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private bool test;
    public float damage;
    public Vector3 velocity;
    private float time;
    private int lifeSpan;
    private bool beingDestroyed;

    public SpriteRenderer spriteRenderer;


    void Start()
    {
        if (gameObject.name.Contains("(Clone)"))
        {
            test = true;
            time = 0;
            lifeSpan = 3;
            beingDestroyed = false;
        }
        else
            test = false;
    }

    void Update()
    {
        if (test)
        {
            time += Time.deltaTime;
            transform.position += velocity * Time.deltaTime;

            if (time >= lifeSpan)
            {
               Destroy(gameObject); 
            }
        }
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
