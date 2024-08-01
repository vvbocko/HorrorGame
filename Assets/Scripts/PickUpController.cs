using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    [SerializeField] private InteractableObject interactable;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private BoxCollider itemCollider;

    [SerializeField] private Transform holdPoint, player, playerCamera;

    [SerializeField] private float pickUpRange;
    [SerializeField] private float dropForceForward, dropForceUpward;

    [SerializeField] private bool isEquipped;
    public static bool isSlotFull;

    private void Start()
    {
        if(!isEquipped)
        {
            interactable.enabled = false;
            rigidBody.isKinematic = false;
            itemCollider.isTrigger = false;
            isSlotFull = false;
        }
        if (!isEquipped)
        {
            interactable.enabled = true;
            rigidBody.isKinematic = true;
            itemCollider.isTrigger = true;
            isSlotFull = true;
        }
    }
    private void Update()
    {
        Vector3 distaceToPlayer = player.position - transform.position;
        if (!isEquipped && distaceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !isSlotFull)
        {
            PickUp();
        }

        if (isEquipped && Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }
    }
    private void PickUp()
    {
        isEquipped = true;
        isSlotFull = true;
        rigidBody.isKinematic = true;
        itemCollider.isTrigger = true;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        interactable.enabled = true;

    }
    private void Drop()
    {
        isEquipped = false;
        isSlotFull = false;
        rigidBody.isKinematic = false;
        itemCollider.isTrigger = false;

        transform.SetParent(null);

        rigidBody.velocity = player.GetComponent<Rigidbody>().velocity;
        rigidBody.AddForce(playerCamera.forward * dropForceForward, ForceMode.Impulse);
        rigidBody.AddForce(playerCamera.up * dropForceUpward, ForceMode.Impulse);

        float random = Random.Range(-1f, 1f);
        rigidBody.AddTorque(new Vector3(random, random, random));

        interactable.enabled = false;
    }


}
