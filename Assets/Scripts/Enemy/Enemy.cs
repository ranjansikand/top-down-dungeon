using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [SerializeField] NavMeshAgent agent;
    [SerializeField] int health;

    [Header("Animations")]
    [SerializeField] bool useAnimations;
    [SerializeField] Animator anim;
    [SerializeField] string deathTrigger, attackTrigger;

    [Header("Particle Effects")]
    [SerializeField] bool useParticleEffect;
    [SerializeField] GameObject particleEffect;

    [Header("Sound Effects")]
    [SerializeField] bool useSoundEffects;
    [SerializeField] AudioSource audiosource;
    [SerializeField] AudioClip hurtSound;
    [SerializeField] float hurtSoundVolume;
    [SerializeField] AudioClip deathSound;
    [SerializeField] float deathSoundVolume;

    Transform player;
    bool deathSequenceStarted = false;
    
    void Awake()
    {
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (!deathSequenceStarted) agent.SetDestination(player.position);
        
        if (health <= 0 && !deathSequenceStarted) 
        {
            deathSequenceStarted = true;
            if (useAnimations) 
            {
                agent.SetDestination(transform.position);
                anim.SetTrigger(deathTrigger);
                Invoke("Delay", 1.5f);
            }
            else Invoke("Delay", 0.05f);
        }
        
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet") 
        {
            health--;
        }
        
    }

    public void TakeDamage()
    {
        health--;
    }

    void Hit()
    {
        // any effects for being hurt go here
        if (useSoundEffects) audiosource.PlayOneShot(hurtSound, hurtSoundVolume);
    }

    public void Delay()
    {
        if (useParticleEffect) Instantiate(particleEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
