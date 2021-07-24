using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject equippedGunPrefab;
    public Guns equippedGun;
    //public Guns previousGun;
    //public Guns nextGun;

    public GameObject[] upgradePrefabs;
    public Guns[] upgrades;
    public int activeGunIndex;

    public int health;
    public int maxHealth;

    private void Start()
    {
        health = maxHealth;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > equippedGun.nextFire)
        {
            if (equippedGun.clip <= 0 && equippedGun.fullMagazine >= 1)
            {
                StartCoroutine(equippedGun.Reload());
            }
            if (equippedGun.clip > 0)
            {
                equippedGun.nextFire = Time.time + equippedGun.fireRate;
                equippedGun.Shoot();
            }
            
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            health = health - 2;
            if (health <= 0)
            {
                health = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(equippedGun.Reload());
        }
    }

    public void UpgradeGun() 
    {
        equippedGunPrefab.SetActive(false);
        activeGunIndex++;
        equippedGun = upgrades[activeGunIndex];
        equippedGunPrefab = upgradePrefabs[activeGunIndex];
        equippedGunPrefab.SetActive(true);
    }
}
