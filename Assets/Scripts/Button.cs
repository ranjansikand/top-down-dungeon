using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] GameObject linkedDoor;
    [SerializeField, Range(0, 2f)] float depressAmount;
    [SerializeField] float unlockDelay;

    public bool hasBeenPushed = false;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && !hasBeenPushed)
        {
            hasBeenPushed = true;
            Invoke(nameof(Unlock), unlockDelay);
            transform.position -= new Vector3(0, depressAmount, 0);
        }
    }

    void Unlock()
    {
        linkedDoor.GetComponent<Door>().Unlock();
    }
}
