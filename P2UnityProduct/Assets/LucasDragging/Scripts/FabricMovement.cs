using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabricMovement : MonoBehaviour
{
    private bool moveTriggered = false;
    public float moveSpeed = 0.01f;

    public void StartMovement()
    {
        transform.position = new Vector3(transform.position.x - moveSpeed, transform.position.y, transform.position.z);
    }
}
