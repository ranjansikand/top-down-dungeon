using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Standard : EnemyScript
{
    bool deathSequenceBegan;

    void Update()
    {
        if (!deathSequenceBegan)
        {
            AcquireTarget();

            if (player == null) Patrol();
            else ChasePlayer();

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
