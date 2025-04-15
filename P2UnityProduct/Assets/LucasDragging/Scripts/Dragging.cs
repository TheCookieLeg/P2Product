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
        // TOUCH CONTROLS
        if (Input.touchCount > 0) // if more than zero fingers is touching the screen
        {
            Debug.Log("I GOT TOUCHED");
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
                    if (spawner != null)
                    {
                        spawner.GetComponent<FabricMovement>().enabled = true;
                    }
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
        } else {
            // MOUSE CONTROLS - TESTING
            if (Input.GetMouseButtonDown(0))
            {
                UnityEngine.Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new UnityEngine.Vector3(Input.mousePosition.x, Input.mousePosition.y, zDepth));
                UnityEngine.Vector2 mouseWorldPos2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos2D, UnityEngine.Vector2.zero);

                if (hit.collider != null && hit.transform == transform)
                {
                    isDragging = true;
                    offset = transform.position - (UnityEngine.Vector3)mouseWorldPos2D;

                    if (spawner != null)
                    {
                        spawner.GetComponent<FabricMovement>().enabled = true;
                    }
                }
            }
            else if (Input.GetMouseButton(0) && isDragging)
            {
                UnityEngine.Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new UnityEngine.Vector3(Input.mousePosition.x, Input.mousePosition.y, zDepth));
                UnityEngine.Vector3 targetPos = new UnityEngine.Vector3(mouseWorldPos.x + offset.x, mouseWorldPos.y + offset.y, mouseWorldPos.z + offset.z);
                transform.position = UnityEngine.Vector3.Lerp(transform.position, targetPos, 0.4f);
            }
            else if (Input.GetMouseButtonUp(0))
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
