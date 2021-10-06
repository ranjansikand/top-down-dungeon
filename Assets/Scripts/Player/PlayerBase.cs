using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [Header("Movement variables")]

    [SerializeField, Range(0f, 25f)] float speed;

    [SerializeField] GameObject healthUI;
    [SerializeField] Camera cam;


    [Header("Health")]

    [SerializeField] int maxHealth;
    [SerializeField] float invulnerableTime = 1f;

    int health = -10;
    public bool invulnerable;
    public bool dead = false;


    [Header("Inventory")]

    [SerializeField] List<GameObject> inventory = new List<GameObject>();
    [SerializeField] int maxInventorySize = 4;

    int key = -1;


    [Header("Animation")]

    public Animator anim;


    [Header("COMBAT SUBSECTION")]

    [Header("Shooting")]
    [SerializeField] GameObject bulletPrefab;
    public Transform firePoint;
    [SerializeField] AudioSource audiosource;
    [SerializeField] float shootForce, shotCooldown = 0.25f;
    [SerializeField] GameObject shootEffect;

    public bool readyToShoot = true;

 
    void Awake()
    {
        if (healthUI == null) healthUI = GameObject.Find("Health"); 
        if (cam == null) cam = GameObject.Find("Main Camera").GetComponent<Camera>();

        health = maxHealth;
        healthUI.GetComponent<Health>().SetUpHearts(maxHealth);

        Cursor.visible = true;
    }
    
    void LateUpdate()
    {   
        // movement
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput.Normalize();

        Vector3 velocity = new Vector3(playerInput.x, 0f, playerInput.y) * speed;
        Vector3 displacement = velocity * Time.deltaTime;

        if (playerInput != Vector2.zero) RotatePlayer(playerInput);
        transform.position += displacement;

        if (health <= 0 && !dead) Death(); 

        // animation
        if (anim != null)
        {
            if (playerInput != Vector2.zero) {
                if (anim.GetBool("Walk") == false) anim.SetBool("Walk", true);
            }
            else {
                anim.SetBool("Walk", false);
            }
        }
    }

    // movement function to rotate player more naturally
    void RotatePlayer(Vector2 input)
    {
        transform.forward = new Vector3(input.x, 0, input.y);
    }

    // various health functions
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 6 && !invulnerable) 
        {
            Debug.Log("Hurt");
            invulnerable = true;
            health--;
            healthUI.GetComponent<Health>().DecreaseHealth();
            Invoke("ResetAfterDamage", invulnerableTime); 
        }
    }

    void ResetAfterDamage() {invulnerable = false;}

    void Death()
    {
        dead = true;
        speed = 0;
        if (anim) anim.SetTrigger("Death");
        // GetComponent<PlayerShooting>().Death();
    }

    public void Heal()
    {
        if (health < maxHealth) health++;
        healthUI.GetComponent<Health>().IncreaseHealth();
    }

    // various inventory functions
    public bool AddToInventory(GameObject item)
    {
        if (inventory.Count < maxInventorySize)
        {
            inventory.Add(item);

            if (key == -1) key = 0;

            return true;
        }
        else return false;
    }

    public void ChangeActiveItem(int direction)
    {
        if (key != -1) 
        {
            // added code to make items loop
            if (direction > 0)
            {
                if (key < inventory.Count) key++;
                else key = 0;
            }
            else
            {
                if (key > 0) key--;
                else key = inventory.Count - 1;
            }
        }
    }

    public GameObject ActiveItem()
    {
        return inventory[key];
    }

    public void UseItem(int index)
    {
        inventory.RemoveAt(index);
    }

    // combat

    public void Shoot()
    {
        readyToShoot = false;

        if (shootEffect != null) Instantiate(shootEffect, firePoint.position, firePoint.rotation);
        
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(shootForce * firePoint.right, ForceMode.Impulse);

        Invoke(nameof(ResetShoot), shotCooldown);
    }

    void ResetShoot()
    {
        readyToShoot = true;
    }
}
