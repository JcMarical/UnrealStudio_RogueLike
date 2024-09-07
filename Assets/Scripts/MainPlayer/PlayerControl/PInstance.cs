using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PInstance<T> : MonoBehaviour where T : PInstance<T>
{
    private static T instance;
    public static T Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<T>();
                if(instance==null)
                {
                    return null;
                }
            }
            return instance; 
        }    
    }

    virtual protected void Awake()
    { 
        if (instance == null)
        {
            instance = (T)this;
        }
        else if(instance!=(T)this)
        {
            Destroy(this);
        }
    }
}
