using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{
    public Transform player;
    public Transform groundPlayer;
    public Transform head;
    public Transform body;
    public GameObject bomb;
    public GameObject explosionEffect;
    public GameObject effectBomb;
    //public NavMeshAgent agent;

    public LayerMask isPlayer;

    public float health;
    public float maxHealth = 200f;
    public float damage;
    public float bullet;
    public float maxBullet = 10f;

    public GameObject bulletBar;
    public Slider sliderBullet;
    public GameObject healthBar;
    public Slider sliderHealth;

    public float sightRange = 80f;
    public float attackRange = 60f;

    public bool isPlayerInSightRange;
    public bool isPlayerInAttackRange;

    public float timeBetweenShooting = 2f;
    public float gravity = 10f;

    private bool alreadyShoot;
    public bool isPlaying;

    public AudioSource alertSFX, attackSFX, reloadSFX, destroySFX;
    
    // Start is called before the first frame update
    void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
        //transform.position = respawnPlace.position;
        health = maxHealth;
        sliderHealth.value = CalculateHeath();
        bulletBar.SetActive(true);
        bullet = maxBullet;
        sliderBullet.value = CalculateBullet();
        isPlaying = true;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        groundPlayer = player.GetChild(1);
        head = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        isPlaying = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacterController>().isPlaying;
        if (isPlaying)
        {
            SlideHealthUpdate();

            /*if(head != null)
            {

            }*/
            isPlayerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer);
            isPlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

            if (isPlayerInSightRange)
            {
                if (!isPlayerInAttackRange)
                {
                    Chasing();
                }
                else
                {
                    Attack();
                }
            }
        }
    }

    public void Chasing()
    {
        transform.LookAt(player);
        alertSFX.Play();
    }
    public void Attack()
    {
        sliderBullet.value = CalculateBullet();
        
        if (bullet > maxBullet)
        {
            bullet = maxBullet;
        }
        if (bullet == 0)
        {
            StartCoroutine(OnReload());
        }

        transform.LookAt(player);

        if (alreadyShoot == false)
        {
            StartCoroutine(OnBossShooting());
            /*Rigidbody rb = Instantiate(bomb, gunEnd.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);*/

            bullet--;
            alreadyShoot = true;
            Invoke(nameof(ResetShoot), 2f);            
        }
    }

    IEnumerator OnBossShooting()
    {
        Rigidbody rb = Instantiate(bomb, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        /*rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
        rb.AddForce(transform.up * 8f, ForceMode.Impulse);*/
        attackSFX.Play();
        StartCoroutine(SimulateProjectile(rb));
        yield return new WaitForSeconds(5f);
        rb.GetComponent<BombStatus>().OnExplosion();
    }
    private void ResetShoot()
    {
        alreadyShoot = false;
    }

    public void Die()
    {
        GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(effect, 1.5f);
        Destroy(head.gameObject);
        Destroy(healthBar);
        Destroy(bulletBar);
        StartCoroutine(Wait());
        GameObject[] grenade02List = GameObject.FindGameObjectsWithTag("Grenade02");
        for (int i = 0; i < grenade02List.Length; i++)
        {
            grenade02List[i].GetComponent<BombStatus>().OnExplosion();
        }
        /*Destroy(healthBar);
        Destroy(bulletBar);*/
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        Destroy(body);
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
    }
    public void SlideHealthUpdate()
    {
        sliderHealth.value = CalculateHeath();
        if (health < maxHealth && health >0)
        {
            healthBar.SetActive(true);
        }
        else if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if (health <= 0)
        {
            destroySFX.Play();
            Die();
        }
    }
    float CalculateHeath()
    {
        return health / maxHealth;
    }
    float CalculateBullet()
    {
        return bullet / maxBullet;
    }
    IEnumerator OnHeal()
    {
        yield return new WaitForSeconds(3f);
        health++;
    }
    IEnumerator OnReload()
    {
        reloadSFX.Play();
        yield return new WaitForSeconds(4f);
        bullet = maxBullet;
    }
    IEnumerator SimulateProjectile(Rigidbody rb)
    {
        // Short delay added before Projectile is thrown
        //yield return new WaitForSeconds(1.5f);

        // Move projectile to the position of throwing object + add some offset if needed.
        rb.position = transform.position + new Vector3(0, 0.0f, 0);

        // Calculate distance to target
        float target_Distance = Vector3.Distance(transform.position, /*player.position*/groundPlayer.position);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * 45.0f * Mathf.Deg2Rad) / gravity);

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(45.0f * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(45.0f * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;

        // Rotate projectile to face the target.
        rb.rotation = Quaternion.LookRotation(/*player.position*/groundPlayer.position - rb.position);

        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            rb.transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }
    }
}
