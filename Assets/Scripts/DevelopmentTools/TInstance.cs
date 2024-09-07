using MainPlayer;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class TInstance<T> : SerializedMonoBehaviour where T : TInstance<T>
{
    private static T instance;

    private static List<Type> Ignore = new()
    { 
        typeof(Player),
        typeof(GameManager)
    };

    public static T Instance
    {
        get 
        {
            if (instance == null || instance.IsUnityNull())
            {
                Debug.LogWarning(Ignore.Contains(typeof(T)));
                instance = FindAnyObjectByType<T>();
                if ((instance == null || instance.IsUnityNull()) && !Ignore.Contains(typeof(T)))
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
        //Debug.Log(this.GetType() + "正在初始化");
        if (Instance == null||Instance==this)
        {
            instance = (T)this;
            //Debug.Log(this.GetType() + "初始化完成" + (Instance == null).ToString());
        }
        else
        {
            //Debug.LogWarning("已存在" + this.GetType() + "实例，销毁新实例");
            Destroy(this);
        }
    }
}
