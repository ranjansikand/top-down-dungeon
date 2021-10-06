using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Slime : EnemyScript
{
    [Header("Splitting")]
    [SerializeField] GameObject slime;
    [SerializeField] int numberToSpawn;
    [SerializeField] int slimeSize;
    [SerializeField] GameObject popEffect;

    bool dying;
    float delay = 0.25f;

    void Start()
    {
        SlimePrep(slimeSize);
    }

    public void SlimePrep(int size)
    {
        slimeSize = size;
        health = size;
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f) * size;
    }

    void Update()
    {
        if (!dying)
        {
            AcquireTarget();

            if (player == null) Patrol();
            else ChasePlayer();

            if (health <= 0 && !dying) 
            {
                dying = true;
                anim.SetTrigger("Die");

                if (slimeSize >= 3) delay = 1.25f;

                Invoke("Divide", delay);
            }
        }
    }

    void Divide()
    {
        if (popEffect) Instantiate(popEffect, transform.position, transform.rotation);

        if (slimeSize > 1)
        {
            for (int i = 0; i < numberToSpawn; i++)
            {
                GameObject babySlime = Instantiate(slime, transform.position + new Vector3(0, Random.Range(-3, 3), 0), transform.rotation);
                babySlime.GetComponent<Enemy_Slime>().SlimePrep(slimeSize - 1);
            }
        }
        Destroy(gameObject);
    }
}
