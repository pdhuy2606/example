using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingScript : MonoBehaviour
{
    public Animator animator;
    public GameObject targetAim;
    public GameObject gunCamera;
    public Camera mainCamera;

    public bool isAiming = false;
    public bool isPlaying_Aim;

    public float aimFOV = 40f;
    private float normalFOV;

    
    void Start()
    {
        isAiming = false;
        targetAim.SetActive(false);
    }
    void Update()
    {
        isPlaying_Aim =  GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacterController>().isPlaying;
        if (isPlaying_Aim)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                isAiming = !isAiming;
                animator.SetBool("Aimed", isAiming);

                if (isAiming)
                {
                    StartCoroutine(OnAim());
                }
                else
                {
                    OnUnAim();
                }
            }
            
        }
        else
        {
            if (targetAim.activeSelf) 
            {
                targetAim.SetActive(false);
                isAiming = !isAiming;
                animator.SetBool("Aimed", isAiming);
                OnUnAim();
            }
        }
    }

    IEnumerator OnAim()
    {
        yield return new WaitForSeconds(0.15f);
        targetAim.SetActive(true);
        gunCamera.SetActive(false);
        normalFOV = mainCamera.fieldOfView;
        mainCamera.fieldOfView = aimFOV;
    }
    void OnUnAim()
    {
        targetAim.SetActive(false);
        gunCamera.SetActive(true);
        mainCamera.fieldOfView = normalFOV;
    }
}
