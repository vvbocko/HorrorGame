using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMiniGameAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    void Update()
    {
        
    }

    public void MoveToPoint(Vector3 point)
    {
        agent.SetDestination(point);
    }
    public void Teleport(Vector3 point)
    {
        transform.position = point;
    }
    public void SetIsStopped(bool isStopped)
    {
        agent.isStopped = isStopped;
    }
}
