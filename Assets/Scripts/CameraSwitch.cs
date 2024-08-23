using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera playerCamera;
    public Camera lampCamera;

    [SerializeField] private bool isLampViewActive = false;
    [SerializeField] private float maxRotation = 90f;
    [SerializeField] private float mouseSensitivity = 120f;

    private float initialYRotation;
    private float currentRotation = 0f;

    void Start()
    {
        SwitchToPlayerView();
    }

    void Update()
    {
        if (isLampViewActive)
        {
            HandleLampRotation();
        }
    }
    public void SwitchCamera()
    {
        if (isLampViewActive)
        {
            SwitchToPlayerView();
        }
        else
        {
            SwitchToLampView();
        }
    }
    public void SwitchToLampView()
    {
        playerCamera.enabled = false;
        lampCamera.enabled = true;
        isLampViewActive = true;

        initialYRotation = lampCamera.transform.localEulerAngles.y;
        currentRotation = 0f;
    }

    public void SwitchToPlayerView()
    {
        lampCamera.enabled = false;
        playerCamera.enabled = true;
        isLampViewActive = false;
    }

    private void HandleLampRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
        lampCamera.transform.Rotate(Vector3.up * mouseX);

        Vector3 rotation = lampCamera.transform.eulerAngles;
        rotation.z = 0f; 
        rotation.x = 0f;
        lampCamera.transform.eulerAngles = rotation;

    }

}
