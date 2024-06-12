using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LightToggle : MonoBehaviour
{
    private Light pointLight;
    private NavMeshObstacle navMeshObstacle;

    private void Start()
    {
        pointLight = GetComponentInChildren<Light>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();
    }

    private void OnMouseDown()
    {
        if (pointLight != null)
        {
            pointLight.enabled = !pointLight.enabled;
            navMeshObstacle.enabled = !navMeshObstacle.enabled;
        }
    }
}
