using MainPlayer;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class TInstance<T> : SerializedMonoBehaviour where T : TInstance<T> ,new()
{
    private static bool _null_protected=false;
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null && !_null_protected)
            {
                InitializeInstance();
            }
            return instance; 
        }
    }

    private static void InitializeInstance()
    {
        if (Application.isPlaying)
        {
            instance = FindAnyObjectByType<T>();
            if (!instance)
            {
                string PrefabPath = "Prefabs/MgrPrefab/" + typeof(T).Name;
                var obj = Resources.Load(PrefabPath);
                if (obj)
                {
                    var ga = Instantiate(obj) as GameObject;
                    instance = ga.GetComponent<T>();
                    _null_protected = true;
                    instance.Awake();
                }
                else
                {
                    Debug.LogError("Load失败");
                }
            }
        }
    }

    virtual protected void Awake()
    {
        Debug.Log(this.GetType() + "Awake");
        if (instance == null||instance==this)
        {
            instance = (T)this;
            Debug.Log(this.GetType() + "Awake完成" + (Instance != null).ToString());
        }
        else
        {
            Debug.LogWarning("已存在" + this.GetType() + "实例，销毁新实例");
            Destroy(this);
        }
    }
}
