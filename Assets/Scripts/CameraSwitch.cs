using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class CameraSwitch : MonoBehaviour
{
    public Camera playerCamera;
    public Camera lampCamera;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private RepairPoints pointBar;

    public NavMeshObstacle navMeshObstacle;
    public InteractableObject interactableObject;

    [SerializeField] private FlashlightMiniGame flashlightMiniGame;
    [SerializeField] private MonsterMiniGame monsterMiniGame;

    [SerializeField] private float mouseSensitivity = 120f;
    [SerializeField] private float rotationTime = 0.1f;

    [SerializeField] private bool isLampViewActive = false;
    [SerializeField] private bool isBoxViewActive = false;

    private float initialYRotation;
    private float currentRotation = 0f;
    private float rotationProgress = 0f;

    private Quaternion targetRotation;
    private bool isRotating = false;


    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        pointBar = FindObjectOfType<RepairPoints>();
        flashlightMiniGame = FindObjectOfType<FlashlightMiniGame>();
        monsterMiniGame = FindObjectOfType<MonsterMiniGame>();
        navMeshObstacle = FindObjectOfType<NavMeshObstacle>();
        interactableObject = FindObjectOfType<InteractableObject>();

        SwitchToPlayerView();
    }

    void Update()
    {
        if (isLampViewActive)
        {
            player.transform.position = new Vector3(73f, -5f, -22f);

            if (!isBoxViewActive)
            {
                HandleLampRotation();
            }

            //if (Input.GetKeyDown(KeyCode.Q))
            //{
            //    SetPlayerPosition();
            //    SwitchToPlayerView();
            //}

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isBoxViewActive)
                {
                    StartElectricBoxView();
                    isBoxViewActive = true;
                    pointBar.SetVisibility(true);
                }
                else
                {
                    isBoxViewActive = false;
                    pointBar.SetVisibility(false);
                }
            }
            if (isRotating)
            {
                RotateTowardsTarget();
            }

            if (Input.GetKeyDown(KeyCode.Space) && isBoxViewActive)
            {
                pointBar.AddPoints();
            }
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
        player.enabled = false;
        playerCamera.enabled = false;
        lampCamera.enabled = true;
        isLampViewActive = true;
        

        flashlightMiniGame.gameObject.SetActive(true);
        monsterMiniGame.StartMiniGame();
        //monsterMiniGame.gameObject.SetActive(true);

        initialYRotation = lampCamera.transform.localEulerAngles.y;
        currentRotation = 0f;
        targetRotation = Quaternion.Euler(0f, -180f, 0f);
    }

    public void SwitchToPlayerView()
    {
        player.enabled = true;
        lampCamera.enabled = false;
        playerCamera.enabled = true;
        isLampViewActive = false;
        isBoxViewActive = false;

        monsterMiniGame.StopMiniGame();
        flashlightMiniGame.gameObject.SetActive(false);
        //monsterMiniGame.gameObject.SetActive(false);

        targetRotation = lampCamera.transform.rotation;
        pointBar.SetVisibility(false);
    }

    private void StartElectricBoxView()
    {
        targetRotation = Quaternion.Euler(0f, -180f, 0f);
        isRotating = true;
        rotationProgress = 0f;
    }

    private void RotateTowardsTarget()
    {
        rotationProgress += Time.deltaTime / rotationTime;

        lampCamera.transform.rotation = Quaternion.Lerp(lampCamera.transform.rotation, targetRotation, rotationProgress);

        if (rotationProgress >= 0.5f || rotationProgress >= (0.5f / rotationTime))
        {
            isRotating = false;
            lampCamera.transform.rotation = targetRotation;
        }
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
    
    public void SetPlayerPosition()
    {
        player.transform.position = playerSpawnPoint.position;
        player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
