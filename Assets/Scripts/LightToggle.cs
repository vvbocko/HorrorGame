using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class LightToggle : MonoBehaviour
{
    [SerializeField] private Light pointLight;
    private void OnTriggerEnter(Collider other)
    {
        MonsterAI monster = other.GetComponent<MonsterAI>();
        if (monster != null)
        {
            monster.EnterLight();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MonsterAI monster = other.GetComponent<MonsterAI>();
        if (monster != null)
        {
            monster.ExitLight();
        }
    }

    public void ToggleLight()
    {
        if (pointLight != null)
        {
            pointLight.enabled = !pointLight.enabled;
        }
    }   
}
