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
    [SerializeField] private RawImage image; // display area for camera output
    [SerializeField] private RawImage pictureTakenDisplay; // display area for picture taken
    [SerializeField] private UnityEngine.Vector2 cameraPreviewSize = new UnityEngine.Vector2(3,4) * 300;
    [SerializeField] private UnityEngine.Vector2 picturePreviewSize = new UnityEngine.Vector2(3,4) * 50;
    
    private void AskCameraPermission()
    {
        PermissionCallbacks callbacks = new PermissionCallbacks();
        callbacks.PermissionDenied += PermissionCallbacksPermissionDenied;
        callbacks.PermissionGranted += PermissionCallbacksPermissionGranted;
        Permission.RequestUserPermission(Permission.Camera, callbacks);
    }
    private void PermissionCallbacksPermissionGranted(string permissionName)
    {
        StartCamera();

        Debug.Log($"Permission {permissionName} Granted :)");
    }

    private void PermissionCallbacksPermissionDenied(string permissionName)
    {
        Debug.LogWarning($"Permission {permissionName} Denied");
    }

    private void CameraInitialization() {
        webcam = new WebCamTexture();
        image.texture = webcam;

        webcam.Play();
    }
        // The following code is only relevant if we  decide to save pictures on users devices
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
    
    void OnEnable() {
        StartCamera();
    }

    void OnDisable() {
        StopCamera();
    }
    
    public void StartCamera() {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera)) {
                AskCameraPermission();
            }
        CameraInitialization();
    }

    public void StopCamera() {
        if (webcam != null || webcam.isPlaying) {
            webcam.Stop();
            image.texture = null;
            webcam = null;
        }
    }

    private void FormatCameraTexture(WebCamTexture webcam, RawImage image, UnityEngine.Vector2 size) {
        // Rotate image to show correct orientation 
        UnityEngine.Vector3 rotationVector = new UnityEngine.Vector3(0f, 0f, 0f);
        rotationVector.z = -webcam.videoRotationAngle;
        image.rectTransform.localEulerAngles = rotationVector;

        // resize image are to fit new rotation (rotate 2d vector formula)
        UnityEngine.Vector2 newSize = new UnityEngine.Vector2(
            Mathf.Cos(Mathf.Deg2Rad * rotationVector.z) * size.x - Mathf.Sin(Mathf.Deg2Rad * rotationVector.z) * size.y,
            Mathf.Sin(Mathf.Deg2Rad * rotationVector.z) * size.x + Mathf.Cos(Mathf.Deg2Rad * rotationVector.z) * size.y); 
        image.rectTransform.sizeDelta = new UnityEngine.Vector2(Mathf.Abs(newSize.x), Mathf.Abs(newSize.y));
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

            // The following code is only relevant if we later descide to save pictures on users devices
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
