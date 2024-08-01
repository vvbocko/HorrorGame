using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] Transform target;
    [SerializeField] MeshRenderer headMeshRenderer;
    [SerializeField] LayerMask playerMask;
    [SerializeField] private float rangeOfSight = 25f;
    [SerializeField] private float sightAngle = 60f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float normalSpeed = 3.5f;
    [SerializeField] private float slowedSpeed = 1f;

    private float distanceToTarget = Mathf.Infinity;
    private Vector3 walkPoint;
    private Vector3 lastKnownPosition;
    private bool isWalkPointSet;
    private bool isChasing;
    public float range = 7.0f;
    private bool inLightedArea = false;
    Material headMaterial;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        headMaterial = headMeshRenderer.material;
        navMeshAgent.speed = normalSpeed;
    }

    private void Update()
    {
        distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget <= attackRange)
        {
            headMaterial.color = Color.red;
            AttackTarget();
        }
        else if (IsTargetVisible(distanceToTarget))
        {
            headMaterial.color = Color.yellow;
            ChaseTarget();
        }
        else if (isChasing)
        {
            headMaterial.color = Color.grey;
            GoToLastKnownPosition();
        }
        else
        {
            headMaterial.color = Color.green;
            StartPatrolling();
        }
        Debug.DrawLine(transform.position, walkPoint);
    }

    private void StartPatrolling()
    {
        if (!isWalkPointSet)
        {
            SetWalkPoint();
        }

        if (isWalkPointSet)
        {
            navMeshAgent.SetDestination(walkPoint);
        }

        if (navMeshAgent.remainingDistance < 1f && !navMeshAgent.pathPending)
        {
            isWalkPointSet = false;
        }
    }

    private bool IsTargetVisible(float distanceToTarget)
    {
        if (distanceToTarget > rangeOfSight)
        {
            return false;
        }
        Vector3 directionToTarget = target.position - transform.position;
        directionToTarget.Normalize();
        float angle = Vector3.Angle(directionToTarget, transform.forward);

        if (angle > sightAngle)
        {
            return false;
        }

        if (!Physics.Raycast(transform.position, directionToTarget, rangeOfSight, playerMask))
        {
            return false;
        }
        Debug.Log("Player in range of sight");
        return true;
    }

    private void SetWalkPoint()
    {
        if (RandomPoint(transform.position, range, out walkPoint))
        {
            isWalkPointSet = true;
        }
        else
        {
            isWalkPointSet = false;
        }
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

    private void ChaseTarget()
    {
        navMeshAgent.SetDestination(target.position);
        lastKnownPosition = target.position;
        isChasing = true;
    }

    private void GoToLastKnownPosition()
    {
        navMeshAgent.SetDestination(lastKnownPosition);

        if (Vector3.Distance(transform.position, lastKnownPosition) < 1f)
        {
            isChasing = false;
            StartPatrolling();
        }
    }

    private void AttackTarget()
    {
        Debug.Log("Jumpscare >:o");
    }

    public void EnterLight()
    {
        inLightedArea = true;
        navMeshAgent.speed = slowedSpeed;
    }

    public void ExitLight()
    {
        inLightedArea = false;
        navMeshAgent.speed = normalSpeed;
    }
}