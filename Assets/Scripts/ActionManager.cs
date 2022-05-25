using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    private static ActionManager instance = null;

    public static ActionManager Instance { get { return instance; } }
    private void Awake() 
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
    }

    public event Action OnCarReset = delegate { };
    
    public void NotifyOnCarReset()
    {
        OnCarReset();
    }

    public void NotifyOnStartPass()
    {

    }
}
