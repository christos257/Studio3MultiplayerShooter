using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool prepDone;
    public float prepTime;
    public Text prepTimeText;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (prepTime <= 0)
        {
            prepDone = true;
            prepTimeText.text = "";
        }
        else if (!prepDone)
        {
            prepTime = prepTime - Time.deltaTime;
            prepTimeText.text = "Prep Phase - " + (int)prepTime;
        }

    }
}
