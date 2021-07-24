using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsPickUp : PowerUps
{
    public override void ActivatePowerUp(GameObject other)
    {
        Player playerScript = other.GetComponent<Player>();
        playerScript.UpgradeGun();
        Destroy(gameObject);
    }
}
