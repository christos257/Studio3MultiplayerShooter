using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelJoinedCaller : MonoBehaviour
{
    public bool called;
    void Start()
    {
        called = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!called && NetworkManagerScript.instance!=null)
        {
            NetworkManagerScript.instance.LevelJoined();
            called = true;
        }
    }
}
