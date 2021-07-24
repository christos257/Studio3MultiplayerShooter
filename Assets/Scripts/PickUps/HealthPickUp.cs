using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : PowerUps
{
    public override void ActivatePowerUp(GameObject other)
    {
        Player playerScript = other.GetComponent<Player>();
        playerScript.health = playerScript.health + 5;
        if (playerScript.health >= playerScript.maxHealth)
        {
            playerScript.health = playerScript.maxHealth;
        }
        Destroy(gameObject);
    }
}
