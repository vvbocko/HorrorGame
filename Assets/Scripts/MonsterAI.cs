using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] Transform target;
    [SerializeField] private float rangeOfSight = 8f;
    [SerializeField] private float attackRange = 2f;

    private float distanceToTarget = Mathf.Infinity;

    private Vector3 walkPoint;
    private Vector3 lastKnownPosition;
    private bool isWalkPointSet;
    private bool isChasing;
    public float range = 7.0f;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget <= attackRange)
        {
            AttackTarget();
        }
        else if (distanceToTarget <= rangeOfSight)
        {
            ChaseTarget();
        }
        else if (isChasing)
        {
            GoToLastKnownPosition();
        }
        else
        {
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

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
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
}
