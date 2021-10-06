using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleric : PlayerBase
{
    [Header("Heavy Spell")]

    [SerializeField] GameObject spell;
    [SerializeField] float rechargeTime, distanceAdjustment;
    [SerializeField] Vector3 spellSpawnLocation;

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
        Vector3 spawn = firePoint.position + spellSpawnLocation + (firePoint.right * distanceAdjustment);

        Instantiate(spell, spawn, new Quaternion(-0.707106829f,0,0,0.707106829f));

        Invoke(nameof(ResetSpell), rechargeTime);
    }

    void ResetSpell()
    {
        readyToCast = true;
    }
}
