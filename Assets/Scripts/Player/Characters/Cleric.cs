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
            readyToCast = false;
            anim.SetBool("Walk", false);
            anim.SetTrigger("Heavy");
        }
    }

    public void HeavySpell()
    {
        movementEnabled = false;

        Instantiate(castEffect, transform.position+new Vector3(0, 0.35f, 0), new Quaternion(0.707106829f,0,0,0.707106829f));

        if (heavySound != null) audiosource.PlayOneShot(heavySound, clipvolume); 

        for (int i = 0; i < numberToShoot; i++)
        {
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f));

            GameObject bullet = Instantiate(projectile, transform.position + Vector3.up, transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(heavyForce * randomDirection, ForceMode.Impulse);
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
