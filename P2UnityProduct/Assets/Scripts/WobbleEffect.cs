using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobbleEffect : MonoBehaviour {

    [Header("Wobble Settings")]
    public float wobbleSpeed = 2f;       // How fast the wobble oscillates
    public float wobbleAmount = 2f;      // Max rotation angle in degrees

    private Quaternion initialRotation;
    private float timeOffset;

    private void Start(){
        initialRotation = transform.localRotation;
        timeOffset = Random.Range(0f, 100f); // So not all objects wobble in sync
    }

    private void Update(){
        float wobble = Mathf.Sin(Time.time * wobbleSpeed + timeOffset) * wobbleAmount;
        transform.localRotation = initialRotation * Quaternion.AngleAxis(wobble, Vector3.forward);
    }
}
