using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private WebCamTexture webcam;
    [SerializeField] private RawImage image;

    // Start is called before the first frame update
    void Start()
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
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("space!");
            TakePicture();
        }
    }

}
