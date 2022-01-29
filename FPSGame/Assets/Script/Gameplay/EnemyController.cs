using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public Transform gunEnd;
    public Rigidbody rig;
    public Camera enemyCam;
    public LineRenderer laserLine;
    public Transform respawnPlace;

    public GameObject explosionEffect;
    public AudioSource attackSFX, alertSFX, moveSFX, reloadSFX, destroySFX;

    public GameObject healthBar;
    public Slider sliderHealth;
    public GameObject bulletBar;
    public Slider sliderBullet;

    public LayerMask isGround, isPlayer;

    public float speed = 10f;
    public float health;
    public float maxHealth = 50f;
    public float damage;
    public float bullet;
    public float maxBullet = 5f;
    public float timeBetweenShooting = 2f;

    public float sightRange = 30f;
    public float attackRange = 20f;

    public bool isPlayerInSightRange;
    public bool isPlayerInAttackRange;

    bool alreadyShoot;
    public bool isPlaying;

    //public GameObject shootEffect;
    void Start()
    {
        transform.position = respawnPlace.position;
        health = maxHealth;
        sliderHealth.value = CalculateHeath();
        bulletBar.SetActive(true);
        bullet = maxBullet;
        sliderBullet.value = CalculateBullet();
        isPlaying = true;
    }
    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (isPlaying)
        {
            SlideHealthUpdate();

            isPlayerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer);
            isPlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

            if (!isPlayerInSightRange)
            {
                alertSFX.Pause();
                moveSFX.Pause();
                Patroling();
            }
            else
            {
                alertSFX.Play();
                if (!isPlayerInAttackRange)
                {
                    moveSFX.Play();
                    Chasing();
                }
                else
                {
                    Attack();
                }
            }
        }
        else
        {
            agent.SetDestination(transform.position);
        }
    }
    
    public void Patroling()
    {
        transform.LookAt(respawnPlace);
        agent.SetDestination(respawnPlace.position);
        if (health < maxHealth)
        {
            StartCoroutine(OnHeal());
        }
    }
    public void Chasing()
    {
        agent.SetDestination(player.position);
        transform.LookAt(player);
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

        agent.SetDestination(player.position);
        transform.LookAt(player);

        if (alreadyShoot == false)
        {
            StartCoroutine(EnemyShooting());
            bullet--;
            alreadyShoot = true;
            Invoke(nameof(ResetShoot), 2f);
        }
    }
   
    private void ResetShoot()
    {
        alreadyShoot = false;
    }

    public void Die()
    {
        GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(effect, 1.5f);
        destroySFX.Play();
        Destroy(gameObject);
        Destroy(healthBar);
        Destroy(bulletBar);
    }
    public void TakeDamage(float amount)
    {
        rig.AddExplosionForce(500f, rig.transform.position, 10f);
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }
    public void SlideHealthUpdate()
    {
        sliderHealth.value = CalculateHeath();
        if (health < maxHealth)
        {
            healthBar.SetActive(true);
        }
        if (health > maxHealth)
        {
            health = maxHealth;
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
    IEnumerator EnemyShooting()
    {
        attackSFX.Play();
        StartCoroutine(visibleLaser());
        yield return new WaitForSeconds(2f);
        
        laserLine.SetPosition(0, gunEnd.position);
        RaycastHit hit;
        if (Physics.Raycast(enemyCam.transform.position, enemyCam.transform.forward, out hit, attackRange, isPlayer))
        {
           
            //Debug.Log("Enemy shoot into: "+ hit.transform.name);
            PlayerCharacterController target = hit.transform.GetComponent<PlayerCharacterController>();
            if (target != null)
            {
                laserLine.SetPosition(1, hit.point);
                damage = 10f;
                target.TakeDamage(damage);
            }
            else
            {
                laserLine.SetPosition(1, enemyCam.transform.position + (enemyCam.transform.forward * attackRange));
            }
        }
        else
        {
            laserLine.SetPosition(1, enemyCam.transform.position + (enemyCam.transform.forward * attackRange));
        }  
    }

    IEnumerator visibleLaser()
    {
        laserLine.enabled = true;
        yield return new WaitForSeconds(0.5f);
        laserLine.enabled = false;
    }
}

