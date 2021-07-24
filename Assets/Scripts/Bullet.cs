using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Guns gun;
    public Vector3 velocity;

    void Start()
    {
       Destroy(gameObject, 2.0f);
    }

    void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }
}
