using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RepairPoints : MonoBehaviour
{
    [SerializeField] private CameraSwitch cameraSwitch;
    [SerializeField] private InteractableObject interactableObject;
    [SerializeField] private MonsterMiniGame monsterMiniGame;
    [SerializeField] private MonsterAI monsterAI;
    [SerializeField] public Light lampLight;
    [SerializeField] private Slider pointSlider;
    [SerializeField] private NavMeshObstacle navMeshObstacle;
    [SerializeField] Transform[] spawnPoints;

    [SerializeField] private float pointDropSpeed = 6f;

    public float maxPoints = 100f;
    public float currentPoints = 0f;
    private float pointsPerClick = 5f;

    void Start()
    {

        lampLight.enabled = false;
        navMeshObstacle.enabled = false;
        SetVisibility(false);

        pointSlider.maxValue = maxPoints;
        pointSlider.value = currentPoints;
        

    }
    private void Update()
    {
        PointReset();
    }
    private void PointReset()
    {
        if(currentPoints > 0 && currentPoints != maxPoints)
        {
            pointSlider.value -= pointDropSpeed * Time.deltaTime;
        }
    }
    public void AddPoints()
    {
        bool isClicked = true;
        currentPoints += pointsPerClick;

        if (currentPoints >= maxPoints)
        {
            currentPoints = maxPoints;
            lampLight.enabled = true;
            cameraSwitch.navMeshObstacle.enabled = true;
            cameraSwitch.interactableObject.enabled = false;
            interactableObject.isInteractable = false;

            cameraSwitch.SwitchToPlayerView();
            cameraSwitch.SetPlayerPosition();
            monsterMiniGame.StopMiniGame();

            TeleportMonsterAI();
        }

        pointSlider.value = currentPoints;
    }
    public void TeleportMonsterAI()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Vector3 randomTeleportPoint = spawnPoints[randomIndex].position;

        monsterAI.transform.position = randomTeleportPoint;

        NavMeshAgent monsterNavMeshAgent = monsterAI.GetComponent<NavMeshAgent>();
        if (monsterNavMeshAgent != null)
        {
            monsterNavMeshAgent.ResetPath();
        }
    }
    public void SetVisibility(bool isVisible)
    {
        CanvasGroup canvasGroup = pointSlider.GetComponentInParent<CanvasGroup>();

        if (canvasGroup != null)
        {
            canvasGroup.alpha = isVisible ? 1 : 0; // is visible? if so 1 if not 0
            canvasGroup.interactable = isVisible;
            canvasGroup.blocksRaycasts = isVisible;
        }
    }
}