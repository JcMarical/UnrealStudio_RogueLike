using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class W_TInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance=null;
    public static T Instance{
        get{
            lock(instance){
                if(instance==null){
                    instance = FindAnyObjectByType<T>();
                    if(instance==null){
                        GameObject temp=new GameObject();
                        temp.AddComponent<T>();
                        instance = temp.GetComponent<T>();
                    }
                }
                return instance;
            }
        }
    }
    private void Awake() {
        if(instance==null){
            instance=this as T;
        }
    }
}
