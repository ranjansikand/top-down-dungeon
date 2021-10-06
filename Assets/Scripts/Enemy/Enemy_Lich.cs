using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Lich : EnemyScript
{
    [Header("Stats")]

    [SerializeField] float attackRange;
    [SerializeField] float lightCooldown, heavyCooldown;

    [Header("Attack Objects")]

    [SerializeField] Transform attackPoint;
    [SerializeField] GameObject lightProjectile, spawnedEnemy;
    [SerializeField] int spawnCount;
    
    [Header("Effects")]

    [SerializeField] GameObject lightAttackEffect;
    [SerializeField] GameObject heavyAttackEffect;

    bool cooldown, deathSequenceBegan;
    Transform playerSaver;


    void Update()
    {
        if (!deathSequenceBegan)
        {
            AcquireTarget();
            if (player != null) playerSaver = player;

            if (playerSaver == null) Patrol();
            else if (!cooldown) 
            {
                EnterCombatState();
            }

            if (health <= 0) Die();
        }
        
    }

    void LateUpdate()
    {
        transform.LookAt(player);
    }

    void EnterCombatState()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < attackRange)
        {
            cooldown = true;
            StopChasing();

            if (Random.Range(1, 4) == 2)
            {
                anim.SetTrigger("Attack2");
                Invoke(nameof(HeavyAttack), 1.5f);
            }
            else 
            {
                anim.SetTrigger("Attack1");
                Invoke(nameof(LightAttack), 0.75f);
            }
        }
        else ChasePlayer();
    }

    void LightAttack()
    {
        if (lightAttackEffect != null) Instantiate(lightAttackEffect, attackPoint.position, attackPoint.rotation);

        GameObject projectile = Instantiate(lightProjectile, attackPoint.position, attackPoint.rotation);
        projectile.GetComponent<EnemyProjectile>().Destination(playerSaver.position);

        Invoke(nameof(ResetCooldown), lightCooldown);
    }

    void HeavyAttack()
    {
        if (heavyAttackEffect != null) Instantiate(heavyAttackEffect, transform.position, Quaternion.identity);

        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(spawnedEnemy, transform.position + Vector3.right * Random.Range(-2, 3), transform.rotation);
        }
        

        Invoke(nameof(ResetCooldown), heavyCooldown);
    }

    void ResetCooldown()
    {
        player = playerSaver;
        cooldown = false;
        anim.SetTrigger("Walk");
    }

    void Die()
    {
        deathSequenceBegan = true;
        StopChasing();
        anim.SetTrigger("Die");
        Invoke("Death", 2.5f);
    }
}
