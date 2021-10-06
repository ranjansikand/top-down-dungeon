using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : PlayerBase
{
    [Header("Heavy Attack")]

    [SerializeField] GameObject heavyBullet;
    [SerializeField] float heavyCooldown = 3, heavyForce;
    [SerializeField] int numberOfOrbs = 3;
    [SerializeField] GameObject castEffect;

    bool canCast = true;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !dead && readyToShoot) 
        {
            readyToShoot = false;
            anim.SetBool("Walk", false);
            anim.SetTrigger("Attack");
        }

        if (Input.GetMouseButtonDown(1) && !dead && canCast)
        {
            canCast = false;
            anim.SetBool("Walk", false);
            anim.SetTrigger("Heavy");
        }
    }

    public void HeavyAttack()
    {
        if (castEffect != null) Instantiate(castEffect, transform.position, transform.rotation);

        for (int i = 0; i < numberOfOrbs; i++)
        {
            Vector3 posAdjust = new Vector3(i - 1, 0, 0);

            GameObject bullet = Instantiate(heavyBullet, firePoint.position + posAdjust, firePoint.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(heavyForce * firePoint.right, ForceMode.Impulse);
        }

        Invoke(nameof(ResetHeavy), heavyCooldown);
    }

    void ResetHeavy()
    {
        canCast = true;
    }
}
