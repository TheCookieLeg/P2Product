using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    WebCamTexture webcam;

    [SerializeField] RawImage image;

    // Start is called before the first frame update
    void Start()
    {
        webcam = new WebCamTexture();
        image.texture = webcam;
        webcam.Play();
    }

}
