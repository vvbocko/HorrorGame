using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private HeadBobController headBob;
    

    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float sprintSpeed = 2.5f;
    
    [SerializeField] private float useSpeed = 20f;
    [SerializeField] private float regenerateSpeed = 10f;
    [SerializeField] private float maxSprintPoints = 50f;
    [SerializeField] private float currentSprintPoints;

    [SerializeField] private float cooldownDuration = 5f;
    private float cooldownTimer = 0f;
    
    [SerializeField] private float walkFootstepInterval = 0.5f;
    [SerializeField] private float sprintFootstepInterval = 0.3f;
    private float footstepTimer;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] footstepSounds;

    private bool isCooldownActive = false;
    private bool isSprinting = false;


    float moveX;
    float moveZ;
    

    void Start()
    {
        currentSprintPoints = maxSprintPoints;
        footstepTimer = walkFootstepInterval;
        rigidBody.freezeRotation = true;

    }
    private void FixedUpdate()
    {
        HandleMovement();

        if (isCooldownActive)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isCooldownActive = false;
                currentSprintPoints = maxSprintPoints;
            }
        }
    }

    private void HandleMovement()
    {
        float currentSpeed = walkSpeed;
        float playFootstepsSoundsThreshold = 0 ;
        if (Input.GetKey(KeyCode.LeftShift) && currentSprintPoints > 0f && !isCooldownActive)
        {
            currentSpeed = sprintSpeed;
            currentSprintPoints -= Time.deltaTime * useSpeed;
            if (!isSprinting)
            {
                isSprinting = true;
                headBob.IncreaseHeadBob();
            }
            playFootstepsSoundsThreshold = sprintFootstepInterval;
        }
        else
        {
            if (isSprinting)
            {
                isSprinting = false;
                headBob.RestartHeadBob();
            }
            if (currentSprintPoints < maxSprintPoints && !isCooldownActive)
            {
                currentSprintPoints += Time.deltaTime * regenerateSpeed;
            }
            playFootstepsSoundsThreshold = walkFootstepInterval;
        }
        if (currentSprintPoints <= 0f && !isCooldownActive)
        {
            isCooldownActive = true;
            cooldownTimer = cooldownDuration;
        }

        currentSprintPoints = Mathf.Clamp(currentSprintPoints, 0, maxSprintPoints); 

        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical") ;

        Vector3 movement = transform.forward * moveZ + transform.right * moveX;
        Vector3 newDirection = new Vector3(movement.x, 0, movement.z);
        newDirection = Vector3.Normalize(newDirection);

        //rigidBody.MovePosition(rigidBody.position + newDirection * currentSpeed * Time.deltaTime);
        rigidBody.AddForce(newDirection * currentSpeed, ForceMode.VelocityChange );
        
        Vector3 flatVelocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
        
        if (flatVelocity.magnitude > currentSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * currentSpeed;
            rigidBody.velocity = new Vector3(limitedVelocity.x, rigidBody.velocity.y, limitedVelocity.z);
        }
        
        if (flatVelocity.magnitude > 0 && (moveX != 0 || moveZ != 0))
        {
            footstepTimer += Time.deltaTime;

            if (footstepTimer > playFootstepsSoundsThreshold)
            {
                PlayRandomFootStepSound();
                footstepTimer = 0;
            }
        }
        else
        {
            footstepTimer = 0;
        }
    }
    private void PlayRandomFootStepSound()
    {
        int randomIndex = Random.Range(0, footstepSounds.Length);
        AudioClip selectedClip = footstepSounds[randomIndex];
        audioSource.PlayOneShot(selectedClip);
    }
}