using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PInstance<T> : MonoBehaviour where T:PInstance<T>
{
    private static T instance;
    public static T Instance
    {
        get {  return instance; }
    }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            instance = (T)this;
        }
        else
        {
            Destroy(gameObject);
        }       
    }

}
