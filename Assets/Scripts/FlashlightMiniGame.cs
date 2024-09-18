using UnityEngine;

public class FlashlightMiniGame : MonoBehaviour
{
    public Light flashlight;
    public CapsuleCollider lightCollider;
    [SerializeField] private MonsterMiniGame monsterMiniGame;

    void Start()
    {
        flashlight.enabled = false;
        lightCollider.enabled = false;
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            flashlight.enabled = true;
            lightCollider.enabled = true;
        }
        else
        {
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