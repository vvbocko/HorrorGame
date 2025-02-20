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
    private bool isHumSoundPlaying = false;
    void Start()
    {
        flashlight.enabled = false;
        lightCollider.enabled = false;
        isHumSoundPlaying = false;
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            audioSource.PlayOneShot(clickSound);
            //if (!audioSource.isPlaying && audioSource != null)
            //{
            //    audioSource.PlayOneShot(humSound);
            //    audioSource.loop = false;
            //    isHumSoundPlaying = true;
            //}
            flashlight.enabled = true;
            lightCollider.enabled = true;
        }
        else
        {
            //if (!isHumSoundPlaying)
            //{
            //    audioSource = null;
            //}
            //audioSource.PlayOneShot(clickSound);      
            flashlight.enabled = false;
            lightCollider.enabled = false;
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