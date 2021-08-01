using System.Collections;
using System;
using UnityEngine;

[Serializable]
public class SceneTransitionPacket : BasePacket
{
    public int sceneIndex;

    public SceneTransitionPacket()
    {

        packetType = type.SceneTransitionType;
    }
}
