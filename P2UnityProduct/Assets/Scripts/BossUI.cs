
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class BossUI : MonoBehaviour {

    [SerializeField] private Button backButton;
    [SerializeField] private Button confirmButton;

    [SerializeField] private Button takePictureButton;
    [SerializeField] private GameObject pictureButtonsParent;
    [SerializeField] private Button retakePictureButton;
    [SerializeField] private RawImage image; // display area for camera output
    [SerializeField] private RawImage pictureTakenDisplay; // display area for picture taken
    [SerializeField] private Vector2 cameraPreviewSize = new Vector2(3,4) * 300;
    [SerializeField] private Button openCameraScreenButton;
    [SerializeField] private GameObject cameraScreen;

    private WebCamTexture webcam = null;
    private bool showingPicture = false;
    
    private Animator anim;
    private Texture2D photo = null;

    private void Awake(){
        anim = GetComponent<Animator>();

        backButton.onClick.AddListener(() => {
            GameManager.Instance.BackToGameScene();
            StopCamera();
            anim.SetTrigger("End");
            Invoke("Hide", 0.5f);
        });
        confirmButton.onClick.AddListener(() => {
            GameManager.Instance.BackToGameScene();
            StopCamera();
            anim.SetTrigger("End");
            Invoke("Hide", 0.5f);
        });
        takePictureButton.onClick.AddListener(() => {
            TakePicture();
        });
        retakePictureButton.onClick.AddListener(() => {
            RetakePicture();
        // enables camera page/screen
        });
        openCameraScreenButton.onClick.AddListener(() => {
            cameraScreen.SetActive(true); 
            StartCamera();   
        });
    }

    private void Start(){
        GameManager.Instance.OnEnterLevel += GameManager_OnEnterLevel;
        Hide();
    }

    private void OnDestroy(){
        GameManager.Instance.OnEnterLevel -= GameManager_OnEnterLevel;
    }

    private void Update(){   
        if (webcam != null) {
            FormatCameraTexture(webcam, image, cameraPreviewSize);
        }
    }

    private void GameManager_OnEnterLevel(object sender, EventArgs e){
        if (GameManager.Instance.currentLevelData is not BossLevelSO) return;

        Show();

        cameraScreen.SetActive(false);
    }

    private void StartCamera() {
        if (showingPicture) return;
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera)) {
                AskCameraPermission();
            }
        CameraInitialization();
    }

    private void StopCamera(){
        if (webcam == null) return;

        if (webcam.isPlaying){
            webcam.Stop();
            image.texture = null;
            webcam = null;
        }    
    }

    private void CameraInitialization() {
        webcam = new WebCamTexture();
        image.texture = webcam;

        webcam.Play();
    }

    public void TakePicture() {
        if (webcam == null) {
            return;
        }
        // take a "screenshot" by storing the cameras pixels as a texture
        photo = new Texture2D(webcam.width, webcam.height);
        photo.SetPixels(webcam.GetPixels());
        photo.Apply();

        OpenPictureOverlay();
    }

    private void OpenPictureOverlay() {
        showingPicture = true;
        
        takePictureButton.gameObject.SetActive(false);
        pictureButtonsParent.SetActive(true);

        FormatCameraTexture(webcam, pictureTakenDisplay, cameraPreviewSize);
        pictureTakenDisplay.texture = photo;
        pictureTakenDisplay.gameObject.SetActive(true);
        StopCamera();
    }

    private void FormatCameraTexture(WebCamTexture webcam, RawImage image, Vector2 size) {
        // Rotate image to show correct orientation 
        Vector3 rotationVector = new Vector3(0f, 0f, 0f);
        rotationVector.z = -webcam.videoRotationAngle;
        image.rectTransform.localEulerAngles = rotationVector;

        // resize image are to fit new rotation (rotate 2d vector formula)
        Vector2 newSize = new Vector2(
            Mathf.Cos(Mathf.Deg2Rad * rotationVector.z) * size.x - Mathf.Sin(Mathf.Deg2Rad * rotationVector.z) * size.y,
            Mathf.Sin(Mathf.Deg2Rad * rotationVector.z) * size.x + Mathf.Cos(Mathf.Deg2Rad * rotationVector.z) * size.y); 
        image.rectTransform.sizeDelta = new Vector2(Mathf.Abs(newSize.x), Mathf.Abs(newSize.y));
    }

    public void RetakePicture() {
        showingPicture = false;
        takePictureButton.gameObject.SetActive(true);
        pictureButtonsParent.SetActive(false);

        pictureTakenDisplay.gameObject.SetActive(false);
        StartCamera();
    }

    private void AskCameraPermission(){
        //cool way of calling a method when permission granted or denied
        PermissionCallbacks callbacks = new PermissionCallbacks();
        callbacks.PermissionDenied += PermissionCallbacksPermissionDenied;
        callbacks.PermissionGranted += PermissionCallbacksPermissionGranted;
        Permission.RequestUserPermission(Permission.Camera, callbacks);
    }

    private void PermissionCallbacksPermissionGranted(string permissionName){
        StartCamera();

        Debug.Log($"Permission {permissionName} Granted :)");
    }

    private void PermissionCallbacksPermissionDenied(string permissionName){
        Debug.LogWarning($"Permission {permissionName} Denied");
        cameraScreen.SetActive(false);
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Show(){
        gameObject.SetActive(true);
    }
}
