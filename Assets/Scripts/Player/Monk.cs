using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monk : PlayerBase
{
    [Header("Punch Variables")]
    [SerializeField] Transform punchPoint;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float hitRadius, hitForce;
    [SerializeField] float cooldown;

    [Header("Dodge Roll")]
    [SerializeField] float immunityTime;
    // left off here

    bool canPunch = true;
    int attackType = 1;

    void Update()
    {
        if (canPunch && Input.GetKeyDown(KeyCode.RightShift))
        {
            canPunch = false;
            if (attackType % 2 == 1) {
                anim.SetTrigger("Punch1");
            }
            else {
                anim.SetTrigger("Punch2");
            }
            attackType++;
        }
    }

    public void Punch()
    {
        invulnerable = true;

        // generate AOE field

        Collider[] enemies = Physics.OverlapSphere(punchPoint.position, hitRadius, enemyLayer);
        for (int i = 0; i < enemies.Length; i++) 
        {
            enemies[i].gameObject.GetComponent<EnemyScript>().Hurt();

            Rigidbody en_rb = enemies[i].GetComponent<Rigidbody>();
            if (en_rb != null) en_rb.AddExplosionForce(hitForce, transform.position, hitRadius, 0f, ForceMode.Impulse);
        }

        Invoke(nameof(ResetPunch), cooldown);
    }

    void ResetPunch()
    {
        invulnerable = false;
        canPunch = true;
    }
}
