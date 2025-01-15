using UnityEngine;

public class OffSetFlashlight : MonoBehaviour
{
    [SerializeField] private GameObject followFlashlight; // The flashlight the camera will follow
    [SerializeField] private Vector3 offset;  // Adjust this in the inspector to fine-tune the camera's relative position
    [SerializeField] private float followSpeed = 5f; // Speed at which the camera follows the flashlight

    private void Start()
    {
        // Initialize the offset as the current relative position between the camera and the flashlight
        offset = transform.position - followFlashlight.transform.position;
    }

    private void Update()
    {
        // Dynamically update the camera position by calculating the flashlight's position + offset
        Vector3 targetPosition = followFlashlight.transform.position + offset;

        // Smoothly follow the flashlight's position
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Smoothly follow the flashlight's rotation (if necessary)
        Quaternion targetRotation = Quaternion.LookRotation(followFlashlight.transform.forward, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, followSpeed * Time.deltaTime);
    }
}
