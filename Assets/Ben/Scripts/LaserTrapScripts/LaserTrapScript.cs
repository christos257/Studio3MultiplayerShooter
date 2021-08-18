using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrapScript : MonoBehaviour
{
    LineRenderer lr;
    GameObject firePE;


    void Start()
    {
        lr = GetComponent<LineRenderer>();
        firePE = transform.Find("FirePE").gameObject;
        firePE.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        lr.SetPosition(0, transform.position);
        RaycastHit rHit;
        if (Physics.Raycast(transform.position, transform.forward, out rHit))
        {
            if (rHit.collider && rHit.collider.tag != "Spinning")
            {

                lr.SetPosition(1, rHit.point);
                if (rHit.collider.tag == "Player")
                {


                    firePE.SetActive(true);
                    firePE.transform.SetPositionAndRotation(rHit.point, Quaternion.identity);


                }
                else if (firePE.activeSelf)
                {
                    firePE.SetActive(false);
                }

            }

        }
        else
        {
            lr.SetPosition(1, transform.position + (transform.forward * 5000));
            firePE.SetActive(false);
        }
    }
}
