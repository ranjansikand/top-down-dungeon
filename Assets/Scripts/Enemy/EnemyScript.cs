// public functions for use in individual enemy scripts

using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    [Header("Initialization variables")]

    [SerializeField] NavMeshAgent agent;
    [Range(0f, 25f)] public float speed;
    public int health;
    [SerializeField] bool takeBulletDamage = true;


    [Header("Detection")]

    [SerializeField] float maxSightRange;
    private Collider target;
    const int layerMask = 1 << 9;
    static Collider[] targetsBuffer = new Collider[100];
    public Transform player;

    [Header("Patrolling")]

    [SerializeField] float patrolRadius;
    [SerializeField] float waitTime;
    [SerializeField] LayerMask groundLayer;
    Vector3 patrolDestination;
    bool patrolDestinationSet;

    [Header("Attacking")]

    [SerializeField] bool attacking;
    [SerializeField] float attackDuration;
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed;
    [SerializeField] Transform fireFromPoint;


    [Header("Animations")]

    public Animator anim;
    [SerializeField] string hurtTrigger = "NA", searchTrigger = "NA", detectedTrigger = "NA", attackTrigger = "NA";

 
    [Header("Sound Effects")]

    public AudioSource audiosource;
    [SerializeField] AudioClip detectionSound, hurtSound, deathSound, attackSound;
    [SerializeField] float detectionSoundVolume, hurtSoundVolume,deathSoundVolume, attackSoundVolume;

 
    [Header("Other effects")]

    [SerializeField] GameObject detectedEffect;
    [SerializeField] GameObject attackingEffect;
    [SerializeField] GameObject hurtEffect;
    [SerializeField] GameObject deathEffect;


    void Awake()
    {
        agent.speed = speed;
    }

    // BEGIN ENEMY FUNCTIONS

    public bool AcquireTarget () {
        Vector3 a = transform.localPosition;
        Vector3 b = a;
        b.y += 2f;
        int hits = Physics.OverlapCapsuleNonAlloc(
            a, b, maxSightRange, targetsBuffer, layerMask
        );
        if (hits > 0) {
            for (int i = 0; i < hits; i++)
            {
                if (targetsBuffer[i].gameObject.tag == "Player") {
                    target = targetsBuffer[i].GetComponent<Collider>();
                    player = target.gameObject.transform;

                    if (detectedEffect != null) Instantiate(detectedEffect, transform.position, transform.rotation);
                    if (detectionSound != null) audiosource.PlayOneShot(detectionSound, detectionSoundVolume);
                    if (detectedTrigger != "NA") anim.SetTrigger(detectedTrigger);
                }
            }
            // Debug.Assert(target != null, "Targeted non-enemy!", targetsBuffer[0]);
            return true;
        }
        target = null;
        return false;
    }

    public void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    public void StopChasing(bool save)
    {
        if (!save) player = null;
        agent.SetDestination(transform.position);
        
    }

    public void GoTo(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    void SetPatrolDestination()
    {
        if (player != null) {return;}

        patrolDestination = new Vector3(
                transform.position.x + Random.Range(-patrolRadius, patrolRadius), 
                transform.position.y, 
                transform.position.z + Random.Range(-patrolRadius, patrolRadius) 
            );

        if (Physics.Raycast(patrolDestination, -transform.up, 2f, groundLayer)) patrolDestinationSet = true;
    }

    public void Patrol()
    {
        if (!patrolDestinationSet) SetPatrolDestination();
    
        agent.SetDestination(patrolDestination);

        Vector3 distanceToDestination = transform.position - patrolDestination;
        if (distanceToDestination.magnitude < 1f) 
        {
            if (searchTrigger != "NA") anim.SetTrigger(searchTrigger);
            Invoke("ResetPatrol", waitTime);
        }
        
    }

    void ResetPatrol()
    {
        if (player == null)
        {
            patrolDestinationSet = false;
        } 
    }

    public void Attacking()
    {  
        if (attacking) {return;}

        // hold still while attacking
        agent.SetDestination(transform.position);
        attacking = true;
        
        if (attackSound != null) audiosource.PlayOneShot(attackSound, attackSoundVolume);
        if (attackingEffect != null) Instantiate(attackingEffect, transform.position, transform.rotation);
        if (attackTrigger != "NA") anim.SetTrigger(attackTrigger);

        if (projectile != null)
        {
            // if it reaches here, this is a ranged enemy
            transform.LookAt(player.position);

            GameObject bullet = Instantiate(projectile, fireFromPoint.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
        }

        Invoke(nameof(ResetAttack), attackDuration);
    }

    void ResetAttack()
    {
        attacking = false;
        agent.SetDestination(player.position);

    }

    public void Hurt()
    {
        health--;

        if (health > 0)
        {
            if (hurtSound != null) audiosource.PlayOneShot(hurtSound, hurtSoundVolume);
            if (hurtEffect != null) Instantiate(hurtEffect, transform.position, transform.rotation);
            if (hurtTrigger != "NA") anim.SetTrigger(hurtTrigger);

        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (takeBulletDamage && other.gameObject.tag == "Bullet") Hurt();
    }

    public void Death()
    {
        if (deathEffect != null) Instantiate(deathEffect, transform.position, Quaternion.identity);
        if (deathSound != null) audiosource.PlayOneShot(deathSound, deathSoundVolume);

        Destroy(gameObject);
    }
}
