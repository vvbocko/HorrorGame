using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float closeAngle = 0f;
    [SerializeField] private float speed = 2f;
    private bool isOpen = false;
    private Quaternion targetRotation;
    private NavMeshObstacle navMeshObstacle;
  
    void Start()
    {
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        targetRotation = transform.rotation;
    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
    }

    public void OpenDoor()
    {
        targetRotation = Quaternion.Euler(0, -openAngle, 0);
        navMeshObstacle.enabled = false;
        isOpen = true;
    }
    public void CloseDoor()
    {
        targetRotation = Quaternion.Euler(0, closeAngle, 0);
        navMeshObstacle.enabled = true;
        isOpen = false;
    }

    public void ToggleDoor()
    {
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }
}