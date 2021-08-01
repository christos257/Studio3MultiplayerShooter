using System.Collections;
using System;
using UnityEngine;

[Serializable]
public class MovementPacket : BasePacket
{
    public float x;
    public float y;
    public float z;
    public float rX;
    public float rY;
    public float rZ;
    public string objectName;

    public MovementPacket()
    {

        packetType = type.MovementType;
    }
}
