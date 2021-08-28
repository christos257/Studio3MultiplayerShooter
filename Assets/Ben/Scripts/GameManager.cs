using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool prepDone;
    public bool iLose;
    public bool iWin;
    public float prepTime;
     float userHealth;
    public Text prepTimeText;
    public Text laserAmmoText;
    public Text hpText;
    public Image hpHeartGO;
    public int userID;
    public GameObject user1GO;
    public GameObject user2GO;
    public GameObject winPanelGO;
    public GameObject losePanelGO;
    Color tempColor;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        prepDone = false;
        iLose = false;
        iWin = false;
        user1GO = GameObject.Find("user1");
        user2GO = GameObject.Find("user2");
        tempColor = hpHeartGO.color;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (prepTime <= 0)
        {
            prepDone = true;
            prepTimeText.text = "";
            laserAmmoText.text =  "";

        }
        else if (!prepDone)
        {
            prepTime = prepTime - Time.deltaTime;
            prepTimeText.text = "Prep Phase - " + (int)prepTime;
            if (userID==1)
            {
                laserAmmoText.text = "Laser Traps - " + user1GO.GetComponent<UserScript>().laserTrapAmmo;
            }
            else if(userID == 2)
            {
                laserAmmoText.text = "Laser Traps - " + user2GO.GetComponent<UserScript>().laserTrapAmmo;
            }
            
        }

        if (userID == 1)
        {
            userHealth = user1GO.GetComponent<UserScript>().hp;
            tempColor.a = userHealth / 100;
            hpHeartGO.color = tempColor;
            hpText.text = ((int)userHealth).ToString();
            
        }
        else if (userID == 2)
        {
            userHealth = user2GO.GetComponent<UserScript>().hp;
            tempColor.a = userHealth / 100;
            hpHeartGO.color = tempColor;
            hpText.text = ((int)userHealth).ToString();
        }
        if (iWin)
        {
            winPanelGO.SetActive(true);
            StartCoroutine("EndGame");
        }
        if (iLose)
        {
            losePanelGO.SetActive(true);
            StartCoroutine("EndGame");
        }
    }
    IEnumerator EndGame()
    {
        iWin = false;
        iLose = false;
        Debug.Log("co start");
        yield return new WaitForSeconds(5f);

       
        Debug.Log("co done");
        //NetworkManagerScript.instance.SceneChangeOnNet(0);
    }
}
