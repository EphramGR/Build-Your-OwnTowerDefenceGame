using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    void Start()
    {
        if (gameObject.name.Contains("(Clone)"))
        {
            ParticleSystem ps = GetComponent<ParticleSystem>();
            var main = ps.main;

            main.stopAction = ParticleSystemStopAction.Destroy;
        }
    }
    //emmision rate over time high but 0 rate over distance makes it buigger closer, so do that for range guy, //wrongstill high
}
