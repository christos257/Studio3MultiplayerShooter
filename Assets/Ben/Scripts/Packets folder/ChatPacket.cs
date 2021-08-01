using System.Collections;
using System;
using UnityEngine;

[Serializable]
public class ChatPacket : BasePacket
{
    public string message;

    public ChatPacket()
    {

        packetType = type.ChatType;
    }
}
