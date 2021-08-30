using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserScript : MonoBehaviour
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

    public string userScriptId;
    public GameObject laserTrap;
    bool trapShot;
    bool trapActivated;
    bool isAlive;
    public bool isMine;
    GameObject tempTrap;
    Rigidbody rb;
    public float mH;
    public float mV;
    public bool spinning;
    public bool canSpin;
    public int laserTrapAmmo;
    public float hp;

    public FloatSO mhSO;
    public FloatSO mvSO;
    public BoolSO moveBO;
    public BoolSO spinBO;
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
        isAlive = true;
        laserTrapAmmo = 3;
        hp = 100;
    }
    public void Spin()
    {

        transform.Rotate(0, 1 * Time.deltaTime * spinSpeed, 0);


    }

    // Update is called once per frame
    void Update()
    {
        if (NetworkManagerScript.instance.nmID == userScriptId)
        {

            if (isAlive && hp <= 0)
            {
                Debug.LogError("I died my friend");
                isAlive = false;

                GameManager.instance.iLose = true;
                NetworkManagerScript.instance.SendMessageOnNet("iLose");
            }
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
            if (spinBO.value && canSpin)
            {
                spinning = true;
                canSpin = false;
                Spin();
                gameObject.tag = "Spinning";
                spinBO.value = false;

            }
            //mH = Input.GetAxis("Horizontal");
            //mV = Input.GetAxis("Vertical");
            mH = mhSO.value;
            mV = mvSO.value;
            rb.velocity = new Vector3(mH * speed, rb.velocity.y, mV * speed);
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);


                if (/*!moveBO.value && */GameManager.instance.prepDone)
                {
                    foreach (Touch t in Input.touches)
                    {
                        Ray ray = mainCam.ScreenPointToRay(t.position);
                        if (!spinning && Physics.Raycast(ray, out RaycastHit raycastHit) && raycastHit.collider.tag == "Floor")
                        {
                            target = raycastHit.point;
                            // target.x = transform.position.x;
                            target.y = transform.position.y;
                            transform.LookAt(target);

                            if (t.phase == TouchPhase.Began )
                            {
                                NetworkManagerScript.instance.InstanOnNet("BulletPrefab", new Vector3(sp.position.x, sp.position.y, sp.position.z), new Vector3(0, transform.eulerAngles.y, 0));
                            }
                        }
                    }
                   

                }
                else
                {
                    if (laserTrapAmmo > 0)
                    {

                        if (touch.phase == TouchPhase.Began)
                        {
                            if (!trapShot)
                            {

                                Ray mobileRays = mainCam.ScreenPointToRay(touch.position);
                                if (Physics.Raycast(mobileRays, out RaycastHit raycastHits))
                                {
                                    if (raycastHits.collider.tag == "Wall")
                                    {
                                        tempTrap = Instantiate(laserTrap, new Vector3(raycastHits.point.x, -0.5f, raycastHits.point.z), Quaternion.identity);
                                        trapShot = true;
                                    }


                                }
                            }
                            else if (trapShot)
                            {
                                Ray mobileRays = mainCam.ScreenPointToRay(touch.position);
                                if (Physics.Raycast(mobileRays, out RaycastHit raycastHits))
                                {
                                    tempTrap.transform.LookAt(raycastHits.point);
                                    trapShot = false;
                                    NetworkManagerScript.instance.InstanOnNet("LaserTrap",
                                                          new Vector3(tempTrap.transform.position.x, -0.5f, tempTrap.transform.position.z),
                                                             new Vector3(0, tempTrap.transform.eulerAngles.y, tempTrap.transform.eulerAngles.z));
                                    Destroy(tempTrap.gameObject);
                                    laserTrapAmmo--;
                                }

                            }
                        }

                    }
                }

            }

            //if (Input.GetMouseButtonDown(0))
            //{
            //    if (GameManager.instance.prepDone)
            //    {
            //        NetworkManagerScript.instance.InstanOnNet("BulletPrefab", new Vector3(sp.position.x, sp.position.y, sp.position.z), new Vector3(0, transform.eulerAngles.y, 0));

            //    }
            //}
            //if (Input.GetMouseButtonDown(1))
            //{
            //    //  NM.instance.InstanOnNet("BulletPrefab", new Vector3(sp.position.x, sp.position.y, sp.position.z), new Vector3(0, transform.eulerAngles.y, 0));
            //    if (laserTrapAmmo > 0 && !GameManager.instance.prepDone)
            //    {
            //        if (!trapShot)
            //        {
            //            Ray rays = mainCam.ScreenPointToRay(Input.mousePosition);
            //            if (Physics.Raycast(rays, out RaycastHit raycastHits))
            //            {
            //                if (raycastHits.transform.gameObject.tag == "Wall")
            //                {
            //                    tempTrap = Instantiate(laserTrap, new Vector3(raycastHits.point.x, -0.5f, raycastHits.point.z), Quaternion.identity);
            //                    trapShot = true;
            //                }


            //            }
            //        }
            //        else if (trapShot)
            //        {
            //            Ray rays = mainCam.ScreenPointToRay(Input.mousePosition);
            //            if (Physics.Raycast(rays, out RaycastHit raycastHits))
            //            {
            //                tempTrap.transform.LookAt(raycastHits.point);
            //                trapShot = false;
            //                NetworkManagerScript.instance.InstanOnNet("LaserTrap",
            //                                      new Vector3(tempTrap.transform.position.x, -0.5f, tempTrap.transform.position.z),
            //                                         new Vector3(0, tempTrap.transform.eulerAngles.y, tempTrap.transform.eulerAngles.z));
            //                Destroy(tempTrap.gameObject);
            //                laserTrapAmmo--;
            //            }

            //        }
            //    }

            //}

        }

    }
}
