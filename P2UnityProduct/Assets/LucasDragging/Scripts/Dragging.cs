using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;

public class Dragging : MonoBehaviour
{
    [Header("Dragging variables")]
    private UnityEngine.Vector3 offset; // offset between the touch point and the center of the object
    [SerializeField] private bool isDragging = false;
    private float zDepth;

    [Header("Rotation Variables")]
    private UnityEngine.Vector2 oldPos;
    private UnityEngine.Vector2 newPos;

    public GameObject spawner;

    // Start is called before the first frame update
    void Start()
    {
        zDepth = Camera.main.WorldToScreenPoint(transform.position).z;
        spawner = GameObject.FindGameObjectWithTag("Spawner");
    }

    // Update is called once per frame
    void Update()
    {
        DragAndDrop();
        RotateObject();
    }

    private void DragAndDrop()
    {
        if (Input.touchCount > 0) // if more than zero fingers is touching the screen
        {
            Debug.Log("I GOT TOUCHED");
            if (spawner != null)
            {
                spawner.GetComponent<FabricMovement>().enabled = true;
            }
            Touch touch = Input.GetTouch(0); // Gets info on the first finger that touches the screen (struct variable)

            UnityEngine.Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new UnityEngine.Vector3(touch.position.x, touch.position.y, zDepth));

            // TouchPhase.Began is the first frame that the finger touches the screen. Here we can calculate some values
            if (touch.phase == TouchPhase.Began)
            {
                UnityEngine.Vector2 touchWorldPos2D = Camera.main.ScreenToWorldPoint(touch.position);
                RaycastHit2D hit = Physics2D.Raycast(touchWorldPos2D, UnityEngine.Vector2.zero);

                if (hit.collider != null && hit.transform == transform)
                {
                    isDragging = true;
                    offset = transform.position - (UnityEngine.Vector3)touchWorldPos2D;
                }
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                UnityEngine.Vector3 targetPos = new UnityEngine.Vector3(touchPosition.x + offset.x, touchPosition.y + offset.y, touchPosition.z + offset.z);
                transform.position = UnityEngine.Vector3.Lerp(transform.position, targetPos, 0.4f);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (spawner != null)
                {
                    spawner.GetComponent<FabricMovement>().enabled = false;
                }
                isDragging = false;
            }
        }
    }

    private void RotateObject()
    {
        if (isDragging)
        {
            newPos = transform.position;
            UnityEngine.Vector2 movement = newPos - oldPos;

            if (movement.magnitude > 0.01f)
            {
                UnityEngine.Quaternion targetRotation = UnityEngine.Quaternion.LookRotation(UnityEngine.Vector3.forward, movement.normalized);

                transform.rotation = UnityEngine.Quaternion.Lerp(transform.rotation, targetRotation, 0.4f);               
                
                oldPos = newPos;
            }

            
        }
    }
}
