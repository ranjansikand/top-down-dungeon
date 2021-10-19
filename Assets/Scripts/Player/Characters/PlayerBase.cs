using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour
{
    [Header("Movement variables")]

    [Range(0f, 25f)] public float speed;

    [SerializeField] GameObject healthUI;
    [SerializeField] Camera cam;

    public bool movementEnabled = true;


    [Header("Health")]

    [SerializeField] int maxHealth;
    [SerializeField] float invulnerableTime = 1f;

    int health = -10;
    public bool invulnerable;
    public bool dead = false;


    [Header("Inventory")]

    [SerializeField] List<GameObject> inventory = new List<GameObject>();
    [SerializeField] List<Sprite> inventoryImages = new List<Sprite>();
    [SerializeField] int maxInventorySize = 4;
    [SerializeField] Image display;

    int key = -1;


    [Header("Animation")]

    public Animator anim;


    [Header("Shooting")]

    [SerializeField] GameObject bulletPrefab;
    public Transform firePoint;
    [SerializeField] float shootForce, shotCooldown = 0.25f;
    [SerializeField] GameObject shootEffect;

    public bool readyToShoot = true;


    [Header("Dodge Roll")]

    [SerializeField] Rigidbody rb;
    [SerializeField] bool canDodge;
    [SerializeField] float invulnerableDuration = 0.5f, dodgeCoolDown = 1, moveAmount = 3;

    bool readyToDodge = true;


    [Header("Sounds")]

    public AudioSource audiosource;
    [SerializeField, Range(0f, 1f)] float shootVolume;
    [SerializeField] AudioClip shootSound;

    [SerializeField, Range(0f, 1f)] float footstepVolume;
    [SerializeField] AudioClip[] footsteps;
    [SerializeField] float delayBetweenSteps;

    bool betweenSteps = true;

    
 
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
        if (movementEnabled) {
            Vector2 playerInput;

            playerInput.x = Input.GetAxis("Horizontal");
            playerInput.y = Input.GetAxis("Vertical");
            playerInput.Normalize();

            Vector3 velocity = new Vector3(playerInput.x, 0f, playerInput.y) * speed;
            Vector3 displacement = velocity * Time.deltaTime;

            if (playerInput != Vector2.zero) RotatePlayer(playerInput);
            transform.position += displacement;

            // animation
            if (anim != null)
            {
                if (playerInput != Vector2.zero) {
                    if (anim.GetBool("Walk") == false) anim.SetBool("Walk", true);

                    if (footsteps.Length != 0 && betweenSteps) FootstepSounds();
                }
                else {
                    anim.SetBool("Walk", false);
                }
            }
        }  
        
        if (health <= 0 && !dead) Death(); 

        // dodge roll
        if (canDodge && readyToDodge && Input.GetKeyDown(KeyCode.Space))
        {
            anim.ResetTrigger("Roll");
            DodgeRoll();
        }

        // inventory
        if (Input.GetKeyDown("1") && inventory.Count > 0) UseItem(key);
    }


    // movement function to rotate player more naturally
    void RotatePlayer(Vector2 input)
    {
        transform.forward = new Vector3(input.x, 0, input.y);
    }

    void DodgeRoll()
    {
        readyToDodge = false;
        invulnerable = true;

        anim.SetTrigger("Roll");
        rb.AddForce(transform.forward * moveAmount, ForceMode.Force);

        Invoke(nameof(ResetDamageAfterDodge), invulnerableDuration);
        Invoke(nameof(ResetDodge), dodgeCoolDown);
    }

    void ResetDamageAfterDodge()
    {
        invulnerable = false;
    }

    void ResetDodge()
    {
        readyToDodge = true;
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
    public bool AddToInventory(GameObject item, Sprite itemImage)
    {
        if (inventory.Count < maxInventorySize)
        {
            inventory.Add(item);
            inventoryImages.Add(itemImage);

            if (key == -1) ChangeActiveItem(0);

            return true;
        }
        else return false;
    }

    public void ChangeActiveItem(int direction)
    {
        if (key != -1) 
        {
            // added code to make items loop
            if (direction == 1)
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
        else {
            key = 0;
        }

        // update in UI
        display.sprite = inventoryImages[key];
    }

    public GameObject ActiveItem()
    {
        return inventory[key];
    }

    public void UseItem(int index)
    {
        if (key == -1) return;

        // spawn item
        GameObject item = Instantiate(inventory[index], transform.position + new Vector3(0, 3, 0), transform.rotation);
        inventory.RemoveAt(index);
        // change UI image
        inventoryImages.RemoveAt(index);
        if (inventoryImages.Count == 0) display.sprite = null;
        else ChangeActiveItem(1);
    }

    // combat

    public void Shoot()
    {
        readyToShoot = false;

        if (shootEffect != null) Instantiate(shootEffect, firePoint.position, firePoint.rotation);
        if (shootSound != null) audiosource.PlayOneShot(shootSound, shootVolume);
        
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(shootForce * firePoint.right, ForceMode.Impulse);

        Invoke(nameof(ResetShoot), shotCooldown);
    }

    void ResetShoot()
    {
        readyToShoot = true;
    }

    // sounds

    void FootstepSounds()
    {
        betweenSteps = false;
        audiosource.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)], footstepVolume);
        Invoke(nameof(ResetSteps), delayBetweenSteps);
    }

    void ResetSteps()
    {
        betweenSteps = true;
    }
}
