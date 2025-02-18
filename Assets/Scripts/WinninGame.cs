using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinninGame : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private GameObject wall;

    void Start()
    {
        container.transform.position = new Vector3(6.07f, 10.8127f, -85.69f);
    }
    
    public void MoveContainer()
    {
        container.transform.position = new Vector3(container.transform.position.x, container.transform.position.y, -89.57f);
    }
    private void OnTriggerExit(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            GameManager.Instance.WinGame();
        }
    }
}
