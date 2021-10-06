using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] float maxLifetime = 3f;

    [SerializeField] GameObject destroyEffect;

    public Vector3 destination;
    public float speed;

    void Update()
    {
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0) Invoke("Delay", 0.05f);

        if (destination != null) transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }

    public void Destination(Vector3 target)
    {
        destination = target;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer != 1) Invoke("Delay", 0.05f);
    }

    void Delay()
    {
        if (destroyEffect != null) Instantiate(destroyEffect, transform.position, new Quaternion(0.707106829f,0,0,0.707106829f));
        Destroy(gameObject);
    }
}
