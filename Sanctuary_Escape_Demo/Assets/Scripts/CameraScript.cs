using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform cameraPosition;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void Update()
    {
        transform.position = cameraPosition.position;
    }

    public void DeathCam()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation * Quaternion.Euler(0f, 0f, -30f);
        transform.position += new Vector3(0f, -1f, 0f);
    }
}
