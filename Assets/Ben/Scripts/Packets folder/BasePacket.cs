using System.Collections;
using System;
using UnityEngine;

[Serializable]
public class BasePacket 
{
    public string username;
    public enum type { ChatType, MovementType,InstantiateType, SceneTransitionType};
    public type packetType;
}
