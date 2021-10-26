using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Bullet
{
    Transform pinTo;

    public void SetPin(Transform item)
    {
        pinTo = item;
    }

    public void RemovePin()
    {
        pinTo = null;
    }

    void LateUpdate()
    {
        if (pinTo != null) transform.position = pinTo.position;
    }
}
