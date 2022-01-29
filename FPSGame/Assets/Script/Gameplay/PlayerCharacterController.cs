using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCharacterController : MonoBehaviour
{
    public CharacterController controller;
    public GameObject spawnPlace;
    public Transform groundCheck;
    public LayerMask groundMask;
    public AudioSource walkSFX, jumpSFX, healingSFX, dieSFX, collectSFX;

    public TMP_Text stageCrysText;
    public GameObject healthBar;
    public Slider slider;
    public TMP_Text textHealth;
    public GameObject bloodOverlay;
    public GameObject healingEffect;

    public float health;
    public float maxHealth = 100f;
    public float speed;
    public float speedWalk = 20f;
    public float speedRun = 60f;
    public float speedWAim= 10f;

    public float gravity = -15f;
    public float jumpHeight = 3f;
    public float groundDistance = 0.4f;
    Vector3 velocity;

    public int totalCrys;
    public int stageCrys;

    bool isGrounded;
    bool isRunning;
    bool isAim;

    public bool isAlive;
    public bool isPlaying;

    void Start()
    {
        stageCrys = 0;
        isPlaying = true;
        health = maxHealth;
        isAlive = true;
        transform.position = spawnPlace.transform.position;
        isRunning = false;
        bloodOverlay.SetActive(false);
        healingEffect.SetActive(false);
    }

    void Update()
    {
        if (isPlaying)
        {
            CrystalUpdate();
            if (isAlive)
            {
                PlayerMovement();
                SlideHealthUpdate();
            }
            else
            {
                bloodOverlay.SetActive(true);
            }
        }
    }

    public void PlayerMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
        
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpSFX.Play();
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            isRunning = true;
        }
        else isRunning = false;
        if (!isRunning && !isAim) 
        {
            speed = speedWalk;
        }
        if (isRunning && !isAim)
        {
            speed = speedRun;
        }
        if (!isRunning && isAim)
        {
            speed = speedWAim;
        }
        if (isRunning && isAim)
        {
            speed = 15f;
        }
        
    }
    public void SlideHealthUpdate()
    {
        slider.value = CalculateHeath();
        if (health < maxHealth)
        {
            healthBar.SetActive(true);
        }
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        if (health <= 0)
        {
            textHealth.text = "Death";
            dieSFX.Play();
            StartCoroutine(OnRespawn());
        } else
        {
            textHealth.text = health.ToString() + "/" + maxHealth.ToString();
        }
    }
    float CalculateHeath()
    {
        return health / maxHealth;
    }
    IEnumerator OnWalk()
    {
        walkSFX.Play();
        yield return new WaitForSeconds(1f);
    }
    IEnumerator OnHeal()
    {
        healingSFX.Play();
        healingEffect.SetActive(true);
        textHealth.text = "Healing...";
        yield return new WaitForSeconds(1.5f);
        health = maxHealth;
        healingEffect.SetActive(false);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        StartCoroutine(OnTakeDamage());
    }
    IEnumerator OnTakeDamage()
    {
        bloodOverlay.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        bloodOverlay.SetActive(false);
    }
    IEnumerator OnRespawn()
    {
        isAlive = false;
        bloodOverlay.SetActive(true);
        yield return new WaitForSeconds(4f);
        transform.position = spawnPlace.transform.position;
        bloodOverlay.SetActive(false);
        isAlive = true;
        health = maxHealth;
    }
    private void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.name + "Collision Enter");
        /*if (col.gameObject.tag == "PickupHealth")
        {
            Destroy(col.gameObject);
            StartCoroutine(OnHeal());
            //col.gameObject.GetComponent<>();
        }*/
        if (col.gameObject.tag == "DeathZone")
        {
            StartCoroutine(OnRespawn());
        }
        if (col.gameObject.tag == "Item")
        {
            //collectSFX.Play();
            col.gameObject.GetComponent<CollectItemScript>().OnActive();
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.gameObject.name + " Trigger Enter");
        if (col.gameObject.tag == "DeathZone")
        {
            StartCoroutine(OnRespawn());
        }
        if (col.gameObject.tag == "PickupHealth")
        {
            StartCoroutine(OnHeal());
            col.gameObject.GetComponent<PickupHealth>().ObjectActive();
            //col.gameObject.GetComponent<>();
        }
        if (col.gameObject.tag == "Crystal")
        {
            collectSFX.Play();
            //totalCrys++;
            stageCrys+=1;
            Destroy(col.gameObject);
            //col.gameObject.SetActive(false);
        }
    }
    public void CrystalUpdate()
    {
        stageCrysText.text = stageCrys.ToString();
    }
    public void LoadTotalCrystal()
    {
        if (SceneManager.GetActiveScene().name.ToString() != "SampleScene")
        {
            totalCrys = PlayerPrefs.GetInt("TotalCrystal");
        }
    }

    public void SaveTotalCrystal()
    {
        totalCrys = PlayerPrefs.GetInt("TotalCrystal");
        totalCrys += stageCrys;
        if (SceneManager.GetActiveScene().name.ToString() != "SampleScene")
        {
            PlayerPrefs.SetInt("TotalCrystal", totalCrys);
        }
    }
}
