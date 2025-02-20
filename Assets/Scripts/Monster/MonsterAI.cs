using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform player;
    [SerializeField] private Jumpscare jumpscare;
    [SerializeField] private MeshRenderer headMeshRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private AudioClip retreatSound;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float rangeOfSight = 25f;
    [SerializeField] private float sightAngle = 50f;
    [SerializeField] private float attackRange = 1.5f;

    [SerializeField] private float teleportTriggerDistance = 30f;
    [SerializeField] private float minTeleportRadius = 15f;
    [SerializeField] private float maxTeleportRadius = 20f;

    [SerializeField] private float minTargetDistance = 1.6f;
    [SerializeField] private float normalSpeed = 3.5f;
    [SerializeField] private float retreatSpeed = 5f;
    [SerializeField] private float retreatDistance = 10f;
    
    [SerializeField] private float footstepInterval = 0.6f;
    private float footstepTimer;
    [SerializeField] private float teleportCooldown = 10f;
    private float nextTeleportTime;

    //[SerializeField] private Transform monstersBody;  // - to rotate only te body when monster is retreating

    private float distanceToTarget = Mathf.Infinity;
    public float range = 7.0f;
    private Vector3 targetPosition;
    private Vector3 lastKnownPosition;
    private bool isWalkPointSet;
    private bool isChasing;
    private bool isRetreating;
    private bool blockChasing;
    private bool inLightedArea;
    Material headMaterial;
    Coroutine IgnorePlayerCoroutine;
    
    // if monster close enough to the player - stop his movement or delete this monster and spawn another
    // get reference to the camera and rotate it so it faces the monster
    //play the animation
    //wait for some seconds and then change the scene to the You died screen 
    private void Start()
    {
        if (GameManager.IsSpawned)
        {
            GameManager.Instance.SayHello();
        }
 
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = normalSpeed;

        headMaterial = headMeshRenderer.material;
        StartPatrolling();

    }

    private void Update()
    {
        ProcesFootstepsSounds();

        if (!blockChasing && IsPlayertVisible(distanceToTarget))
        {            
            ChaseTarget();
            headMaterial.color = Color.yellow;
        }
        else if (isChasing)
        {
            GoToLastKnownPosition();
            headMaterial.color = Color.grey;
        }

        Vector3 flatPosition = new Vector3 (transform.position.x, 0,transform.position.z); //pozycja na p³aszczyznie (pod³ogi)
        Vector3 flatTargetPosition = new Vector3 (targetPosition.x, 0, targetPosition.z );

        distanceToTarget = Vector3.Distance(flatPosition, flatTargetPosition); //dystans miedzy pozycjami potwora i targetu

        if (distanceToTarget > minTargetDistance && CanReachTarget())
        {
            GoToTarget();  
        }
        else if (isChasing && !CanReachTarget())
        {
            if(IgnorePlayerCoroutine != null)
            {
                StopCoroutine(IgnorePlayerCoroutine);
            }
            IgnorePlayerCoroutine = StartCoroutine(IgnorePlayerAndPatrol());
            if (inLightedArea)
            {
                Retreat();
            }
            else
            {
                StartPatrolling();
            }
        }
        else/////
        {
            headMaterial.color = Color.green;
            if (inLightedArea)
            {
                Retreat();
            }
            else
            {
                StartPatrolling();
            }
        }

        Vector3 flatPlayerPosition = new Vector3(player.position.x, 0, player.position.z);
        float distanceToPlayer = Vector3.Distance(flatPosition, flatPlayerPosition);
        
        if (!inLightedArea && isChasing && distanceToPlayer <= attackRange)
        {
            headMaterial.color = Color.red;
            //AttackTarget();
            jumpscare.PlayJumpscare();
        }

        Debug.LogFormat("Distance:{0}" ,distanceToPlayer);
        //gracz czy kolejne miejsce
        if (distanceToPlayer >= teleportTriggerDistance && Time.time >= nextTeleportTime)
        {
            TeleportMonster();
            nextTeleportTime = Time.time + teleportCooldown;
           
        }
    }

    private bool IsPlayertVisible(float distanceToPlayer)
    {
        if (distanceToPlayer > rangeOfSight)
        {
            return false;
        }

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(directionToPlayer, transform.forward);

        if (angle > sightAngle)
        {
            return false;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, rangeOfSight, playerMask | obstacleMask))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.Log("Player in range and visible.");
                return true;
            }
        }

        return false;
    }


    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1f, NavMesh.AllAreas))
            {
                result = hit.position;

                NavMeshPath path = new NavMeshPath();
                navMeshAgent.CalculatePath(result, path);
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    return true;
                }
            }
        }
        result = Vector3.zero;
        return false;
    }

    private void GoToTarget()
    {
        navMeshAgent.SetDestination(targetPosition);
    }
    private bool CanReachTarget()
    {
        NavMeshPath path = new NavMeshPath();
        navMeshAgent.CalculatePath(targetPosition, path);
        if (path.status == NavMeshPathStatus.PathPartial || path.status == NavMeshPathStatus.PathInvalid)
        {
            return false;
        }
        return true;
    }
    private void SetTarget(Vector3 target)
    {
        targetPosition = target;
    }

    private void ChaseTarget()
    {
        animator.SetTrigger("walk");

        isChasing = true;

        SetTarget(player.position);
        lastKnownPosition = player.position;

        RotateTowardsTarget(player.position);

    }

    private void GoToLastKnownPosition()
    {
        SetTarget(lastKnownPosition);

        if (Vector3.Distance(transform.position, lastKnownPosition) < 1f)
        {
            isChasing = false;
            StartPatrolling();
        }
    }
    public void Retreat()
    {
        if (IgnorePlayerCoroutine != null)
        {
            StopCoroutine(IgnorePlayerCoroutine);
        }
        IgnorePlayerCoroutine = StartCoroutine(IgnorePlayerAndPatrol());
        //bool isCovering , level design aby mo¿na by³o wygraæ przegraæ aby rozwiazywanie zagadek dzia³a³o, UI - przegrana i menu z mo¿liwoœci zamykaniem aplikacji, plus sound design
        navMeshAgent.speed = retreatSpeed;

        Vector3 directionAwayFromTarget = transform.position - player.position;
        directionAwayFromTarget.Normalize();
        Vector3 retreatPosition = transform.position + directionAwayFromTarget * retreatDistance;

        Vector3 randomTarget;
        if (RandomPoint(retreatPosition, range, out randomTarget))
        {
            SetTarget(randomTarget);
        }
        else
        {
            retreatPosition = Vector3.zero;
        }
    }
    private void StartPatrolling()
    {
        Vector3 randomTarget;
        if (RandomPoint(transform.position, range, out randomTarget))
        {
            SetTarget(randomTarget);
        }
        else
        {
            SetTarget(transform.position);
        }
    }
    private void AttackTarget()
    {     
        animator.SetTrigger("jumpscare");
        Debug.Log("Jumpscare >:o");
    }
    private void RotateTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // want to only rotate the "visual part" | 5f = rotation speed
    }

    public void EnterLight()
    {
        if (!inLightedArea)
        {
            StartCoroutine(StopInLight());
            Retreat();
            inLightedArea = true;
        }
        navMeshAgent.updateRotation = false;
        RotateTowardsTarget(player.position);
    }

    public void ExitLight()
    {
        inLightedArea = false;
        navMeshAgent.updateRotation = true;
        navMeshAgent.speed = normalSpeed;
    }
    private IEnumerator StopInLight()
    {
        navMeshAgent.isStopped = true;
        animator.SetTrigger("damage");
        audioSource.PlayOneShot(retreatSound);
        yield return new WaitForSeconds(0.8f);
        navMeshAgent.isStopped = false;

    }
    private IEnumerator IgnorePlayerAndPatrol()
    {
        blockChasing = true;
        isChasing = false;
        //StartPatrolling();
        yield return new WaitForSeconds(3f);
        blockChasing = false;

        IgnorePlayerCoroutine = null;
    }
    private void PlayRandomFootStepSound()
    {
        int randomIndex = Random.Range(0, footstepSounds.Length);
        AudioClip selectedClip = footstepSounds[randomIndex];
        audioSource.PlayOneShot(selectedClip);
    }
    private void ProcesFootstepsSounds()
    {
        footstepTimer += Time.deltaTime;
        if (footstepTimer > footstepInterval)
        {
            PlayRandomFootStepSound();
            footstepTimer = 0;
        }
    }
    private void TeleportMonster()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float radius = Random.Range(minTeleportRadius, maxTeleportRadius);

        float x = player.position.x + Mathf.Cos(angle) * radius;
        float z = player.position.z + Mathf.Sin(angle) * radius;
        Vector3 teleportPosition = new Vector3(x, transform.position.y, z);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(teleportPosition, out hit, 2f, NavMesh.AllAreas))
        {
            navMeshAgent.Warp(hit.position);
        }
    }
    public void StopMovement()
    {
        navMeshAgent.enabled = false;
    }
}