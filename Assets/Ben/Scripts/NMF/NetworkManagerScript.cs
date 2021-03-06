using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using System;
public class NetworkManagerScript : MonoBehaviour
{
    Socket socket;
    [SerializeField]
    Button connectButton;
    [SerializeField]
    Button usernameSetButton;
    [SerializeField]
    InputField usernameInputF;
    [SerializeField]
    GameObject user1GO;
    [SerializeField]
    GameObject user2GO;
    [SerializeField]
    GameObject levelSelectionPanel;
    [SerializeField]
    bool isConnected;

    public string usernameInputString;
    public string nmID;
    public bool otherExists;

    public bool iAmHost;
    public bool hostChecked;
    BinaryFormatter sbf;
    BinaryFormatter rbf;

    MemoryStream sms;
    MemoryStream rms;

    public Queue<BasePacket> sendQueue;
    public static NetworkManagerScript instance = null;

    public List<GameObject> objSendList;
    int heyThereCount;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);

        Guid objGuid = Guid.NewGuid();
        nmID = objGuid.ToString();
        print(nmID);

    }
    private void OnLevelWasLoaded()
    {

    }
    void Start()
    {
        sbf = new BinaryFormatter();
        rbf = new BinaryFormatter();
        sms = new MemoryStream();
        rms = new MemoryStream();
        sendQueue = new Queue<BasePacket>();
        objSendList = new List<GameObject>();
        isConnected = false;
        otherExists = false;
        iAmHost = false;
        hostChecked = false;


        connectButton?.onClick.AddListener(() =>
       {
           if (!isConnected)
           {
               socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
               socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4420));
               socket.Blocking = false;
               isConnected = true;
               StartCoroutine("NetworkingLoop");
               // GameObject.Find("MainPanel").SetActive(false);
               levelSelectionPanel.SetActive(true);
           }
           else
           {
               Debug.LogError("Already connected bro");
           }


       });

        usernameSetButton?.onClick.AddListener(() =>
        {

            if (usernameInputF.text != null)
            {

                usernameInputString = usernameInputF.text;
                //  usernamePanel.SetActive(false);
            }

        });
    }
    IEnumerator NetworkingLoop()
    {
        while (isConnected)
        {
            if (!otherExists)
            {
                ChatPacket cp = new ChatPacket();
                cp.username = nmID;
                cp.message = "hey there";
                sendQueue.Enqueue(cp);
            }
            //if (user1GO != null || user2GO != null)
            //{
            //    if (usernameInputString == "1")
            //    {
            //        MovementPacket movePacket = new MovementPacket()
            //        {
            //            username = usernameInputString,
            //            x = user1GO.transform.position.x,
            //            y = user1GO.transform.position.y,
            //            z = user1GO.transform.position.z,
            //            rX = user1GO.transform.eulerAngles.x,
            //            rY = user1GO.transform.eulerAngles.y,
            //            rZ = user1GO.transform.eulerAngles.z,
            //            objectName = "user1"

            //        };

            //        sendQueue.Enqueue(movePacket);

            //        //sms.Seek(0, SeekOrigin.Begin);
            //        //sbf.Serialize(sms, movePacket);
            //        //socket.Send(sms.ToArray());
            //        //sms.Seek(0, SeekOrigin.Begin);

            //        // socket.Send(Util.Serialize(movePacket));
            //    }
            //    if (usernameInputString == "2")
            //    {
            //        MovementPacket movePacket = new MovementPacket()
            //        {
            //            username = usernameInputString,
            //            x = user2GO.transform.position.x,
            //            y = user2GO.transform.position.y,
            //            z = user2GO.transform.position.z,
            //            rX = user2GO.transform.eulerAngles.x,
            //            rY = user2GO.transform.eulerAngles.y,
            //            rZ = user2GO.transform.eulerAngles.z,
            //            objectName = "user2"
            //        };
            //        sendQueue.Enqueue(movePacket);

            //        //sms.Seek(0, SeekOrigin.Begin);
            //        //sbf.Serialize(sms, movePacket);
            //        //socket.Send(sms.ToArray());
            //        //sms.Seek(0, SeekOrigin.Begin);

            //        // socket.Send(Util.Serialize(movePacket));
            //    }
            //}
            //else
            //{
            //    try
            //    {
            //        user1GO = GameObject.Find("user1");
            //        user2GO = GameObject.Find("user2");
            //    }
            //    catch
            //    {
            //        print("user finding error");

            //    }

            //}
            for (int i = 0; i < objSendList.Count; i++)
            {
                if (objSendList[i].gameObject == null || objSendList[i].activeInHierarchy == false)
                {
                    objSendList.Remove(objSendList[i]);
                }
                MovementPacket movePacket = new MovementPacket()
                {
                    username = nmID,
                    x = objSendList[i].transform.position.x,
                    y = objSendList[i].transform.position.y,
                    z = objSendList[i].transform.position.z,
                    rX = objSendList[i].transform.eulerAngles.x,
                    rY = objSendList[i].transform.eulerAngles.y,
                    rZ = objSendList[i].transform.eulerAngles.z,
                    objectName = objSendList[i].name

                };
                print(movePacket.objectName);

                sendQueue.Enqueue(movePacket);
            }

            try
            {
                if (sendQueue.Count > 0)
                {
                    sms.Seek(0, SeekOrigin.Begin);
                    sbf.Serialize(sms, sendQueue.Dequeue());

                    sms.Seek(0, SeekOrigin.Begin);
                    socket.Send(sms.ToArray());
                }



            }
            catch
            {

                print("quee error");
            }
            try
            {


                byte[] buffer = new byte[1024];
                socket.Receive(buffer);

                rms.Seek(0, SeekOrigin.Begin);
                rms.Write(buffer, 0, 1024);
                rms.Seek(0, SeekOrigin.Begin);

                //BasePacket BP = (BasePacket)Util.Deserialize(buffer);
                try
                {
                    if (!hostChecked)
                    {
                        print("host checking");
                        try
                        {
                            string t = Encoding.ASCII.GetString(buffer);
                            print(t);
                            t = t.Replace("\0", "");
                            if (t == "h")
                            {

                                iAmHost = true;
                                hostChecked = true;
                                print("host");




                            }
                            else if (t == "nh")
                            {
                                iAmHost = false;
                                hostChecked = true;
                                print("no host");

                            }

                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }

                    BasePacket BP = (BasePacket)rbf.Deserialize(rms);
                    print("revein");
                    switch (BP.packetType)
                    {
                        case BasePacket.type.ChatType:
                            ChatPacket CP = (ChatPacket)BP;
                            if (CP.message == "hey there")
                            {
                                otherExists = true;
                                heyThereCount++;
                                if (heyThereCount > 10)
                                {
                                    ChatPacket cp = new ChatPacket();
                                    cp.username = nmID;
                                    cp.message = "hey there";
                                    sendQueue.Enqueue(cp);
                                    heyThereCount = 0;
                                }
                            }
                            else if (CP.message == "iLose")
                            {

                                GameManager.instance.iWin = true;
                            }

                            Debug.LogError(CP.username + ": " + CP.message);
                            break;
                        case BasePacket.type.MovementType:
                            MovementPacket MP = (MovementPacket)BP;
                            GameObject g = GameObject.Find(MP.objectName);
                            g.transform.position = new Vector3(MP.x, MP.y, MP.z);
                            g.transform.rotation = Quaternion.Euler(MP.rX, MP.rY, MP.rZ);
                            break;
                        case BasePacket.type.InstantiateType:
                            InstantiatePacket IP = (InstantiatePacket)BP;

                            Instantiate(Resources.Load<GameObject>(IP.objectName),
                           (IP.position.GetVector()),
                                Quaternion.Euler(IP.rotation.GetVector()));
                            //AddInGOList(tempGOReceive);
                            break;
                        case BasePacket.type.SceneTransitionType:
                            SceneTransitionPacket stp = (SceneTransitionPacket)BP;
                            SceneManager.LoadScene(stp.sceneIndex);
                            break;
                        default:
                            break;
                    }
                }
                catch
                {

                    print("recevin error");
                }

                //    Debug.LogError(Encoding.ASCII.GetString(buffer));

            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode != SocketError.WouldBlock)
                {
                    Debug.LogError(ex);
                }

            }
            yield return new WaitForSeconds(0.04f);
        }


    }
    // Update is called once per frame
    void Update()
    {



    }
    public void InstanOnNet(string goName, Vector3 pos, Vector3 rot)
    {
        InstantiatePacket ip = new InstantiatePacket()
        {
            objectName = goName,
            position = new Vec3(pos),
            rotation = new Vec3(rot),
        };

        sendQueue.Enqueue(ip);

        /*tempGOMethod =*/

        Instantiate(Resources.Load<GameObject>(ip.objectName),
               (ip.position.GetVector()),
                   Quaternion.Euler(ip.rotation.GetVector()));
        //AddInGOList(tempGOMethod);
    }
    public void InstanPlayerOnNet()
    {



    }
    public void LevelSelectionButton(int i)
    {

        SceneTransitionPacket stp = new SceneTransitionPacket()
        {
            sceneIndex = i
        };
        sendQueue.Enqueue(stp);
        SceneManager.LoadScene(stp.sceneIndex);


    }
    public void SendMessageOnNet(string mes) 
    {
        ChatPacket cp = new ChatPacket();
        cp.username = nmID;
        cp.message = mes;
        sendQueue.Enqueue(cp);

    }
    public void SceneChangeOnNet(int i) 
    {

        LevelSelectionButton(i);
    }
    public void LevelJoined()
    {
        if (iAmHost)
        {
            try
            {
                user1GO = GameObject.Find("user1");
                user2GO = GameObject.Find("user2");

                user1GO.GetComponent<UserScript>().userScriptId = nmID;
                user1GO.GetComponent<UserScript>().isMine = true;
                user2GO.GetComponent<UserScript>().isMine = false;
                GameManager.instance.userID = 1;
                objSendList.Add(user1GO);
                print("user id assigned");
            }
            catch
            {
                print("user finding error");

            }
        }
        else
        {
            try
            {
                user1GO = GameObject.Find("user1");
                user2GO = GameObject.Find("user2");

                user2GO.GetComponent<UserScript>().userScriptId = nmID;
                user2GO.GetComponent<UserScript>().isMine = true;
                user1GO.GetComponent<UserScript>().isMine = false;
                GameManager.instance.userID = 2;
                objSendList.Add(user2GO);
                print("user id assigned");
            }
            catch
            {
                print("user finding error");

            }
        }
    }
}
