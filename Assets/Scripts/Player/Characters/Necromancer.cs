using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromancer : PlayerBase
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
        // spawn a familiar to attack enemies

        Invoke(nameof(ResetSpell), rechargeTime);
    }

    void ResetSpell()
    {
        readyToCast = true;
    }
}
