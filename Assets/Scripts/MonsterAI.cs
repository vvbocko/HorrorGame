using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] Transform player;
    [SerializeField] MeshRenderer headMeshRenderer;
    [SerializeField] LayerMask playerMask;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] private float rangeOfSight = 25f;
    [SerializeField] private float sightAngle = 50f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float minTargetDistance = 1.6f;
    [SerializeField] private float normalSpeed = 3.5f;
    [SerializeField] private float retreatSpeed = 5f;
    [SerializeField] private float retreatDistance = 10f;

    private float distanceToTarget = Mathf.Infinity;
    private Vector3 targetPosition;
    private Vector3 lastKnownPosition;
    private bool isWalkPointSet;
    private bool isChasing;
    private bool blockChasing;
    public float range = 7.0f;
    private bool inLightedArea;
    Material headMaterial;
    Coroutine IgnorePlayerCoroutine;

    private void Start()
    {
        if (GameManager.IsSpawned)
        {
            //This is how you can access GameManager, through the "Instance" static property
            GameManager.Instance.SayHello();
        }
        
        navMeshAgent = GetComponent<NavMeshAgent>();
        headMaterial = headMeshRenderer.material;
        navMeshAgent.speed = normalSpeed;
        StartPatrolling();

    }

    private void Update()
    {
        if (!blockChasing && IsPlayertVisible(distanceToTarget)) //
        {
            headMaterial.color = Color.yellow;
            ChaseTarget();
        }
        else if (isChasing)
        {
            headMaterial.color = Color.grey;
            GoToLastKnownPosition();
        }

        Vector3 flatPosition = new Vector3 (transform.position.x, 0,transform.position.z);
        Vector3 flatTargetPosition = new Vector3 (targetPosition.x, 0, targetPosition.z );

        distanceToTarget = Vector3.Distance(flatPosition, flatTargetPosition);

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
            StartPatrolling();
        }
        else
        {
            headMaterial.color = Color.green;
            StartPatrolling();
        }

        if (distanceToTarget <= attackRange)
        {
            headMaterial.color = Color.red;
            AttackTarget();
        }
        Debug.DrawLine(transform.position, targetPosition);
        //else if (IsPlayertVisible(distanceToTarget) && CanReachTarget())
        //{

        //    headMaterial.color = Color.yellow;
        //    RotateTowardsTarget(player.position);
        //    ChaseTarget();
        //}
        //else if (isChasing)
        //{
        //    headMaterial.color = Color.grey;
        //    GoToLastKnownPosition();
        //}
        //else
        //{
        //    headMaterial.color = Color.green;
        //    StartPatrolling();
        //}
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
        SetTarget(player.position);
        lastKnownPosition = player.position;
        isChasing = true;

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
        navMeshAgent.speed = retreatSpeed;

        Vector3 directionAwayFromTarget = transform.position - player.position;
        directionAwayFromTarget.Normalize();
        Vector3 retreatPosition = transform.position + directionAwayFromTarget * retreatDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(retreatPosition, out hit, 1.0f, NavMesh.AllAreas))
        {
            //isWalkPointSet = true;
            SetTarget(hit.position);
        }
    }
    private void StartPatrolling()
    {
        Vector3 randomTarget;
        if (RandomPoint(transform.position, range, out randomTarget))
        {
            SetTarget(randomTarget);
            //isWalkPointSet = true;
        }
        else
        {
            SetTarget(transform.position);
        }
        //if (!isWalkPointSet)
        //{

        //    Vector3 randomTarget ;
        //    if (RandomPoint(transform.position, range, out randomTarget))
        //    {
        //        isWalkPointSet = true;
        //    }
        //    else
        //    {
        //        isWalkPointSet = false;
        //    }
        //}

        //if (isWalkPointSet)
        //{
        //    navMeshAgent.SetDestination(targetPosition);
        //}

        //if (navMeshAgent.remainingDistance < 1f && !navMeshAgent.pathPending)
        //{
        //    isWalkPointSet = false;
        //}
    }
    private void AttackTarget()
    {
        Debug.Log("Jumpscare >:o");
    }
    private void RotateTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // 5f = rotation speed
    }

    public void EnterLight()
    {
        RotateTowardsTarget(player.position);
        Retreat();
 
        inLightedArea = true;
    }

    public void ExitLight()
    {
        inLightedArea = false;
        navMeshAgent.speed = normalSpeed;
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
}