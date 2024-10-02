using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
    // Static private reference to the singleton instance (only one in the whole game can exist!)
    private static T m_Instance = null;

    // This is how you access the m_Instance! Static means that I don't need a reference to an object, I can access it directly from the class
    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                // Try to find one in the scene
                m_Instance = FindObjectOfType<T>();
                if (m_Instance != null)
                {
                    (m_Instance as Singleton<T>).InitOnce();
                }
            }
            return m_Instance;
        }
    }

    public static bool IsSpawned => m_Instance != null;

    protected bool IsInit { get; private set; }
    
    protected void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            // If I'm not the m_Instance, I will destroy myself
            Debug.LogErrorFormat(this, "There's already a singleton of type {0}, destroying copy", GetType());
            Destroy(gameObject);
            return;
        }
        
        // If there's no m_Instance, I'll become m_Instance
        m_Instance = this as T;
        InitOnce();
    }

    private void InitOnce()
    {
        if (IsInit)
        {
            return;
        }
        IsInit = true;
        Init();
    }

    // "virtual" means that it can be overriden in derived classes (e.g. in Game Manager can have its own implementation)
    protected virtual void Init()
    {
        // For derived classes to initialize
    }

    protected virtual void Uninit()
    {
        // For derived classes to deinitialize
    }

    protected virtual void OnDestroy()
    {
        if (IsInit)
        {
            Uninit();
        }
        m_Instance = null;
    }
}
