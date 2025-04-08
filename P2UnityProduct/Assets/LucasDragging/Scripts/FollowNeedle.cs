using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowNeedle : MonoBehaviour
{
    GameObject needleBack;

    // Start is called before the first frame update
    void Start()
    {
        needleBack = GameObject.FindWithTag("NeedleBack");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = needleBack.transform.position;
    }
}
