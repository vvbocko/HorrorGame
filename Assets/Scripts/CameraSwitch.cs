using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class CameraSwitch : MonoBehaviour
{
    public Camera playerCamera;
    public Camera lampCamera;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform monsterSpawnPoint;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private MonsterAI monster;
    [SerializeField] private RepairPoints pointBar;
    [SerializeField] private TMP_Text clickToEnter;
    [SerializeField] private TMP_Text clickToExit;
    [SerializeField] private TMP_Text holdF;
    [SerializeField] private TMP_Text spamQ;

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
        SwitchToPlayerView();
        clickToExit.enabled = false;
        clickToEnter.enabled = false;
        holdF.enabled = false;
        spamQ.enabled = false;
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

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isBoxViewActive)
                {
                    clickToExit.enabled = true;
                    clickToEnter.enabled = false;
                    spamQ.enabled = true;
                    holdF.enabled = false;

                    StartElectricBoxView();
                    isBoxViewActive = true;
                    pointBar.SetVisibility(true);
                    
                }
                else
                {
                    clickToExit.enabled = false;
                    clickToEnter.enabled = true;
                    spamQ.enabled = false;
                    holdF.enabled = true;

                    isBoxViewActive = false;
                    pointBar.SetVisibility(false);
                }
            }
            if (isRotating)
            {
                RotateTowardsTarget();
            }

            if (Input.GetKeyDown(KeyCode.Q) && isBoxViewActive)
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
            clickToEnter.enabled = true;
            holdF.enabled = true;

            SwitchToLampView();
            monsterMiniGame.StartMiniGame();
        }
    }

    public void SwitchToLampView()
    {
        player.enabled = false;
        playerCamera.enabled = false;
        lampCamera.enabled = true;
        isLampViewActive = true;

        flashlightMiniGame.gameObject.SetActive(true);

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

        flashlightMiniGame.gameObject.SetActive(false);

        targetRotation = lampCamera.transform.rotation;
        pointBar.SetVisibility(false);

        clickToExit.enabled = false;
        spamQ.enabled = false;
    }

    private void StartElectricBoxView()
    {
        float currentYRotation = transform.eulerAngles.y;
        targetRotation = Quaternion.Euler(0f, currentYRotation - 360f, 0f);
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

    public void SetMonsterPosition()
    {
        monster.transform.position = monsterSpawnPoint.position;
        monster.transform.rotation = Quaternion.Euler(0f, 180f, 0f);     
    }
}
