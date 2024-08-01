using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObjectUser : MonoBehaviour
{
    [SerializeField] public Action onClick;
    [SerializeField] private float sightAngle = 20f;
    
    private InteractableObject interactableObject;

    void Update()
    {
        if (interactableObject != null)
        {
            Debug.DrawLine(transform.position, interactableObject.GetInteractablePointTranform().position, Color.red);
        }
        
        if (Input.GetButtonDown("Fire1") && IsInteractableObjectVisible())
        {

            onClick?.Invoke();
        }

    }
    public void SetInteractableObject(InteractableObject interactable)
    {
        interactableObject = interactable;
    }
    private bool IsInteractableObjectVisible() 
    { 
        if(interactableObject == null)
        {
            return false;
        }

        Vector3 directionToTarget = interactableObject.GetInteractablePointTranform().position - transform.position;
        directionToTarget.Normalize();
        directionToTarget.y = 0;
        float angle = Vector3.Angle(directionToTarget, transform.forward);
        
        Debug.Log(angle);
        if(angle > sightAngle)
        {
            return false;
        }
        return true;
    }
}
