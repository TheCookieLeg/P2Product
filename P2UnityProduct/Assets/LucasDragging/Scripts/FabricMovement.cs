using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabricMovement : MonoBehaviour
{
    public float moveSpeed = 0.01f;

    void Start()
    {
        enabled = false;
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x - moveSpeed, transform.position.y, transform.position.z);
    }
}
