using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMiniGame : MonoBehaviour
{
    [SerializeField] private MonsterMiniGameAI minigameMonsterAI;
    [SerializeField] private MonsterAI monsterAI;
    [SerializeField] private CameraSwitch cameraSwitch;
    [SerializeField] private Animator animator;
    [SerializeField] private RepairPoints repairPoints;

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform player;
    [SerializeField] private Transform monster;

    [SerializeField] private float retreatTime = 2f;
    [SerializeField] private float attackRange = 1.2f;

    private bool isScared = false;
    private bool isMinigameStarted = false;
    private bool isMiniGameCompleted = false;
    private float distanceToPlayer = Mathf.Infinity;

    void Start()
    {
        minigameMonsterAI.gameObject.SetActive(false);
        monsterAI.gameObject.SetActive(true);
    }
    void Update()
    {
        if (!isScared && isMinigameStarted)
        {
            ChasePlayer();
            CheckAttackRange();
        }
    }

    void ChasePlayer()
    {
        minigameMonsterAI.MoveToPoint(player.position);
    }
    void CheckAttackRange()
    {
        Vector3 flatMonsterPosition = new Vector3(monster.position.x, 0, monster.position.z);
        Vector3 flatPlayerPosition = new Vector3(player.position.x, 0, player.position.z);
        distanceToPlayer = Vector3.Distance(flatMonsterPosition, flatPlayerPosition);

        if (distanceToPlayer <= attackRange)
        {//StopMiniGame() without retreat + Repairpoints
            cameraSwitch.SwitchToPlayerView();
            cameraSwitch.SetPlayerPosition();
            
            isMinigameStarted = false;
            isMiniGameCompleted = true;

            minigameMonsterAI.gameObject.SetActive(false);
            monsterAI.gameObject.SetActive(true);
            cameraSwitch.SetMonsterPosition();

            GameManager.Instance.CompleteOneMiniGame();
        }
    }

    public void ScareMonster()
    {
        if (!isScared)
        {
            isScared = true;
            Debug.Log("Monster is retreating");
            
            animator.SetTrigger("damage");
            minigameMonsterAI.SetIsStopped(true);

            StartCoroutine(RetreatAndTeleport());
        }
    }

    IEnumerator RetreatAndTeleport()
    {
        yield return new WaitForSeconds(retreatTime);

        TeleportToRandomPoint();

        Debug.Log("Monster teleported");

        yield return new WaitForSeconds(0.5f);
        minigameMonsterAI.SetIsStopped(false);
        isScared = false;
    }
    public void TeleportToRandomPoint()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        minigameMonsterAI.Teleport(spawnPoints[randomIndex].position);
    }

    public void StartMiniGame()
    {
        if (isMiniGameCompleted)
        { 
            return;
        }
        isMinigameStarted = true;

        minigameMonsterAI.gameObject.SetActive(true);
        monsterAI.gameObject.SetActive(false);

        TeleportToRandomPoint();
    }
    public void StopMiniGame()
    {
        isMinigameStarted = false;
        isMiniGameCompleted = true;

        minigameMonsterAI.gameObject.SetActive(false);
        monsterAI.gameObject.SetActive(true);

        monsterAI.Retreat();

        GameManager.Instance.CompleteOneMiniGame();
    }

}
