using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckOtherExists : MonoBehaviour
{
    [SerializeField]
    Button buttonOne;
    [SerializeField]
    Button buttonTwo;
    bool otherDoesExist;
    void Awake()
    {
        buttonOne.interactable = false;
        buttonTwo.interactable = false;
        otherDoesExist = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!otherDoesExist && NetworkManagerScript.instance.otherExists)
        {
            buttonOne.interactable = true;
            buttonTwo.interactable = true;
            otherDoesExist = true;
        }
    }
}
