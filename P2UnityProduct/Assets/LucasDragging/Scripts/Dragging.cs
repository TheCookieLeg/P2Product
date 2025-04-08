using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Dragging : MonoBehaviour
{
    private Vector3 offset; // offset between the touch point and the center of the object
    [SerializeField] private bool isDragging = false;
    private float zDepth;

    // Start is called before the first frame update
    void Start()
    {
        zDepth = Camera.main.WorldToScreenPoint(transform.position).z;
    }

    // Update is called once per frame
    void Update()
    {
        DragAndDrop();
    }

    private void DragAndDrop()
    {
        if (Input.touchCount > 0) // if more than zero fingers is touching the screen
        {
            Debug.Log("I GOT TOUCHED");
            Touch touch = Input.GetTouch(0); // Gets info on the first finger that touches the screen (struct variable)

            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, zDepth));

            // TouchPhase.Began is the first frame that the finger touches the screen. Here we can calculate some values
            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("TouchPhase.Began");
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && hit.transform == transform)
                {
                    Debug.Log("Raycast stuff");
                    isDragging = true;
                    offset = transform.position - touchPosition;
                }
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Debug.Log(touchPosition);
                transform.position = new Vector3(touchPosition.x + offset.x, touchPosition.y + offset.y, touchPosition.z + offset.z);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
    }
}
