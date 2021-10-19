using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleric : PlayerBase
{
    [Header("Heavy Spell")]
    [SerializeField] int numberToShoot;
    [SerializeField] GameObject castEffect, projectile;
    [SerializeField] float castTime, rechargeTime, heavyForce;
    [SerializeField, Range(0f, 1f)] float clipvolume;
    [SerializeField] AudioClip heavySound;

    [SerializeField] float blastRadius, explosionForce;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] GameObject burst;


    bool readyToCast = true;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !dead && readyToShoot) 
        {
            readyToShoot = false;
            anim.SetBool("Walk", false);
            anim.SetTrigger("Attack");
        }

        if (Input.GetMouseButtonDown(1) && !dead && readyToCast)
        {
            Instantiate(castEffect, transform.position+new Vector3(0, 0.35f, 0), new Quaternion(0.707106829f,0,0,0.707106829f));
            readyToCast = false;
            anim.SetBool("Walk", false);
            anim.SetTrigger("Heavy");
        }
    }

    public void HeavySpell()
    {
        movementEnabled = false;

        if (burst != null) Instantiate(burst, transform.position+new Vector3(0, 0.35f, 0), new Quaternion(0.707106829f,0,0,0.707106829f));
        if (heavySound != null) audiosource.PlayOneShot(heavySound, clipvolume); 

        Collider[] enemies = Physics.OverlapSphere(transform.position, blastRadius, enemyLayer);
        for (int i = 0; i < enemies.Length; i++) 
        {
            enemies[i].gameObject.GetComponent<EnemyScript>().Hurt();

            Rigidbody en_rb = enemies[i].GetComponent<Rigidbody>();
            if (en_rb != null) en_rb.AddExplosionForce(explosionForce, transform.position, blastRadius, 0f, ForceMode.Impulse);
        }

        Invoke(nameof(ResetMovement), castTime);
        Invoke(nameof(ResetSpell), rechargeTime);
    }

    void ResetMovement()
    {
        
        movementEnabled = true;
    }

    void ResetSpell()
    {
        readyToCast = true;
    }
}
