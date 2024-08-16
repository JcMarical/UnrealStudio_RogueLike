using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TInstance<T> : MonoBehaviour where T : TInstance<T> , new()
{
    private static T instance;

    public static T Instance
    {
        get 
        {
            if (instance == null || instance.IsUnityNull())
            {
                instance = FindAnyObjectByType<T>();
                if (instance == null || instance.IsUnityNull())
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    instance = go.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    virtual protected void Awake()
    {
        Debug.Log(this.GetType() + "正在初始化" );
        if (Instance == null || instance == this)
        {
            instance = (T)this;
            Debug.Log(this.GetType() + "初始化完成" + (Instance == null).ToString());
        }
        else
        {
            Debug.LogWarning("已存在" + this.GetType() + "实例，销毁新实例");
            Destroy(this);
        }
    }
}
