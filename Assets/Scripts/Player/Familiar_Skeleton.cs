using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Familiar_Skeleton : MonoBehaviour
{
    [Header("References")]

    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] AudioSource source;

    [Header("Stats")]

    [SerializeField] int health;
    [SerializeField] float speed, followingDistance, attackRadius, attackSpeed, sightRange;

    [Header("Animations")]

    [SerializeField] bool useAnimations;
    [SerializeField] string walkAnim, attackAnim, deathAnim;

    [Header("Sound")]

    [SerializeField] float volume;
    [SerializeField] AudioClip deathSound;

    const int layerMask = 1 << 6;
    Transform player, target;
    bool canAttack = true;

    void Start()
    {
        agent.speed = speed;
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target == null)
        {
            // check for enemies
            Detection();

            // follow player character
            if (Vector3.Distance(transform.position, player.position) > followingDistance) {
                // walk
                if (useAnimations) anim.SetBool(walkAnim, true);

                agent.SetDestination(player.position);
            }
            else {
                // the only time the object is not moving
                if (useAnimations) anim.SetBool(walkAnim, false);

                agent.SetDestination(transform.position);
            }
        }
        else 
        {
            if (Vector3.Distance(transform.position, target.position) > attackRadius) {
                if (useAnimations) anim.SetBool(walkAnim, true);

                agent.SetDestination(target.position);
            }
            else if (canAttack)
            {
                if (useAnimations)
                {
                    anim.SetBool(walkAnim, false);
                    anim.SetTrigger(attackAnim);

                    Attack();
                }
            }
        }

        if (health <= 0)
        {
            if (useAnimations) anim.SetTrigger(deathAnim);
            if (deathSound) source.PlayOneShot(deathSound, volume);

            Invoke(nameof(Die), 0.75f);
        }
        
    }

    void Detection()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, sightRange, layerMask);

        if (enemies.Length > 0) target = enemies[0].gameObject.transform;
        else target = null;
    }

    void Attack()
    {
        canAttack = false;

        if (Vector3.Distance(transform.position, target.position) < attackRadius)
        {
            target.gameObject.GetComponent<EnemyScript>().Hurt();
        }

        Invoke(nameof(ResetAttack), attackSpeed);
    }

    void ResetAttack()
    {
        canAttack = true;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 6)
        {
            health--;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
