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
    bool isConnected;

    public string usernameInputString;


    BinaryFormatter sbf;
    BinaryFormatter rbf;

    MemoryStream sms;
    MemoryStream rms;

    Queue<BasePacket> sendQueue;
    public static NetworkManagerScript instance = null;

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
    }
    void Start()
    {
        sbf = new BinaryFormatter();
        rbf = new BinaryFormatter();
        sms = new MemoryStream();
        rms = new MemoryStream();
        sendQueue = new Queue<BasePacket>();
        isConnected = false;


        connectButton?.onClick.AddListener(() =>
       {
           if (!isConnected)
           {
               socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
               socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4420));
               socket.Blocking = false;
               isConnected = true;
               // GameObject.Find("MainPanel").SetActive(false);
               GameObject.Find("LobbyPanel").SetActive(false);
           }
           else
           {
               Debug.LogError("Already connected bro");
           }


       });

        usernameSetButton?.onClick.AddListener(() =>
        {
            print("out");
            Debug.Log("outie");
            if (usernameInputF.text != null)
            {
                print("ini");
                usernameInputString = usernameInputF.text;
                //  usernamePanel.SetActive(false);
            }

        });
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("asdwease");
        if (isConnected)
        {
            if (user1GO != null || user2GO != null)
            {
                if (usernameInputString == "1")
                {
                    MovementPacket movePacket = new MovementPacket()
                    {
                        username = usernameInputString,
                        x = user1GO.transform.position.x,
                        y = user1GO.transform.position.y,
                        z = user1GO.transform.position.z,
                        rX = user1GO.transform.eulerAngles.x,
                        rY = user1GO.transform.eulerAngles.y,
                        rZ = user1GO.transform.eulerAngles.z,
                        objectName = "user1"

                    };

                    sendQueue.Enqueue(movePacket);

                    //sms.Seek(0, SeekOrigin.Begin);
                    //sbf.Serialize(sms, movePacket);
                    //socket.Send(sms.ToArray());
                    //sms.Seek(0, SeekOrigin.Begin);

                    // socket.Send(Util.Serialize(movePacket));
                }
                if (usernameInputString == "2")
                {
                    MovementPacket movePacket = new MovementPacket()
                    {
                        username = usernameInputString,
                        x = user2GO.transform.position.x,
                        y = user2GO.transform.position.y,
                        z = user2GO.transform.position.z,
                        rX = user2GO.transform.eulerAngles.x,
                        rY = user2GO.transform.eulerAngles.y,
                        rZ = user2GO.transform.eulerAngles.z,
                        objectName = "user2"
                    };
                    sendQueue.Enqueue(movePacket);

                    //sms.Seek(0, SeekOrigin.Begin);
                    //sbf.Serialize(sms, movePacket);
                    //socket.Send(sms.ToArray());
                    //sms.Seek(0, SeekOrigin.Begin);

                    // socket.Send(Util.Serialize(movePacket));
                }
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
                    BasePacket BP = (BasePacket)rbf.Deserialize(rms);

                    switch (BP.packetType)
                    {
                        case BasePacket.type.ChatType:
                            ChatPacket CP = (ChatPacket)BP;
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
        }

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
}
