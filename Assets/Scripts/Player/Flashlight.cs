using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private MonsterAI monster;
    [SerializeField] private Light flashlight;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource humAudioSource;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip humSound;
    [SerializeField] private KeyCode toggleKey = KeyCode.F;
    [SerializeField] private float flashlightRange = 15f;
    [SerializeField] private LayerMask monsterMask;

    //private bool sk;



    private void Start()
    {
        monster = FindObjectOfType<MonsterAI>();

        if (flashlight == null)
        {
            flashlight = GetComponentInChildren<Light>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleFlashlight();
        }

        if (flashlight.enabled)
        {
            DetectMonster();
        }
    }
    private void ToggleFlashlight()
    {
        flashlight.enabled = !flashlight.enabled;

        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        if (flashlight.enabled)
        {
            if (humSound != null && !humAudioSource.isPlaying)
            {
                humAudioSource.clip = humSound;
                humAudioSource.loop = true;
                humAudioSource.Play();
            }
        }
        else
        {
            if (humAudioSource.isPlaying)
            {
                humAudioSource.clip = null;
                humAudioSource.Stop();
            }

            if (monster != null)
            {
                monster.ExitLight();
            }
        }
    }
    private void DetectMonster()
    {
        RaycastHit hit;
        Vector3 directionToMonster = flashlight.transform.forward;

        if (Physics.Raycast(flashlight.transform.position, directionToMonster, out hit, flashlightRange, monsterMask))
        {

            MonsterAI monster = hit.collider.GetComponent<MonsterAI>();
            if (monster != null)
            {
                monster.EnterLight();
            }

            Debug.DrawRay(flashlight.transform.position, directionToMonster * flashlightRange, Color.green);
        }
        else
        {
            Debug.DrawRay(flashlight.transform.position, directionToMonster * flashlightRange, Color.red);
        }
    }
}