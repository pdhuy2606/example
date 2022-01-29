using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pickupHealthObj;
    void Start()
    {
        pickupHealthObj.SetActive(true);
    }

    public void ObjectActive()
    {
        StartCoroutine(OnRespawn());
    }
    IEnumerator OnRespawn()
    {
        pickupHealthObj.SetActive(false);
        yield return new WaitForSeconds(5f);
        pickupHealthObj.SetActive(true);
    }
}
