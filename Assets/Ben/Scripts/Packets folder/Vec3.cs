using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Vec3
{
    public float x;
    public float y;
    public float z;

    public Vec3()
    {
        x = 0;
        y = 0;
        z = 0;
    }

    public Vec3(float a, float b, float c)
    {
        x = a;
        y = b;
        z = c;
    }
    public Vec3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }
    public Vector3 GetVector()
    {
        return new Vector3(x, y, z);
    }
}
