using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RepairPoints : MonoBehaviour
{
    [SerializeField] private CameraSwitch cameraSwitch;
    [SerializeField] private MonsterMiniGame monsterMiniGame;
    [SerializeField] private MonsterAI monsterAI;
    [SerializeField] private Light lampLight;
    [SerializeField] private Slider pointSlider;
    [SerializeField] private NavMeshObstacle navMeshObstacle;
    [SerializeField] Transform[] spawnPoints;

    private int maxPoints = 100;
    private int currentPoints = 0;
    private int pointsPerClick = 10;

    void Start()
    {
        cameraSwitch = FindObjectOfType<CameraSwitch>();
        monsterMiniGame = FindObjectOfType<MonsterMiniGame>();
        monsterAI = FindObjectOfType<MonsterAI>();

        lampLight.enabled = false;
        navMeshObstacle.enabled = false;
        SetVisibility(false);

        pointSlider.maxValue = maxPoints;
        pointSlider.value = currentPoints;

    }

    public void AddPoints()
    {
        currentPoints += pointsPerClick;

        if (currentPoints >= maxPoints)
        {
            currentPoints = maxPoints;
            lampLight.enabled = true;
            cameraSwitch.navMeshObstacle.enabled = true;
            cameraSwitch.interactableObject.enabled = false;
            cameraSwitch.SwitchToPlayerView();
            cameraSwitch.SetPlayerPosition();

            TeleportMonsterAI();
            //Debug.Log("Play a short cutscene later");
        }

        pointSlider.value = currentPoints;
    }
    private void TeleportMonsterAI()
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