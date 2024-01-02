using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    private bool firsttry;

    // Start is called before the first frame update
    void Start()
    {
        firsttry = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (firsttry)
        {
            try
            {
                GameObject go = GameObject.Find("Voxel Grid");
                VoxelGrid cs = go.GetComponent<VoxelGrid>();
                Color color = cs.backgroundColor;

                transform.GetComponent<Renderer>().material.color = color;

            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

        }
    }
}
