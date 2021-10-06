using UnityEngine;
using UnityEngine.AI;

public class Slime : MonoBehaviour
{

    [SerializeField] NavMeshAgent agent;
    [SerializeField] bool rotate;
    [SerializeField] int health;

    [SerializeField] int slimesize;
    [SerializeField] GameObject slime;
    [SerializeField] Animator anim;

    [SerializeField] bool usePopEffect;
    [SerializeField] GameObject slimePop;

    Transform player;
    bool called = false;
    
    void Awake()
    {
        player = GameObject.Find("Player").transform;
        transform.localScale = new Vector3(1.5f,1.5f,1.5f) * slimesize;
        health = slimesize;
    }

    public void SetSize(int size)
    {
        slimesize = size;
        health = size;
        transform.localScale = new Vector3(1.5f,1.5f,1.5f) * size;
    }

    void Update()
    {
        if (!called && Vector3.Distance(transform.position, player.position) < 12) agent.SetDestination(player.position);

        if (rotate)
        {
            Quaternion rotation = player.position.x < transform.position.x ? new Quaternion(0,-0.707106829f,0.707106829f,0) : new Quaternion(0.707106829f,0,0,0.707106829f);
            transform.rotation = rotation;
        }
        
        if (health <= 0 && !called) 
        {
            called = true;
            if (slimesize >= 3)
            {
                anim.SetTrigger("Die");
                agent.SetDestination(transform.position);
                Invoke("Delay", 1.25f);
            }
            else Invoke("Delay", .25f);
            
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet") health-=1;
    }

    public void TakeDamage()
    {
        health--;
    }

    public void Delay()
    {
        if (usePopEffect) {
            GameObject popEffect = Instantiate(slimePop, transform.position, transform.rotation);
            popEffect.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f) * slimesize;
        }
        
        if (slimesize == 1) Destroy(gameObject);
        else 
        {
            Vector3 spawnPos = transform.position;
            
            GameObject slime1 = Instantiate(slime, transform.position, transform.rotation);
            GameObject slime2 = Instantiate(slime, transform.position, transform.rotation);

            slime1.GetComponent<Slime>().SetSize(slimesize - 1);
            slime2.GetComponent<Slime>().SetSize(slimesize - 1);

            Destroy(gameObject);
        }
    }
}