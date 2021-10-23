using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float maxLifetime = 3f;

    [SerializeField] bool useDestroyEffect;
    [SerializeField] GameObject destroyEffect;

    [SerializeField] float invulnerableTime = 0;
    bool invulnerable = true;

    void Update()
    {
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0) Invoke("Delay", 0.05f);

        invulnerableTime -= Time.deltaTime;
        if (invulnerableTime <= 0) invulnerable = false;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer != 1 && !invulnerable) Invoke("Delay", 0.05f);
    }

    void Delay()
    {
        if (useDestroyEffect) Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
