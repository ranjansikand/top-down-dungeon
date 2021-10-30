using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject disappearEffect;

    public bool isLocked = true;

    public void Unlock()
    {
        if (gameObject.activeSelf)
        {
            if (disappearEffect != null) Instantiate(disappearEffect, transform.position + Vector3.up, transform.rotation);
            gameObject.SetActive(false);
        }
    }

    public void Lock()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }
}
