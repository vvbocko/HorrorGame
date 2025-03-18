using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] public UnityEvent onInteraction;
    [SerializeField] public Transform interactablePoint = null;
    [SerializeField] private bool isPickable;
    [SerializeField] public bool isInteractable = true;
    //[SerializeField] private bool isRotatable;
    [SerializeField] public ItemDefinition itemDefinition;
    private bool isSubscribed = false;
    public bool IsPickable
    {
        get
        {
            return isPickable;
        }
    }
    //public bool IsInteractable
    //{
    //    get
    //    {
    //        return isRotatable;
    //    }
    //}
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

        if (player != null && !isSubscribed)
        {
            isSubscribed = true;
            player.onClick += Interact;
            player.SetInteractableObject(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableObjectUser player = other.GetComponent<InteractableObjectUser>();

        if (player != null && isSubscribed)
        {
            isSubscribed = false;
            player.onClick -= Interact;
            player.SetInteractableObject(null);
        }
    }

    private void Interact()
    {
        onInteraction.Invoke();
    }
}
