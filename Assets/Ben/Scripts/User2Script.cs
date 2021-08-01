using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User2Script : MonoBehaviour
{
    public float speed;
    public Camera mainCam;
    Vector3 target = new Vector3();
    public Transform sp;


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
        if (NetworkManagerScript.instance.usernameInputString == "2")
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(0, 0, 1 * Time.deltaTime * speed, Space.World);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(-1 * Time.deltaTime * speed, 0, 0, Space.World);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(0, 0, -1 * Time.deltaTime * speed, Space.World);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(1 * Time.deltaTime * speed, 0, 0, Space.World);
            }

            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                target = raycastHit.point;
                // target.x = transform.position.x;
                target.y = transform.position.y;
                transform.LookAt(target);


            }
            if (Input.GetMouseButtonDown(0))
            {
                // NetworkManagerScript.instance.InstanOnNet("BulletPrefab", new Vector3(sp.position.x, sp.position.y, sp.position.z), new Vector3(0, transform.eulerAngles.y, 0));
            }
            if (Input.GetMouseButtonDown(1))
            {
                //  NM.instance.InstanOnNet("BulletPrefab", new Vector3(sp.position.x, sp.position.y, sp.position.z), new Vector3(0, transform.eulerAngles.y, 0));

                if (!trapShot)
                {
                    Ray rays = mainCam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(rays, out RaycastHit raycastHits))
                    {
                        tempTrap = Instantiate(laserTrap, new Vector3(raycastHits.point.x, 0, raycastHits.point.z), Quaternion.identity);
                        trapShot = true;

                    }
                }
                else if (trapShot)
                {
                    Ray rays = mainCam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(rays, out RaycastHit raycastHits))
                    {
                        tempTrap.transform.LookAt(raycastHits.point);
                        trapShot = false;
                        NetworkManagerScript.instance.InstanOnNet("LaserTrap",
                                              new Vector3(tempTrap.transform.position.x, 0, tempTrap.transform.position.z),
                                                 new Vector3(0, tempTrap.transform.eulerAngles.y, tempTrap.transform.eulerAngles.z));
                        //Destroy(tempTrap.gameObject);
                    }

                }
            }

        }

    }
}
