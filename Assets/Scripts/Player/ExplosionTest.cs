using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTest : EnemyScript
{
    int maxHealth;
    public MeshRenderer mesh;

    void Start()
    {
        maxHealth = health;
    }
    void Update()
    {
        if (health < maxHealth)
        {
            mesh.enabled = false;
            Invoke("ReEnable", 3f);
        }
    }

    void ReEnable()
    {
        maxHealth = health;
        mesh.enabled = true;
    }
}
