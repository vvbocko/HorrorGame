using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private GameObject cam;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float mouseSensitivity = 100f;
    float moveX;
    float moveZ;
    bool cursorIsLocked = true;
    bool lockCursor = true;

    void Start()
    {
        
    }
    private void Update()
    {
        HandleMovement();
        RotationHandler();
        if (lockCursor) CursorLock();
    }
    private void HandleMovement()
    {
        moveX = Input.GetAxis("Horizontal") * speed;
        moveZ = Input.GetAxis("Vertical") * speed;
        transform.position += cam.transform.forward * moveZ + cam.transform.right * moveX;

    }
    private void RotationHandler()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
        cam.transform.Rotate(Vector3.left * mouseY);
    }
    private void CursorLock()
    {

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            cursorIsLocked = false;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            cursorIsLocked = true;
        }

        if (cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}