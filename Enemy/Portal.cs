using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    //private bool copy;
    //public GameObject conPath;
    //public int pathSide;

    public bool beingDestroyed;
    private float counter;

    // Start is called before the first frame update
    void Start()
    {
        beingDestroyed = false;
        counter = 0;
    }

    public float overlap()
    {
        counter += 0.0001f;

        return counter;
    }
}
