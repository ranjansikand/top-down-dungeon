using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] bool toggledButton = false;
    [SerializeField] List<GameObject> doors = new List<GameObject>();
    [SerializeField, Range(0, 2f)] float depressAmount;
    [SerializeField] float unlockDelay;

    bool hasBeenPushed = false;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && !hasBeenPushed)
        {
            hasBeenPushed = true;
            Invoke(nameof(Unlock), unlockDelay);
            transform.position -= new Vector3(0, depressAmount, 0);
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player" && toggledButton)
        {
            Invoke(nameof(ResetButton), 0.1f);
        }
    }

    void ResetButton()
    {
        hasBeenPushed = false;
        transform.position += new Vector3(0, depressAmount, 0);
    }

    void Unlock()
    {
        for (int i = 0; i < doors.Count; i++)
        {
            doors[i].GetComponent<Door>().Toggle();
        }
    }
}
