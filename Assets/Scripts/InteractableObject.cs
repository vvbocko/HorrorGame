using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] public UnityEvent onInteraction;
    [SerializeField] public Transform interactablePoint = null;
    [SerializeField] private bool isPickable;
    [SerializeField] public ItemDefinition itemDefinition;

    public bool IsPickable
    {
        get
        {
            return isPickable;
        }
    }
    public Transform GetInteractablePointTranform()
    {
        if(interactablePoint != null)
        {
            return interactablePoint;
        }
        else
        {
            return transform;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        InteractableObjectUser player = other.GetComponent<InteractableObjectUser>();

        if (player != null)
        {
            player.onClick += Interact;
            player.SetInteractableObject(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableObjectUser player = other.GetComponent<InteractableObjectUser>();

        if (player != null)
        {
            player.onClick -= Interact;
            player.SetInteractableObject(null);
        }
    }

    private void Interact()
    {
        onInteraction.Invoke();
    }
}
