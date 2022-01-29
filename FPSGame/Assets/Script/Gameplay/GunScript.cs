using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GunScript : MonoBehaviour
{
    public Transform gunEnd;
    public Transform hitEnd;

    public AimingScript ac;
    public Camera fpsCam;
    public LineRenderer laserLine;
    RaycastHit hit;

    public GameObject bulletBar;
    public Slider slider;
    public TMP_Text textReload;
    public Animator animator;

    public GameObject explosionEffect;
    public AudioSource shootSFX, reloadSFX;

    public float damage;
    public float range = 45f;

    public float bullet;
    public float maxBullet = 10f;

    public Vector3 dir;


    public bool isAim;
    public bool isPlaying;
    
    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        textReload.text = "";
        bulletBar.SetActive(true);
        bullet = maxBullet;
        slider.value = CalculateBullet();
        laserLine.enabled = false;
        //ac.GetComponent<AimingScript>();
    }
    // Update is called once per frame
    void Update()
    {
        isPlaying = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacterController>().isPlaying;
        if (isPlaying)
        {
            BulletCheck();
            dir = fpsCam.transform.forward * fpsCam.transform.localEulerAngles.x/*fpsCam.transform.localRotation.x*/;
            isAim = ac.isAiming;

        }
    }
    private void Shoot()
    {
        laserLine.SetPosition(0, gunEnd.position);
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            /*laserLine.SetPosition(1, hit.point);*/
            Debug.Log("Player shoot into: " + hit.transform.name);
            //BossScript boss = hit.transform.GetComponent<BossScript>();
            EnemyController enemy = hit.transform.GetComponent<EnemyController>();
            CollectItemScript item = hit.transform.GetComponent<CollectItemScript>();
            BombStatus bomb = hit.transform.GetComponent<BombStatus>();
            if (enemy != null)
            {
                laserLine.SetPosition(1, hit.point);
                //damage = Random.Range(5f, 10f);
                damage = 10f;
                enemy.TakeDamage(damage);
            }
            else if (item != null)
            {
                laserLine.SetPosition(1, hit.point);
                item.OnActive();
            }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Boss"))
            {
                Debug.Log("Boss take damage");
                laserLine.SetPosition(1, hit.point);
                //damage = Random.Range(5f, 10f);
                damage = 15f;
                GameObject boss = GameObject.FindGameObjectWithTag("Boss");
                boss.GetComponent<BossScript>().TakeDamage(damage);
            }
            else if(bomb != null)
            {
                bomb.OnExplosion();
            }
            else if(hit.transform.tag != "Player" && hit.transform.tag !=null )
            {
                laserLine.SetPosition(1, hit.point);
            }
            else
            {
                laserLine.SetPosition(1, fpsCam.transform.position + (fpsCam.transform.forward * range));
            }
            GameObject effect = Instantiate(explosionEffect, laserLine.GetPosition(1), gunEnd.rotation);
            Destroy(effect, 1f);
        }
        else
        {
            laserLine.SetPosition(1, fpsCam.transform.position + (fpsCam.transform.forward * range));
        }
        
    }

    public void BulletCheck()
    {
        slider.value = CalculateBullet();
        if (bullet > 0)
        {
            textReload.text = bullet.ToString() + "/" + maxBullet.ToString();
            if (Input.GetButtonDown("Fire1"))
            {
                shootSFX.Play();
                StartCoroutine(OnShoot());
                Shoot();
                bullet--;
            }
            if(Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(OnReload());
            }
        }
        else if (bullet > maxBullet)
        {
            bullet = maxBullet;
        }
        else if (bullet == 0)
        {
            StartCoroutine(OnReload());
        }
    }
    IEnumerator OnReload()
    {
        reloadSFX.Play();
        textReload.text = "Reloading...";
        yield return new WaitForSeconds(1.5f);
        reloadSFX.Stop();
        bullet = maxBullet;
    }
    IEnumerator OnShoot()
    {
        //laserLine.enabled = true;
       
        if (isAim==false)
        {
            animator.SetBool("Shooted", true);
        }
        else
        {
            animator.SetBool("ShootedWAim", true);
        }
        yield return new WaitForSeconds(0.2f);
        laserLine.enabled = false;
        if(isAim==false)
        {
            animator.SetBool("Shooted", false);
        }
        else
        {
            animator.SetBool("ShootedWAim", false);
        }
    }

    float CalculateBullet()
    {
        return bullet / maxBullet;
    }
}
