using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float maxLifetime = 3f;

    [SerializeField] bool useDestroyEffect;
    [SerializeField] GameObject destroyEffect;

    void Update()
    {
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0) Invoke("Delay", 0.05f);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer != 1) Invoke("Delay", 0.05f);
    }

    void Delay()
    {
        if (useDestroyEffect) Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
