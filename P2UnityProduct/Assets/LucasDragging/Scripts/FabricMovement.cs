using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabricMovement : MonoBehaviour {
    
    public float moveSpeed = 0;
    private Dragging dragging;

    private void Start(){
        GameManager.Instance.OnEnterLevel += GameManager_OnEnterLevel;

        dragging = GameObject.Find("Needle").GetComponent<Dragging>();
    }

    private void GameManager_OnEnterLevel(object sender, EventArgs e){
        transform.localPosition = Vector3.zero;
    }

    private void FixedUpdate(){
        if (dragging.isDragging){
            transform.position = new Vector3(transform.position.x - moveSpeed, transform.position.y, transform.position.z);
        }
    }
}
