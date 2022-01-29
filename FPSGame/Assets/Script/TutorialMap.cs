using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMap : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject Boss;
    public GameObject PickupHealth;
    public GameObject Item;
    public GameObject Grenade;
    public GameObject Crystal;
    Transform enemy, boss, pickupHealth, item, grenade, crystal;

    // Start is called before the first frame update
    void Start()
    {
        enemy = Enemy.transform;
        boss = Boss.transform;
        pickupHealth = PickupHealth.transform;
        item = Item.transform;
        grenade = Grenade.transform;
        crystal = Crystal.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Enemy == null)
        {
            StartCoroutine(Wait());
            GameObject clone = Instantiate(Enemy, enemy.position, enemy.rotation);
        }
        if (Boss == null)
        {
            StartCoroutine(Wait());
            GameObject clone = Instantiate(Boss, boss.position, boss.rotation);
        }
        if (PickupHealth == null)
        {
            StartCoroutine(Wait());
            GameObject clone = Instantiate(PickupHealth, pickupHealth.position, pickupHealth.rotation);
        }
        if (Item == null)
        {
            StartCoroutine(Wait());
            GameObject clone = Instantiate(Item, item.position, item.rotation);
        }
        if (Grenade == null)
        {
            StartCoroutine(Wait());
            GameObject clone = Instantiate(Grenade, grenade.position, grenade.rotation);
        }
        if (Crystal == null)
        {
            StartCoroutine(Wait());
            GameObject clone = Instantiate(Crystal, crystal.position, crystal.rotation);
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(10f);
    }
}
