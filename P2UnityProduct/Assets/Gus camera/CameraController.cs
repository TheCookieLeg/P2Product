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
    private WebCamTexture webcam = null;
    [SerializeField] private RawImage image;

    // Image rotation
    Vector3 rotationVector = new Vector3(0f, 0f, 0f);

        private void AskCameraPermission()
    {
        PermissionCallbacks callbacks = new PermissionCallbacks();
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
        
        webcam = new WebCamTexture();
        image.texture = webcam;

        webcam.Play();
    }


    private void AskStorageWritePermission()
    {
        var callbacks = new PermissionCallbacks();
        Permission.RequestUserPermission(Permission.ExternalStorageWrite, callbacks);
    }
    // private void AskStorageReadPermission()
    // {
    //     var callbacks = new PermissionCallbacks();
    //     Permission.RequestUserPermission(Permission.ExternalStorageRead, callbacks);
    // }

    void Start()
    {
        //this checks if we have permission for stuff like camera and storage, and asks for them if we don't
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            AskCameraPermission();
        }
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            AskStorageWritePermission();
        }
        // if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        // {
        //     AskStorageReadPermission();
        // }

        //InitializeCamera();
        StartCoroutine(DelayedCameraInitialization());
    }

    private void FormatCameraTexture(WebCamTexture webcam) {
        // Skip making adjustment for incorrect camera data
        // if (webcam.width < 100)
        // {
        //     Debug.Log("Still waiting another frame for correct info...");
        //     return;
        // }

        // Rotate image to show correct orientation 
        rotationVector.z = -webcam.videoRotationAngle;
        image.rectTransform.localEulerAngles = rotationVector;

        // if (webcam.width / webcam.height != 3/4) {
        //     Debug.Log("Camera ratio " + webcam.width + "/" + webcam.height + "doesn't fit design. Streched to fit.");
        // }
        image.rectTransform.sizeDelta = new Vector2(3,4) * 100;
        
    }

    public void TakePicture() {
        if (webcam == null) {
            return;
        }
        Texture2D photo = new Texture2D(webcam.width, webcam.height);
        photo.SetPixels(webcam.GetPixels());
        photo.Apply();

        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            // This should save the picture on the device 
            byte[] bytes = photo.EncodeToPNG();
            string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";

            File.WriteAllBytes(Application.persistentDataPath + fileName, bytes);
            Debug.Log("Saved: " + Application.persistentDataPath + fileName);
        }
    }

    void Update()
    {   
        if (webcam != null) {
            FormatCameraTexture(webcam);

            if (Input.GetKeyDown(KeyCode.Space) || Input.touchCount == 1) {
                Debug.Log("input rescieved!");
                TakePicture();
            }
        }
    }
}
