
using UnityEngine;

public class OffSetFlashlight : MonoBehaviour
{
    [SerializeField] Vector3 vectorOffSet;
    [SerializeField] GameObject followCameraX; // kamera followuje latarke
    [SerializeField] float followSpeed = 1f;
    void Start()
    {
        vectorOffSet = transform.position - followCameraX.transform.position;
    }
    void Update()
    {
        transform.position = followCameraX.transform.position + vectorOffSet;
        transform.rotation = Quaternion.Slerp(transform.rotation, followCameraX.transform.rotation, followSpeed * Time.deltaTime);
    }
}
