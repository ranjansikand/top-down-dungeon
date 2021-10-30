using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public GameObject mainCamera;

    void Start()
    {
        if (mainCamera == null) mainCamera = GameObject.FindWithTag("MainCamera");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.transform.forward);
    }
}
