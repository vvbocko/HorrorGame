using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;
    [SerializeField] private Transform cam;
    [SerializeField] private float maxPickUpDistance = 5f;
    [SerializeField] private PlayerInventory playerInventory;
    private InteractableObject interactableObject;
    private GameObject heldObject;
    private bool InRotationMode = false;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (heldObject == null)
            {
                TryPickUp();
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            DropObject();
        }
        //if (Input.GetKeyDown(KeyCode.R) && !InRotationMode) //in rotation mode time stop as well (like in pause game)
        //{
        //    //get it to the middle
        //    //RotateObject();
        //}
        //else if(Input.GetKeyDown(KeyCode.R) && !InRotationMode)
        //{
        //    //get back
        //}

    }
    private void TryPickUp()
    {

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxPickUpDistance))
        {
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();
            if (interactable != null && interactable.IsPickable)
            {
                playerInventory.AddItem(interactable.itemDefinition);
                heldObject = interactable.gameObject;
                PickUpObject();
            }
        }
    }

    private void PickUpObject()
    {
        Rigidbody rigidbody = heldObject.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.isKinematic = true;
        }
        Collider collider = heldObject.GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
        heldObject.transform.SetParent(holdPoint, false);
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;

    }

    void DropObject()
    {
        if (heldObject != null)
        {
            Rigidbody rigidbody = heldObject.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.isKinematic = false;
            }
            Collider collider = heldObject.GetComponent<Collider>();
            if (collider != null)
            {
                collider.isTrigger = false;
            }

            InteractableObject interactable = heldObject.GetComponent<InteractableObject>();

            if (interactable != null)
            {
                 playerInventory.RemoveItem(interactable.itemDefinition);
            }
            heldObject.transform.SetParent(null);
            heldObject = null;
        }
    }
}
