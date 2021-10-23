using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mimic : EnemyScript
{
    bool deathSequenceBegan;

    void Update()
    {
        if (!deathSequenceBegan)
        {
            AcquireTarget();

            if (player != null)
            {
                anim.SetTrigger("Walk");
                ChasePlayer();
            }

            if (health <= 0)
            {
                deathSequenceBegan = true;
                StopChasing(false);
                anim.SetTrigger("Die");
                Invoke("Death", 1.5f);
            }
        }
    }
}
