using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPickUp : PowerUps
{
    public override void ActivatePowerUp(GameObject other)
    {
        Player playerScript = other.GetComponent<Player>();
        Guns gunEquipped = other.GetComponent<Guns>();
        playerScript.equippedGun.fullMagazine = playerScript.equippedGun.fullMagazine + playerScript.equippedGun.magazine * 2;

        if (playerScript.equippedGun.fullMagazine >= playerScript.equippedGun.magazine * 4)
        {
            playerScript.equippedGun.fullMagazine = playerScript.equippedGun.magazine * 4;
        }
        Destroy(gameObject);
    }
}
