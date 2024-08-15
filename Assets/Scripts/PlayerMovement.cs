using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private GameObject cam;

    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float sprintSpeed = 2.5f;
    
    [SerializeField] private float useSpeed = 20f;
    [SerializeField] private float regenerateSpeed = 10f;
    [SerializeField] private float maxSprintPoints = 50f;
    [SerializeField] private float currentSprintPoints;

    [SerializeField] private float cooldownDuration = 5f;
    private bool isCooldownActive = false;
    private float cooldownTimer = 0f;

    [Header("Camera Rotation")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float  maxRotation = 80f;

    float moveX;
    float moveZ;
    float camRotX = 0;
    bool cursorIsLocked = true;
    bool lockCursor = true;
    

    void Start()
    {
        currentSprintPoints = maxSprintPoints;
        rigidBody.freezeRotation = true;
        CursorLock();

    }

    private void Update()
    {
        RotationHandler();

        if (lockCursor)
        {
            CursorLock();
        }

        if (isCooldownActive)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isCooldownActive = false;
                currentSprintPoints = maxSprintPoints;
            }
        }
    }
    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float currentSpeed = walkSpeed;

        if (Input.GetKey(KeyCode.LeftShift) && currentSprintPoints > 0f && !isCooldownActive)
        {
            currentSpeed = sprintSpeed;
            currentSprintPoints -= Time.deltaTime * useSpeed;
        }
        else if (currentSprintPoints < maxSprintPoints && !isCooldownActive)
        {
            currentSprintPoints += Time.deltaTime * regenerateSpeed;
        }
        if (currentSprintPoints <= 0f && !isCooldownActive)
        {
            isCooldownActive = true;
            cooldownTimer = cooldownDuration;
        }
        currentSprintPoints = Mathf.Clamp(currentSprintPoints, 0, maxSprintPoints); 

        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical") ;

        //transform.position = cam.transform.forward * moveZ + cam.transform.right * moveX;
        Vector3 movement = transform.forward * moveZ + transform.right * moveX;
        Vector3 newDirection = new Vector3(movement.x, 0, movement.z);
        newDirection = Vector3.Normalize(newDirection);
        //rigidBody.MovePosition(rigidBody.position + newDirection * currentSpeed * Time.deltaTime);
        rigidBody.AddForce(newDirection * currentSpeed, ForceMode.VelocityChange );
        
        Vector3 flatVelocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
        if (flatVelocity.magnitude > currentSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * currentSpeed;
            rigidBody.velocity = new Vector3(limitedVelocity.x, rigidBody.velocity.y, limitedVelocity.z);
        }
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
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}