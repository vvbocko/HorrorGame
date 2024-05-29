using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private GameObject cam;
    [SerializeField] private CharacterController charController;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float  maxRotation = 80f;
    float moveX;
    float moveZ;
    bool cursorIsLocked = true;
    bool lockCursor = true;
    float camRotX = 0;

    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        HandleMovement();
        RotationHandler();
        if (lockCursor) CursorLock();
    }
    private void HandleMovement()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical") ;
        //transform.position = cam.transform.forward * moveZ + cam.transform.right * moveX;
        Vector3 movement = transform.forward * moveZ + transform.right * moveX;
        Vector3 newDirection = new Vector3(movement.x, 0, movement.z);
        newDirection = Vector3.Normalize(newDirection);
        rigidBody.MovePosition(rigidBody.position + newDirection * speed * Time.deltaTime);
    }
    private void RotationHandler()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        transform.Rotate(Vector3.up * mouseX);
        cam.transform.Rotate(Vector3.left * mouseY);
        camRotX -= mouseY;
        camRotX = Mathf.Clamp(camRotX, -maxRotation, maxRotation);
        cam.transform.localRotation = Quaternion.Euler(camRotX, 0f, 0f);
        
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