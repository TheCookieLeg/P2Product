using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using TMPro;
using System.Numerics;

public class CameraController : MonoBehaviour
{
    private WebCamTexture webcam = null;
    [SerializeField] private RawImage image;
    [SerializeField] private RawImage pictureTakenDisplay;    
    UnityEngine.Vector2 cameraPreviewSize = new UnityEngine.Vector2(3,4) * 300;
    UnityEngine.Vector2 picturePreviewSize = new UnityEngine.Vector2(3,4) * 50;


    // Image rotation
    UnityEngine.Vector3 rotationVector = new UnityEngine.Vector3(0f, 0f, 0f);

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


    // private void AskStorageWritePermission()
    // {
    //     var callbacks = new PermissionCallbacks();
    //     Permission.RequestUserPermission(Permission.ExternalStorageWrite, callbacks);
    // }
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
        // if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        // {
        //     AskStorageWritePermission();
        // }
        // if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        // {
        //     AskStorageReadPermission();
        // }

        //InitializeCamera();
        StartCoroutine(DelayedCameraInitialization());
    }
    private void FormatCameraTexture(WebCamTexture webcam, RawImage image, UnityEngine.Vector2 size) {

        // Rotate image to show correct orientation 
        rotationVector.z = -webcam.videoRotationAngle;
        image.rectTransform.localEulerAngles = rotationVector;

        UnityEngine.Vector2 newSize = new UnityEngine.Vector2(
            Mathf.Cos(Mathf.Deg2Rad * rotationVector.z) * size.x - Mathf.Sin(Mathf.Deg2Rad * rotationVector.z) * size.y,
            Mathf.Sin(Mathf.Deg2Rad * rotationVector.z) * size.x + Mathf.Cos(Mathf.Deg2Rad * rotationVector.z) * size.y); 
        
        // image.rectTransform.sizeDelta = Quaternion.AngleAxis(rotationVector.z, image.rectTransform.sizeDelta) * new Vector2(3,4) * 300;
        image.rectTransform.sizeDelta = new UnityEngine.Vector2(Mathf.Abs(newSize.x), Mathf.Abs(newSize.y));

        // debugText.SetText(
        //     "cameraPreviewSize: " + cameraPreviewSize + 
        //     " | image.rectTransform.localEulerAngles: " + image.rectTransform.localEulerAngles + 
        //     " | rotationVector: " + rotationVector +
        //     " | newCameraPreviewSize: " + newCameraPreviewSize);
    }

    public void TakePicture() {
        if (webcam == null) {
            return;
        }
        Texture2D photo = new Texture2D(webcam.width, webcam.height);
        photo.SetPixels(webcam.GetPixels());
        photo.Apply();

        FormatCameraTexture(webcam, pictureTakenDisplay, picturePreviewSize);
        pictureTakenDisplay.texture = photo;

        // if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        // {
        //     // This should save the picture on the device
        //     byte[] bytes = photo.EncodeToPNG();
        //     string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";

        //     File.WriteAllBytes(Application.persistentDataPath + fileName, bytes);
        //     Debug.Log("Saved: " + Application.persistentDataPath + fileName);
        // }
    }

    void Update()
    {   
        if (webcam != null) {
            FormatCameraTexture(webcam, image, cameraPreviewSize);
        }
    }
}
