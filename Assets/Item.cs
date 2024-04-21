using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private float MoveSpeed = 0.2f;
    void FixedUpdate()
    {
        transform.position += Vector3.left * MoveSpeed;
    }
}
