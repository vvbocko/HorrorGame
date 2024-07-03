using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] public UnityEvent onInteraction;

    private void OnTriggerEnter(Collider other)
    {
        InteractableObjectUser player = other.GetComponent<InteractableObjectUser>();

        if (player != null)
        {
            player.onClick += Interact;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableObjectUser player = other.GetComponent<InteractableObjectUser>();

        if (player != null)
        {
            player.onClick -= Interact;
        }
    }

    private void Interact()
    {
        onInteraction.Invoke();
    }
}
