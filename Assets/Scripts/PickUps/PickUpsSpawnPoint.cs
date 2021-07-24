using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpsSpawnPoint : MonoBehaviour
{
    public GameObject pickUps;
    public float respawnTimer;
    void Start()
    {
        StartCoroutine(InitialSpawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(RespawnPoint());
        }
    }


    IEnumerator InitialSpawn() 
    {
        yield return new WaitForSeconds(5.0f);
        Instantiate(pickUps, this.transform.position, this.transform.rotation);
    }

    IEnumerator RespawnPoint()
    {
        yield return new WaitForSeconds(respawnTimer);
        Instantiate(pickUps, this.transform.position, this.transform.rotation);

    }

    
}
