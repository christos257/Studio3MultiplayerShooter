using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUps : MonoBehaviour
{
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ActivatePowerUp(other.gameObject);
        }
    }

    public abstract void ActivatePowerUp(GameObject other);
    


}
