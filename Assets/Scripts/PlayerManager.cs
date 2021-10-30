using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] List<GameObject> characters = new List<GameObject>();
    [SerializeField] GameObject resurrectionLight;

    Vector3 startPos;
    GameObject player;

    void Awake()
    {
        startPos = GameObject.Find("Idol").transform.position + Vector3.forward;

        Instantiate(resurrectionLight, startPos, new Quaternion(-0.707106829f,0,0,0.707106829f));
        Invoke(nameof(LoadPlayerIntoScene), 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadPlayerIntoScene()
    {
        Instantiate(characters[Random.Range(0, characters.Count)], startPos, Quaternion.identity);
    }

    void ResetPlayer()
    {
        player.transform.position = startPos;
    }
}
