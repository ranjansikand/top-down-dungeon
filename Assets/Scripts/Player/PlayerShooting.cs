using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] AudioSource audiosource;

    [SerializeField] float shootForce, shotCooldown = 0.25f;

    [Header("Special Effects")]
    [SerializeField] Animator anim;
    [SerializeField] bool useSFX;
    [SerializeField] GameObject shootEffect;


    bool dead, readyToShoot = true;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !dead && readyToShoot) 
        {
            readyToShoot = false;
            anim.SetBool("Walk", false);
            anim.SetTrigger("Attack");
        }
    }

    public void Shoot()
    {
        if (useSFX) Instantiate(shootEffect, firePoint.position, firePoint.rotation);
        
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(shootForce * firePoint.right, ForceMode.Impulse);

        Invoke(nameof(ResetShoot), shotCooldown);
    }

    void ResetShoot()
    {
        readyToShoot = true;
    }

    public void Death() {dead = true;}
}
