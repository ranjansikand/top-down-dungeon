using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Bull : EnemyScript
{
    [Header("Ranges")]

    [SerializeField] float meleeRange;
    [SerializeField] float mediumRange;
    [SerializeField] float jumpExplosionRange;

    [Header("Light Attack")]
    
    [SerializeField] float lightAttackTime;
    
    [Header("Jump Attack")]

    [SerializeField] GameObject smashEffect;
    [SerializeField] float jumpAttackTime, explosionTiming;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Transform explosionPoint;

    [Header("Charge Attack")]

    [SerializeField] float chargeSpeed;

    [Header("Projectile Attack")]

    [SerializeField] GameObject spawnedProjectile;
    [SerializeField] int numberToSpawn;
    [SerializeField] Vector3 spawnPosition;
    [SerializeField] float xRange, yRange, zRange;
    [SerializeField] float projectileCooldown;

    [Header("Sound Effects")]

    [SerializeField] bool useSound;
    [SerializeField] AudioClip lightAttackSound1, lightAttackSound2, jumpAttackSound, chargeSound, bounceSound;
    [SerializeField] float lightVolume, jumpVolume, chargeVolume, bounceVolume;

    bool dead = false, isAttacking = false;
    float originalSpeed;

    Vector3 targetDestination = new Vector3(0, 44, 0);


    void Start()
    {
        originalSpeed = speed;
    }

    void Update()
    {
        if (!dead)
        {
            if (player == null) {
                AcquireTarget();
            } else if (!isAttacking) {
                Combat();
            }
            if (health <= 0) Die();
            if (isAttacking && targetDestination.y != 44) {
                if (Vector3.Distance(transform.position, targetDestination) < 2) {
                    EndAttack();
                }
            }
        }
    }

    void Die()
    {
        dead= true;
        StopChasing(false);
        anim.SetTrigger("Die");
        Invoke("Death", 4.5f);
    }

    void Combat()
    {
        StopChasing(true);

        if (IsPlayerBehind()) {
            // AOE or turn and face the player
            PursuePlayer(1.25f);
            return;
        }

        if (IsPlayerToSide()) {
            // turn toward player
            PursuePlayer(.5f);
            return;
        }

        if (Vector3.Distance(player.position, transform.position) < meleeRange)
        {
            // if player is within melee range
            LightAttack();
        }
        else if (Vector3.Distance(player.position, transform.position) < mediumRange)
        {
            JumpAttack();
        }
        else
        {
            if (Random.Range(1, 4) <= 2) ChargeAttack();
            else DropProjectiles();
        }
    }

    bool IsPlayerBehind()
    {
        Vector3 playerRelative  = transform.InverseTransformPoint(player.position);
        Debug.Log("Player's relative position: " + playerRelative);

        if (playerRelative.z < 0) {
            return true;
        }
        return false;
    }

    bool IsPlayerToSide()
    {
        Vector3 playerRelative  = transform.InverseTransformPoint(player.position);

        if (Mathf.Abs(playerRelative.x) > 1.5f) {
            return true;
        }
        return false;
    }

    void LightAttack()
    {
        isAttacking = true;

        // pick one or two
        if (Random.Range(1,3) == 1) { 
            anim.SetTrigger("Attack1");
            if (useSound) audiosource.PlayOneShot(lightAttackSound1, lightVolume);
        }
        else {
            anim.SetTrigger("Attack2");
            if (useSound) audiosource.PlayOneShot(lightAttackSound2, lightVolume);
        }


        Invoke(nameof(EndAttack), lightAttackTime);
    }

    void JumpAttack()
    {
        isAttacking = true;

        anim.SetTrigger("Attack3");
        if (useSound) audiosource.PlayOneShot(jumpAttackSound, jumpVolume);

        Invoke(nameof(Explosion), explosionTiming);
        Invoke(nameof(EndAttack), jumpAttackTime);
    }

    void Explosion()
    {
        Instantiate(smashEffect, explosionPoint.position, transform.rotation);

        Collider[] players = Physics.OverlapSphere(explosionPoint.position, jumpExplosionRange, playerLayer);
        for (int i = 0; i < players.Length; i++) 
        {
            players[i].gameObject.GetComponent<PlayerBase>().TakeDamage();
        }
    }

    void PursuePlayer(float duration)
    {
        isAttacking = true;

        anim.SetBool("Walk", true);
        ChasePlayer();

        Invoke(nameof(EndAttack), duration);
    }

    void ChargeAttack()
    {
        isAttacking = true;

        anim.SetBool("Run", true);
        if (useSound) audiosource.PlayOneShot(chargeSound, chargeVolume);
        speed = chargeSpeed;

        targetDestination = player.position;
        GoTo(targetDestination);
    }

    void DropProjectiles()
    {
        isAttacking = true;

        anim.SetTrigger("Jump");

        for (int i = 0; i < numberToSpawn; i++) {
            Vector3 randomPos = player.position + spawnPosition + new Vector3(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange), Random.Range(-zRange, zRange));
            Instantiate(spawnedProjectile, randomPos, Quaternion.identity);
        }

        Invoke(nameof(EndAttack), projectileCooldown);
    }

    void EndAttack()
    {
        if (anim.GetBool("Walk")) anim.SetBool("Walk", false);
        if (anim.GetBool("Run")) anim.SetBool("Run", false);

        StopChasing(true);

        speed = originalSpeed;
        targetDestination = new Vector3(0, 44, 0);

        isAttacking = false;
    }
}
