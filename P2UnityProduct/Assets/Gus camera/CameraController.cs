using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class CameraController : MonoBehaviour
{
    // ok, so I have pasted a shit ton of android permision stuff... Do I know how it works? NO!
        private void AskCameraPermission()
    {
        var callbacks = new PermissionCallbacks();
        callbacks.PermissionDenied += PermissionCallbacksPermissionDenied;
        callbacks.PermissionGranted += PermissionCallbacksPermissionGranted;
        Permission.RequestUserPermission(Permission.Camera, callbacks);
    }
    private void PermissionCallbacksPermissionGranted(string permissionName)
    {
        StartCoroutine(DelayedCameraInitialization());
    }

    private void PermissionCallbacksPermissionDenied(string permissionName)
    {
        Debug.LogWarning($"Permission {permissionName} Denied");
    }


    private IEnumerator DelayedCameraInitialization()
    {
        yield return null;
        InitializeCamera();
    }

    void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            AskCameraPermission();
        }

        InitializeCamera();
    }

    private WebCamTexture webcam;
    [SerializeField] private RawImage image;
    void InitializeCamera()
    {
        //finds a camera and shows the output as a texture
        webcam = new WebCamTexture();
        image.texture = webcam;
        webcam.Play();
    }

    public void TakePicture() {
        Texture2D photo = new Texture2D(webcam.width, webcam.height);
        photo.SetPixels(webcam.GetPixels());
        photo.Apply();

        // This should save the picture on the device 
        byte[] bytes = photo.EncodeToPNG();
        string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";

        File.WriteAllBytes(Application.persistentDataPath + fileName, bytes);
        Debug.Log("Saved: " + Application.persistentDataPath + fileName);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.touchCount == 1) {
            Debug.Log("space!");
            TakePicture();
        }
    }

}
