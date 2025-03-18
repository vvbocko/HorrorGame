using UnityEngine;

public class OffSetFlashlight : MonoBehaviour
{
    [SerializeField] private GameObject followFlashlight;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float followSpeed = 5f;

    private void Start()
    {
        offset = transform.position - followFlashlight.transform.position;
    }

    private void Update()
    {
        Vector3 targetPosition = followFlashlight.transform.position + offset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(followFlashlight.transform.forward, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, followSpeed * Time.deltaTime);
    }
}
