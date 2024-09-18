using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMiniGame : MonoBehaviour
{
    [SerializeField] MonsterMiniGameAI minigameMonsterAI;
    [SerializeField] MonsterAI monsterAI;

    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Transform player;
    [SerializeField] float retreatTime = 2f;

    private bool isScared = false;
    private bool minigameStarted = false;


    void Update()
    {
        if (!isScared && minigameStarted)
        {
            ChasePlayer();
        }
    }

    void ChasePlayer() //
    {
        minigameMonsterAI.MoveToPoint(player.position);
    }

    public void ScareMonster()
    {
        if (!isScared)
        {
            isScared = true;
            Debug.Log("Monster is retreating");

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
        minigameStarted = true;
        minigameMonsterAI.gameObject.SetActive(true);
        monsterAI.gameObject.SetActive(false);
        TeleportToRandomPoint();
    }
    public void StopMiniGame()
    {
        minigameStarted = false;
        minigameMonsterAI.gameObject.SetActive(false);
        monsterAI.gameObject.SetActive(true);
        monsterAI.Retreat();
    }
}