using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayerCam : MonoBehaviour
{
    Camera mainCam;
    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        var rotation = mainCam.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
    }
}
