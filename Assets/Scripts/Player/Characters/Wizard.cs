using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : PlayerBase
{
    [Header("Heavy Attack")]

    [SerializeField] GameObject heavyBullet;
    [SerializeField] float heavyCooldown = 3;
    [SerializeField] int numberOfOrbs = 3;
    [SerializeField] GameObject castEffect;
    [SerializeField] AudioClip lightAttackClip, heavyAttackClip;
    [SerializeField] float audioVolume;

    bool canCast = true;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !dead && readyToShoot) 
        {
            readyToShoot = false;
            anim.SetBool("Walk", false);
            anim.SetTrigger("Attack");

            if (lightAttackClip != null) audiosource.PlayOneShot(lightAttackClip, audioVolume);
        }

        if (Input.GetMouseButtonDown(1) && !dead && canCast)
        {
            canCast = false;
            anim.SetBool("Walk", false);
            anim.SetTrigger("Heavy");

            if (heavyAttackClip != null) audiosource.PlayOneShot(heavyAttackClip, audioVolume);
        }
    }

    public void HeavyAttack()
    {
        if (castEffect != null) Instantiate(castEffect, transform.position + (2*Vector3.up), transform.rotation);

        StartCoroutine("SpawnOrbs");

        Invoke(nameof(ResetHeavy), heavyCooldown);
    }

    IEnumerator SpawnOrbs()
    {
        for (int i = 0; i < numberOfOrbs; i++)
        {
            Instantiate(heavyBullet, transform.position + (5 * Vector3.up) + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)), transform.rotation);

            yield return new WaitForSeconds(.15f);
        }
    }

    void ResetHeavy()
    {
        canCast = true;
        StopCoroutine("SpawnOrbs");
    }
}
