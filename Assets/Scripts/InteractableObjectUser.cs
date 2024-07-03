using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObjectUser : MonoBehaviour
{
    [SerializeField] public Action onClick;

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            onClick();
        }
    }
}
