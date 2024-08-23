using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private Transform playerBody;

    [Header("Camera Rotation")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float maxRotation = 80f;

    private float camRotX = 0f;

    void Start()
    {
        CursorLock();
    }

    void Update()
    {
        RotationHandler();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            CursorLock();
        }
    }

    private void RotationHandler()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        playerBody.Rotate(Vector3.up * mouseX);

        transform.Rotate(Vector3.left * mouseY);
        camRotX -= mouseY;
        camRotX = Mathf.Clamp(camRotX, -maxRotation, maxRotation);
        transform.localRotation = Quaternion.Euler(camRotX, 0f, 0f);

    }

    private void CursorLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
