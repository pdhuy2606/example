using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItemScript : MonoBehaviour
{
    public GameObject UnActiveItem;
    public GameObject ActiveItem;
    public AudioSource activeSFX;
    public bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        UnActiveItem.SetActive(true);
        ActiveItem.SetActive(false);
        isActive = false;
    }

    public void OnActive()
    {
        if (!isActive)
        {
            activeSFX.Play();
            UnActiveItem.SetActive(false);
            //Destroy(UnActiveItem);
            ActiveItem.SetActive(true);
            isActive = true;
        }
    }
}
