using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmText : MonoBehaviour
{
    public GameObject CanvasGameObject;

    // Start is called before the first frame update
    void Start()
    {
        CanvasGameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject go = GameObject.Find("Base");
        Base cs = go.GetComponent<Base>();
        bool working = cs.textRender;

        if(working)
        {
            CanvasGameObject.SetActive(true);
        }
        else
        {
            CanvasGameObject.SetActive(false);
        }
    }
}
