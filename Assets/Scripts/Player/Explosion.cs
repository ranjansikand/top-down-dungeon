using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Explosion stats")]
    [SerializeField] float explosionRadius, explosionForce, duration = -1;
    [SerializeField] LayerMask enemyLayer;

    [Header("Effects")]
    [SerializeField] GameObject explosion;

    bool exploded, deleted;


    // Update is called once per frame
    void Update()
    {
        if (exploded)
        {
            if (duration <= 0 && !deleted)
            {
                deleted = true;
                Invoke("Delay", 0.05f);
            }
            else 
            {
                Explode();
                duration -= Time.deltaTime;
            }
        }
    }

    private void Explode()
    {
        exploded = true;

        if (explosion != null) Instantiate(explosion, transform.position, transform.rotation);

        // generate explosion field

        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);
        for (int i = 0; i < enemies.Length; i++) 
        {
            enemies[i].gameObject.GetComponent<EnemyScript>().Hurt();

            Rigidbody en_rb = enemies[i].GetComponent<Rigidbody>();
            if (en_rb != null) en_rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 0f, ForceMode.Impulse);
        }
    }

    private void Delay() {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    public void GenerateExplosion(Vector3 epicenter, float blastRadius)
    {
        Collider[] enemies = Physics.OverlapSphere(epicenter, blastRadius, enemyLayer);
        for (int i = 0; i < enemies.Length; i++) 
        {
            enemies[i].gameObject.GetComponent<EnemyScript>().Hurt();

            Rigidbody en_rb = enemies[i].GetComponent<Rigidbody>();
            if (en_rb != null) en_rb.AddExplosionForce(explosionForce, epicenter, blastRadius, 0f, ForceMode.Impulse);
        }
    }
}
