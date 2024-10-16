using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private MonsterAI monster;
    [SerializeField] private Light flashlight;
    [SerializeField] private KeyCode toggleKey = KeyCode.F;
    [SerializeField] private float flashlightRange = 15f;
    [SerializeField] private LayerMask monsterMask;

    private bool sk;



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
            flashlight.enabled = !flashlight.enabled;
            if (!flashlight.enabled && monster != null)
            {
                monster.ExitLight();
            }
        }

        if (flashlight.enabled)
        {
            DetectMonster();
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