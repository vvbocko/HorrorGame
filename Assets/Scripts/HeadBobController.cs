using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    [SerializeField] private bool enable = true;

    [SerializeField] private float amplitude;
    [SerializeField, Range(0f, 0.1f)] private float walkAmplitude = 0.0031f;
    [SerializeField, Range(0f, 0.1f)] private float sprintAmplitude = 0.025f;

    [SerializeField] private float frequency;
    [SerializeField, Range(0f, 30f)] private float walkFrequency = 10.9f;
    [SerializeField, Range(0f, 30f)] private float sprintFrequency = 17.5f;

    [SerializeField] private Transform playerCamera = null;
    [SerializeField] private Transform cameraHolder = null;

    private float toggleSpeed = 0.5f;
    private Vector3 startPosition;
    private Rigidbody rigidBody;

    void Start()
    {
        amplitude = walkAmplitude;
        frequency = walkFrequency;
        rigidBody = GetComponent<Rigidbody>();
        startPosition = playerCamera.localPosition;
    }

    void Update()
    {
        if (!enable) return;

        CheckMotion();
        RestartPosition();
        playerCamera.LookAt(FocusTarget());
    }

    private void CheckMotion()
    {
        float speed = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z).magnitude;

        if (speed >= toggleSpeed)
        {
            PlayMotion(FootstepMotion());
        }
    }

    private void PlayMotion(Vector3 motion)
    {
        playerCamera.localPosition += motion;
    }

    private Vector3 FootstepMotion()
    {
        Vector3 position = Vector3.zero;
        float time = Time.time * frequency;

        position.x += Mathf.Cos(time * 0.5f) * amplitude * 0.5f;
        position.y += Mathf.Sin(time) * amplitude;
        position.z += Mathf.Sin(time * 0.5f) * amplitude * 0.2f;

        return position;
    }

    private void RestartPosition()
    {
        if (Vector3.Distance(playerCamera.localPosition, startPosition) > 0.001f)
        {
            playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, startPosition, 1f * Time.deltaTime);
        }
    }

    private Vector3 FocusTarget()
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y + cameraHolder.localPosition.y, transform.position.z);
        position += cameraHolder.forward * 10f;
        return position;
    }

    public void IncreaseHeadBob()
    {
        amplitude = sprintAmplitude;
        frequency = sprintFrequency;
    }
    public void RestartHeadBob()
    {
        amplitude = walkAmplitude;
        frequency = walkFrequency;
    }
}
