using System.Collections;
using System;
using UnityEngine;

[Serializable]
public class InstantiatePacket : BasePacket
{
    public Vec3 position;
    public Vec3 rotation;
    public string objectName;

    public InstantiatePacket()
    {

        packetType = type.InstantiateType;
    }
}
