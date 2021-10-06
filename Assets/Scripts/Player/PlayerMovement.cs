using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement variables")]

    [SerializeField, Range(0f, 100f)] float speed;

    [SerializeField] GameObject healthUI;
    [SerializeField] Camera cam;
    [SerializeField] int maxHealth;
    int health = -10;
    bool invulnerable;
    [SerializeField] float invulnerableTime = 1f;


    [Header("Inventory")]

    [SerializeField] List<GameObject> inventory = new List<GameObject>();
    [SerializeField] int maxInventorySize = 4;

    [Header("Animation")]
    [SerializeField] Animator anim;

    int key = -1;
    bool dead = false;

    void Awake()
    {
        if (healthUI == null) healthUI = GameObject.Find("Health"); 
        if (cam == null) cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        if (health == -10) 
        {
            health = maxHealth;
            healthUI.GetComponent<Health>().SetUpHearts(maxHealth);
        }

        Cursor.visible = true;
    }
    
    void Update()
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

    void RotatePlayer(Vector2 input)
    {
        transform.forward = new Vector3(input.x, 0, input.y);
    }

    // health
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

    // inventory functions

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

    void ChangeActiveItem(int direction)
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

    GameObject ActiveItem()
    {
        return inventory[key];
    }

    void UseItem(int index)
    {
        inventory.RemoveAt(index);
    }
}
