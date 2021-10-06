using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : MonoBehaviour
{
    [SerializeField] Camera cam;

    void Awake()
    {
        if (cam == null) cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        // aim at mouse position
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerPos = cam.WorldToScreenPoint(transform.position);
        Vector3 lookDir = mousePos - playerPos;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(-angle, Vector3.up);
    }
}
