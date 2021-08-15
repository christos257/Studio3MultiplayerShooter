using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User1Script : MonoBehaviour
{
    public float speed;
    public float jumpSpeed;
    public float spinSpeed;
    float spinTimer;
    float cantSpinTimer;
    public float cantSpinTimerLimit;
    public Camera mainCam;
    Vector3 target = new Vector3();
    public Transform sp;


    public GameObject laserTrap;
    bool trapShot;
    bool trapActivated;
    GameObject tempTrap;
    Rigidbody rb;
    float mH;
    float mV;
    public bool spinning;
    public bool canSpin;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        trapShot = false;
        trapActivated = false;
        spinning = false;
        canSpin = true;
    }
    public void Spin()
    {

        transform.Rotate(0, 1 * Time.deltaTime * spinSpeed, 0);


    }

    // Update is called once per frame
    void Update()
    {
        if (NetworkManagerScript.instance.usernameInputString == "1")
        {
            //if (Input.GetKey(KeyCode.W))
            //{
            //    transform.Translate(0, 0, 1 * Time.deltaTime * speed, Space.World);

            //}
            //if (Input.GetKey(KeyCode.A))
            //{
            //    transform.Translate(-1 * Time.deltaTime * speed, 0, 0, Space.World);
            //}
            //if (Input.GetKey(KeyCode.S))
            //{
            //    transform.Translate(0, 0, -1 * Time.deltaTime * speed, Space.World);
            //}
            if (spinning)
            {
                spinTimer += Time.deltaTime;
                Spin();
                if (spinTimer > 1)
                {
                    spinning = false;
                    spinTimer = 0;
                    gameObject.tag = "Player";
                }
            }
            if (!canSpin)
            {
                cantSpinTimer += Time.deltaTime;
                if (cantSpinTimer > cantSpinTimerLimit)
                {
                    canSpin = true;
                    cantSpinTimer = 0;
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) && canSpin)
            {
                spinning = true;
                canSpin = false;
                Spin();
                gameObject.tag = "Spinning";

            }
            mH = Input.GetAxis("Horizontal");
            mV = Input.GetAxis("Vertical");
            rb.velocity = new Vector3(mH * speed, rb.velocity.y, mV * speed);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            }
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (!spinning && Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                target = raycastHit.point;
                // target.x = transform.position.x;
                target.y = transform.position.y;
                transform.LookAt(target);


            }
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.instance.prepDone)
                {
                    NetworkManagerScript.instance.InstanOnNet("BulletPrefab", new Vector3(sp.position.x, sp.position.y, sp.position.z), new Vector3(0, transform.eulerAngles.y, 0));

                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                //  NM.instance.InstanOnNet("BulletPrefab", new Vector3(sp.position.x, sp.position.y, sp.position.z), new Vector3(0, transform.eulerAngles.y, 0));
                if (!GameManager.instance.prepDone)
                {
                    if (!trapShot)
                    {
                        Ray rays = mainCam.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(rays, out RaycastHit raycastHits))
                        {
                            if (raycastHits.transform.gameObject.tag == "Wall")
                            {
                                tempTrap = Instantiate(laserTrap, new Vector3(raycastHits.point.x, -0.5f, raycastHits.point.z), Quaternion.identity);
                                trapShot = true;
                            }


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
                                                  new Vector3(tempTrap.transform.position.x, -0.5f, tempTrap.transform.position.z),
                                                     new Vector3(0, tempTrap.transform.eulerAngles.y, tempTrap.transform.eulerAngles.z));
                            Destroy(tempTrap.gameObject);
                        }

                    }
                }

            }

        }

    }
}
