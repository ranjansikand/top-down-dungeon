using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pyromancer : PlayerBase
{
    [Header("Pyromancy - Light")]

    [SerializeField] GameObject fireBall;
    [SerializeField] int maxFireBalls;
    [SerializeField] float spread;
    [SerializeField] float fireForce;
    [SerializeField] float cooldown;


    [Header("Pyromancy - Heavy")]

    [SerializeField] GameObject effect;
    [SerializeField] GameObject glowEffectPrefab;
    [SerializeField] Transform body;
    [SerializeField] float castTime, effectDuration, effectCooldown, speedIncrease;

    [Header("Audio")]
    [SerializeField, Range(0f, 1f)] float audioVolume;
    [SerializeField] AudioClip lightAttack, special;

    bool readyToCast = true;
    float currentSpeed;
    GameObject glowEffect;


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && readyToShoot)
        {
            readyToShoot = false;
            anim.SetBool("Walk", false);
            anim.SetTrigger("Attack");
            audiosource.PlayOneShot(lightAttack, audioVolume);
        }

        if (Input.GetMouseButtonDown(1) && readyToCast) HeavyAttack();
    }

    public void ShootFire()
    {
        for (int i = 0; i < maxFireBalls; i++)
        {
            Vector3 randomSpread = new Vector3(Random.Range(-spread, spread), 0, Random.Range(-spread, spread));
            Vector3 directionWithSpread = firePoint.right + randomSpread;

            GameObject bullet = Instantiate(fireBall, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(fireForce * directionWithSpread.normalized, ForceMode.Impulse);
        }

        Invoke(nameof(ResetAttack), cooldown);
    }

    void ResetAttack()
    {
        readyToShoot = true;
    }

    void HeavyAttack()
    {
        movementEnabled = false;
        readyToCast = false;
        readyToShoot = false;

        currentSpeed = speed;

        Instantiate(effect, transform.position, new Quaternion(-0.707106829f,0,0,0.707106829f));
        if (special != null) audiosource.PlayOneShot(special, audioVolume);

        if (glowEffectPrefab != null) glowEffect = Instantiate(glowEffectPrefab, body.position, new Quaternion(1,0,0,0), gameObject.transform);

        speed += speedIncrease;


        Invoke(nameof(ResetHeavyStats), castTime);
        Invoke(nameof(EndEffect), effectDuration);
        Invoke(nameof(ResetHeavyCooldown), effectCooldown);
    }

    void ResetHeavyStats()
    {
        movementEnabled = true;
        readyToShoot = true;
    }

    void EndEffect()
    {
        speed = currentSpeed;
        if (glowEffect != null) Destroy(glowEffect);
    }

    void ResetHeavyCooldown()
    {
        readyToCast = true;
    }
}
