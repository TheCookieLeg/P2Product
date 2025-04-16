using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Dragging : MonoBehaviour {

    [Header("Dragging variables")]
    private Vector3 offset; // offset between the touch point and the center of the object
    public bool isDragging = false;
    private float zDepth;

    [Header("Rotation Variables")]
    private Vector2 oldPos;
    private Vector2 newPos;

    [SerializeField] private bool interactable = false;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody2D rb;
    private Vector2 targetRbPosition;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();

        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
        interactable = true;

        zDepth = Camera.main.WorldToScreenPoint(transform.position).z;
    }

  private void Start(){
        GameManager.Instance.OnEnterLevel += GameManager_OnEnterLevel;
        GameManager.Instance.OnExitToGameScene += GameManager_OnExitToGameScene;
    }

    private void GameManager_OnEnterLevel(object sender, EventArgs e){
        interactable = true;

        transform.localPosition = startPosition;
        transform.localRotation = startRotation;
    }

    private void GameManager_OnExitToGameScene(object sender, EventArgs e){
        interactable = false;
        isDragging = false;
    }

    private void OnDestroy(){
        GameManager.Instance.OnEnterLevel -= GameManager_OnEnterLevel;
        GameManager.Instance.OnExitToGameScene -= GameManager_OnExitToGameScene;
    }

    private void Update()
    {
        if (!interactable) return;

        DragAndDrop();
        RotateObject();
    }

    private void FixedUpdate()
    {
        if (isDragging)
        {
            rb.MovePosition(targetRbPosition);
        }
    }

    private void DragAndDrop()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, zDepth));

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(touch.position);
                RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

                if (hit.collider != null && hit.transform == transform)
                {
                    isDragging = true;
                    offset = transform.position - (Vector3)worldPos;
                }
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                targetRbPosition = touchPosition + offset;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

                if (hit.collider != null && hit.transform == transform)
                {
                    isDragging = true;
                    offset = transform.position - (Vector3)worldPos;
                }
            }
            else if (Input.GetMouseButton(0) && isDragging)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDepth));
                Vector3 targetPos = mousePos + offset;
                targetRbPosition = targetPos;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
        }
    }

    private void RotateObject()
    {
        if (isDragging)
        {
            Vector2 movement = targetRbPosition - rb.position;

            if (movement.sqrMagnitude > 0.001f)
            {
                float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);
            }
        }
    }
}
