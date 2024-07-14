using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TInstance<T> : MonoBehaviour where T : TInstance<T>
{
    private static T instance;

    public static T Instance
    {
        get { return instance; }
    }

    virtual protected void Awake()
    {
        if (Instance == null)
            instance = (T)this;
        else Destroy(this);
    }
}
