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
    // [SerializeField] private UnityEngine.Vector2 picturePreviewSize = new UnityEngine.Vector2(3,4) * 50;
    [SerializeField] private GameObject takePictureButton;
    [SerializeField] private GameObject confirmOrRetakeButtons;
    
    private void AskCameraPermission()
    {
        PermissionCallbacks callbacks = new PermissionCallbacks();
        callbacks.PermissionDenied += PermissionCallbacksPermissionDenied;
        callbacks.PermissionGranted += PermissionCallbacksPermissionGranted;
        Permission.RequestUserPermission(Permission.Camera, callbacks);
    }
    private void PermissionCallbacksPermissionGranted(string permissionName)
    {
        CameraInitialization();
        
        Debug.Log($"Permission {permissionName} Granted :)");
    }

    private void PermissionCallbacksPermissionDenied(string permissionName)
    {
        Debug.LogWarning($"Permission {permissionName} Denied");
    }

    private void CameraInitialization() {
        Debug.Log("waiting for next frame...");
        //yield return null;

        Debug.Log("Initializing Camera...");
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

<<<<<<< HEAD
    void OnDisable() {
        StopCamera();
    }
    
    public void StartCamera() {
        if (showingPicture) return;
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera)) {
                AskCameraPermission();
            }
=======
    void Start()
    {
        Debug.Log("script has started...");
        //checks if we have permission for camera and storage, and asks for them if we don't
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            
            Debug.Log("asking for camera permission...");
            AskCameraPermission();
        }
        
        Debug.Log("we have camera permissions...");
            // The following code is only relevant if we  decide to save pictures on users devices
        // if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        // {
        //     AskStorageWritePermission();
        // }
        // if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        // {
        //     AskStorageReadPermission();
        // }
>>>>>>> a186cdc36535b178edc8f718b87d8497c5dba9d3
        CameraInitialization();
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

    private Texture2D photo = null;
    public void TakePicture() {
        if (webcam == null) {
            return;
        }
<<<<<<< HEAD
        // take a "screenshot" by storing the cameras pixels as a texture
        photo = new Texture2D(webcam.width, webcam.height);
=======
        Texture2D photo = new Texture2D(webcam.width, webcam.height);
>>>>>>> a186cdc36535b178edc8f718b87d8497c5dba9d3
        photo.SetPixels(webcam.GetPixels());
        photo.Apply();

        OpenPictureOverlay();
    }
    private bool showingPicture = false;
    private void OpenPictureOverlay() {
        showingPicture = true;
        
        takePictureButton.SetActive(false);
        confirmOrRetakeButtons.SetActive(true);

        FormatCameraTexture(webcam, pictureTakenDisplay, cameraPreviewSize);
        pictureTakenDisplay.texture = photo;
<<<<<<< HEAD
        pictureTakenDisplay.gameObject.SetActive(true);
        StopCamera();
    }

    public void RetakePicture() {
        showingPicture = false;
        takePictureButton.SetActive(true);
        confirmOrRetakeButtons.SetActive(false);

        pictureTakenDisplay.gameObject.SetActive(false);
        StartCamera();
=======

            // The following code is only relevant if we later descide to save pictures on users devices
        // if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        // {
        //     // This should save the picture on the device
        //     byte[] bytes = photo.EncodeToPNG();
        //     string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";

        //     File.WriteAllBytes(Application.persistentDataPath + fileName, bytes);
        //     Debug.Log("Saved: " + Application.persistentDataPath + fileName);
        // }
>>>>>>> a186cdc36535b178edc8f718b87d8497c5dba9d3
    }

    void Update()
    {   
        if (webcam != null) {
            FormatCameraTexture(webcam, image, cameraPreviewSize);
        }
    }
}
