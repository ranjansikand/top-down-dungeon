using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Golem : EnemyScript
{
    [Header("Combat Manager")]

    [SerializeField] GameObject rock;
    [SerializeField] float throwRange, punchRange;
    [SerializeField] float throwDelay, punchDelay;

    [Header("Throwing")]

    [SerializeField] Transform rightHand;
    [SerializeField] float throwForce;

    GameObject currentRock;

    [Header("Sounds")]

    [SerializeField] AudioSource additionalSource;
    [SerializeField] float volume;
    [SerializeField] AudioClip roar;
    [SerializeField] AudioClip[] grunts;

    bool dead = false, alreadyDetected = false, isAttacking = false;

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            if (health <= 0) Die();

            if (player == null) AcquireTarget();
            else {
                if (!isAttacking) {
                    Combat();
                }
                if (!isAttacking)
                {
                    anim.SetBool("Walk", true);
                    ChasePlayer();
                }

                if (Vector3.Distance(player.position, transform.position) > throwRange) transform.LookAt(player.position);
            }

            
        }
    }

    void Die()
    {
        dead= true;
        StopChasing(false);
        anim.SetTrigger("Die");
        Invoke("Death", 1.5f);
    }

    void Combat()
    {
        isAttacking = true;

        if (!alreadyDetected) 
        {
            anim.SetTrigger("Detected");
            audiosource.PlayOneShot(roar, volume);
            Invoke(nameof(Detection), 2.5f);
            return;
        }

        float dist = Vector3.Distance(player.position, transform.position);

        if (dist > throwRange)
        {
            // throw rocks
            ThrowRocks();
            return;
        }
        else if (dist < punchRange)
        {
            // throw hands
            Punch();
            return;
        }

        isAttacking = false;
    }

    void Detection()
    {
        alreadyDetected = true;
        isAttacking = false;
    }

    void ThrowRocks()
    {
        StopChasing(true);
        anim.SetBool("Walk", false);

        currentRock = Instantiate(rock, rightHand.position, rightHand.rotation);
        currentRock.GetComponent<Rock>().SetPin(rightHand);

        anim.SetTrigger("Throw");

        Invoke(nameof(EndAttack), throwDelay);
    }

    void ReleaseRock()
    {
        additionalSource.PlayOneShot(grunts[Random.Range(0, grunts.Length)], volume);
        currentRock.GetComponent<Rock>().RemovePin();
        currentRock.GetComponent<Rigidbody>().AddForce(throwForce * transform.forward, ForceMode.Impulse);
    }

    void Punch()
    {
        StopChasing(true);
        anim.SetBool("Walk", false);

        anim.SetTrigger("Punch");

        Invoke(nameof(EndAttack), punchDelay);
    }

    void EndAttack()
    {
        isAttacking = false;
    }
}
