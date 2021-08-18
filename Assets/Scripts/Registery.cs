using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
struct UserData
{
    public string fname;
    public string lname;
    public string username;
    public string password;
    public string age;
    public string email;

    public string GetDataInUrlEncoding()
    {
        return $"?fname={fname}&lname={lname}&username={username}&password={password}&age={age}&email={email}";
    }
    public string GetNameInUrlEncoding()
    {
        return $"?fname={fname}";
    }
}
[System.Serializable]
struct UsersData
{
    public UserData[] usersData;
}
public class Registery : MonoBehaviour
{

    [SerializeField]
    public string sendDataserverUrl;
    [SerializeField] 
    public string getDataserverUrl;

    public InputField firstNameInput;
    public InputField lastNameInput;
    public InputField ageInput;
    public InputField emailInput;
    public InputField usernameInput;
    public InputField passwordInput;


    UserData userData;

    UsersData[] usersData;

  
    void Start()
    {
         
        //StartCoroutine(GetUserDataUsingGetRequest(getDataserverUrl, userDatas.GetNameInUrlEncoding()));
    }

    public void UserData()
    {
        userData = new UserData()
        {
            fname = firstNameInput.text,
            lname = lastNameInput.text,
            age = ageInput.text,
            email = emailInput.text,
            username = usernameInput.text,
            password = passwordInput.text
        };
        //StartCoroutine(SendUserDataUsingGetRequest(sendDataserverUrl, userData.GetDataInUrlEncoding()));
        StartCoroutine(SendUserDataUsingPostRequest(sendDataserverUrl, userData));
        //StartCoroutine(GetUserDataUsingGetRequest(getDataserverUrl, userData.GetNameInUrlEncoding()));
    }


    void Update()
    {

    }

    IEnumerator SendUserDataUsingGetRequest(string serverUrl, string urlEncodedData)
    {
        UnityWebRequest webRequest = new UnityWebRequest(serverUrl + urlEncodedData, "GET");
        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            print(webRequest.error);
        }
        else
        {
            print("Web request sent");
        }
    }
    IEnumerator SendUserDataUsingPostRequest(string serverUrl, UserData userData)
    {
        string jsonData = JsonUtility.ToJson(userData);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(serverUrl, jsonData))
        {
            webRequest.SetRequestHeader("content-type", "application/json");
            webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData));

            yield return webRequest.SendWebRequest();
        }
    }

    IEnumerator GetUserDataUsingGetRequest(string serverUrl, string urlEncodedData)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(serverUrl + urlEncodedData);
        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            print(webRequest.error);
        }
        else
        {
            print("Web request sent");
            print(webRequest.downloadHandler.text);
            UsersData usersData = JsonUtility.FromJson<UsersData>(webRequest.downloadHandler.text);
            for (int i = 0; i < usersData.usersData.Length; i++)
            {
                print(usersData.usersData[i].fname);
            }

        }
    }
}
