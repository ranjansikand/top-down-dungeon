using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Metalon : EnemyScript
{
    [SerializeField] float attackRange, attackDelay, chargeRange, chargeSpeed;

    bool dead, isAttacking;

    void Start()
    {
        anim.SetBool("Walk", true);
    }

    void Update()
    {
        if (!dead) {
            if (player == null) 
            {
                AcquireTarget();
                Patrol();
            }
            else {
                if (!isAttacking) 
                {
                    ChasePlayer();
                    anim.SetBool("Walk", true);
                }

                if (Vector3.Distance(player.position, transform.position) > chargeRange && !isAttacking) PrepCharge();

                if (Vector3.Distance(player.position, transform.position) < attackRange && !isAttacking)
                {
                    isAttacking = true;
                    StopChasing(true);
                    anim.SetTrigger("Stab");
                    Invoke(nameof(StopAttack), attackDelay);
                }
            }

            if (health <= 0) Die();
        }
    }

    void PrepCharge()
    {
        StopChasing(true);
        isAttacking = true;

        anim.SetBool("Walk", false);
        anim.SetTrigger("Cast Spell");

        Invoke(nameof(ChargeAttack), 1);
    }

    void ChargeAttack()
    {
        float dist = Vector3.Distance(player.position, transform.position);

        agent.speed = chargeSpeed;
        GoTo(player.position);

        anim.SetBool("Run", true);

        Invoke(nameof(StopAttack), 2 * dist/chargeSpeed);
    }

    void StopAttack()
    {
        isAttacking = false;
        agent.speed = speed;

        if (anim.GetBool("Run")) anim.SetBool("Run", false);
    }

    void Die()
    {
        dead = true;
        StopChasing(false);
        anim.SetTrigger("Die");
        Invoke("Death", 1.5f);
    }
}
