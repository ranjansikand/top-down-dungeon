using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIK : MonoBehaviour
{
    [SerializeField] Transform followPoint;

    void Update()
    {
        // stick to point
       transform.position = followPoint.position;
    }
}
