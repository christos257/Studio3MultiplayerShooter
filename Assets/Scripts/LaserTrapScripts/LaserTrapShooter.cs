using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrapShooter : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    public GameObject laserTrap;
    bool trapShot;
    bool trapActivated;
    GameObject tempTrap;
    void Start()
    {
        trapShot = false;
        trapActivated = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (!trapShot)
            {
                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit raycastHit))
                {
                    tempTrap = Instantiate(laserTrap, raycastHit.point, Quaternion.identity);
                    trapShot = true;
                    
                }
            }
            else if (trapShot)
            {
                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit raycastHit))
                {
                    tempTrap.transform.LookAt(raycastHit.point);
                    trapShot = false;

                }
                
            }

        }
    }
}
