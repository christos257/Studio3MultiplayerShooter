using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Guns : MonoBehaviour
{

    public GameObject player;
    public float damage;
    public float range;
    public float fireRate;
    public float bulletSpeed;
    public float knockbackForce;
    public float nextFire;
    public int magazine;
    public int clip;
    public int fullMagazine;
    public int ammoRemaining;
    public TMP_Text magazineText;
    public bool isReloading;

    public GameObject bullet;
    public Transform gunMuzzle;
    public Rigidbody rb;

    public Camera Cam;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        clip = magazine;
        fullMagazine = magazine * 4;
        isReloading = false;
        //magazineText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        magazineText.text = $"{clip} / {fullMagazine}";
        ammoRemaining = magazine - clip;
    }

    public void Shoot() 
    {
        if (clip == 0)
        {
            return;
        }
        GameObject bulletInstance;
        bulletInstance = Instantiate(bullet, gunMuzzle.position, gunMuzzle.rotation);
        bulletInstance.GetComponent<Bullet>().velocity = player.transform.forward * bulletSpeed;
        clip--;
        Debug.Log("Bullet should have shot");

    }

    public IEnumerator Reload()
    {
        if (isReloading == true)
        {
            yield break;
        }
        isReloading = true;
        yield return new WaitForSeconds(1);
        //add animations
        if (fullMagazine <= 0)
        {
            fullMagazine = 0;
            clip = 0;
            Debug.Log("You don't have any bullets left");
        }
        fullMagazine = fullMagazine - ammoRemaining;
        clip = magazine;
        isReloading = false;
       
    }
}
