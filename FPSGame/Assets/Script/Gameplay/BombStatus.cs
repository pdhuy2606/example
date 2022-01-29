using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombStatus : MonoBehaviour
{
    public GameObject bombModel;
    public LayerMask isPlayer;
    public AudioSource destroySFX;
    public GameObject explosionEffect;

    bool isPlayerInEnterRange, isPlayerInDamageRange;
    public bool isAvtiveable;
    private float damage;
    public float enterRange = 10f;
    public float damageRange = 15f;
    public float force = 700f;

    

    // Start is called before the first frame update
    void Start()
    {
        isAvtiveable = true;
        //bulletRig = GetComponent<Rigidbody>();

    }
    
    // Update is called once per frame
    void Update()
    {
        if (bombModel != null && isAvtiveable) 
        {
            isPlayerInEnterRange = Physics.CheckSphere(bombModel.transform.position, enterRange, isPlayer);
            isPlayerInDamageRange = Physics.CheckSphere(bombModel.transform.position, damageRange, isPlayer);
            if (isPlayerInEnterRange)
            {
                OnExplosion();
            }
        }
    }

    public void OnExplosion()
    {
        if(bombModel != null && isAvtiveable)
        {
            if (isPlayerInDamageRange)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                Rigidbody rig = player.GetComponent<Rigidbody>();
                rig.AddExplosionForce(force, bombModel.transform.position, damageRange);
                player.GetComponent<PlayerCharacterController>().TakeDamage(20f);
            }
            DestroySelf();
        }
    }
    
    public void DestroySelf()
    {
        GameObject effect = Instantiate(explosionEffect, bombModel.transform.position, bombModel.transform.rotation);
        Destroy(effect, 1.5f);
        destroySFX.Play();
        Destroy(bombModel);
    }
    IEnumerator End()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision col)
    {
       /* if (col.gameObject.tag == "Player")
        {
            PlayerCharacterController player = GetComponent<PlayerCharacterController>();
            damage = Random.Range(5f, 10f);
            player.TakeDamage(damage);
            Destroy(bulletRig);
        }
        else if (col.gameObject.tag != null && col.gameObject.tag != "Player")
        {
            Destroy(bulletRig);
        }*/
    }
}
