using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] Jumpscare jumpscare;

    [Header("Camera Rotation")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float maxRotation = 80f;
    bool isJumpscareActive = false;

    private float camRotX = 0f;

    void Start()
    {
        CursorLock();
    }

    void Update()
    {
        if (!isJumpscareActive)
        {
            RotationHandler();
        }
        else
        {
            jumpscare.LookAtMonsterHead();
        }
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

        playerBody.Rotate(Vector3.up * mouseX);// obracamy graczem odpowiedzialne za obracanie na lewo i prawo gracza
        transform.Rotate(Vector3.left * mouseY); // obraca sie latarka w góre i w dó³
        
        camRotX -= mouseY;
        camRotX = Mathf.Clamp(camRotX, -maxRotation, maxRotation);
        transform.localRotation = Quaternion.Euler(camRotX, 0f, 0f);

    }

    private void CursorLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void ActivateJumpscare()
    {
        isJumpscareActive = true;
    }
}
