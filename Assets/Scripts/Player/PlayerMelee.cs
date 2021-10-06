
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [Tooltip("What is being hit")]
    [SerializeField] LayerMask m_LayerMask;
    [Tooltip("Where is the attack launched from")]
    [SerializeField] Transform attackPoint;
    [Tooltip("What is the range of the attack")]
    [SerializeField] Vector3 hitBoxDimensions = new Vector3(1,1,1);
    [Tooltip("Time between attacks")]
    [SerializeField] float attackCooldown;

    bool readyToAttack = true;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && readyToAttack) Attack();
    }

    void Attack()
    {
        readyToAttack = false;

        Collider[] hitColliders = Physics.OverlapBox(attackPoint.position, hitBoxDimensions, Quaternion.identity, m_LayerMask);
        for (int i = 0; i < hitColliders.Length; i++)
        {
        hitColliders[i].gameObject.GetComponent<Enemy>().TakeDamage();
        }

        Invoke("ResetAttack", attackCooldown);
    }

    void ResetAttack()
    {
        readyToAttack = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, hitBoxDimensions*2);
    }
}
