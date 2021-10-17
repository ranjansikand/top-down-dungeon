using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingAttack : MonoBehaviour
{
    [SerializeField] float detectionRadius;
    const int layerMask = 1 << 6;
    Transform target;
    [SerializeField] float distanceDelta;
    [SerializeField] GameObject destroyEffect;
    [SerializeField] float maxLifetime = 5;

    void Update()
    {
        if (target == null) Detection();
        else transform.position = Vector3.MoveTowards(transform.position, target.position, distanceDelta);

        // anti-cheesing strategy
        maxLifetime-=Time.deltaTime;
        if (maxLifetime <= 0) Destroy(gameObject);
    }

    void Detection()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, detectionRadius, layerMask);

        if (enemies.Length > 0) target = enemies[0].gameObject.transform;

        Debug.Log("Detected: " + target);
    }

    void OnCollisionEnter()
    {
        if (destroyEffect != null) Instantiate(destroyEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
