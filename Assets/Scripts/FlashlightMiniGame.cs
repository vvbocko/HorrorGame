using UnityEngine;

public class FlashlightMiniGame : MonoBehaviour
{
    public Light flashlight;
    public CapsuleCollider lightCollider;
    [SerializeField] private MonsterMiniGame monsterMiniGame;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource humAudioSource;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip humSound;

    void Start()
    {
        flashlight.enabled = false;
        lightCollider.enabled = false;
        humAudioSource.loop = true;
        humAudioSource.clip = humSound;
        humAudioSource.Stop();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            audioSource.PlayOneShot(clickSound);
            flashlight.enabled = true;
            lightCollider.enabled = true;
            
            if (!humAudioSource.isPlaying)
            {
                humAudioSource.Play();
            }
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            audioSource.PlayOneShot(clickSound);
            flashlight.enabled = false;
            lightCollider.enabled = false;

            if (humAudioSource.isPlaying)
            {
                humAudioSource.Stop();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        MonsterMiniGameAI monster = other.GetComponent<MonsterMiniGameAI>();
        if (monster != null)
        {
            monsterMiniGame.ScareMonster();
        }
    }
}