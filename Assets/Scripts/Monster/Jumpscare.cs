using System.Collections;
using UnityEditor;
using UnityEngine;

public class Jumpscare : MonoBehaviour
{
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] CameraRotation cameraRotation;
    [SerializeField] MonsterAI monsterAI;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] GameObject jumpscareMonster;
    [SerializeField] GameObject player;
    [SerializeField] Transform monster;
    [SerializeField] Transform playerCamera;
    [SerializeField] Animator animator;

    [SerializeField] Transform monsterHead;
    [SerializeField] Transform monsterHand;

    [SerializeField] float lookSpeed = 2f;
    [SerializeField] float jumpscareDistance = 5f;
    private bool isJumpscareActive = false;

    private void Update()
    {
        if (isJumpscareActive)
        {
            MoveCameraToHand();
        }
        else
        {
            CheckJumpscareTrigger();
        }
    }

    private void CheckJumpscareTrigger()
    {
        float distanceToMonster = Vector3.Distance(player.transform.position, monster.position);

        if (distanceToMonster <= jumpscareDistance)
        {
            PlayJumpscare();
        }
    }

    public void PlayJumpscare()
    {
        if (!isJumpscareActive)
        {
            isJumpscareActive = true;

            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }

            if (cameraRotation != null)
            {
                cameraRotation.ActivateJumpscare();
            }

            if (monsterAI != null)
            {
                monsterAI.StopMovement();
            }
            animator.SetTrigger("jumpscare");
            pauseMenu.LoseGame();

            //LookAtMonsterHead();
            //StartCoroutine(CameraShake());
        }
    }
    public void LookAtMonsterHead()
    {
        Vector3 directionToHead = monsterHead.position - playerCamera.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToHead);
        playerCamera.rotation = Quaternion.Slerp(playerCamera.rotation, targetRotation, Time.deltaTime * lookSpeed);
    }

    private void MoveCameraToHand()
    {
        Vector3 directionToHand = monsterHand.position - playerCamera.position;
        playerCamera.position = Vector3.MoveTowards(playerCamera.position, monsterHand.position, Time.deltaTime * lookSpeed);
    }
    private IEnumerator CameraShake()
    {
        Vector3 originalPosition = playerCamera.position;
        float shakeDuration = 0.5f;
        float shakeMagnitude = 0.1f;

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            playerCamera.position = originalPosition + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        playerCamera.position = originalPosition;
    }
}
