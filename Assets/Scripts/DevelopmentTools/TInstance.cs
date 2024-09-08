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
            Debug.LogError("�Ҳ������Mgr�ǲ���������GameManager�����ʼ���ˣ�");
            string PrefabPath = "Prefabs/MgrPrefab/" + typeof(T).Name;
            var obj = Resources.Load(PrefabPath);
            if (obj)
            {
                var ga = Instantiate(obj) as GameObject;
                instance = ga.GetComponent<T>();
                if (instance != null)
                {
                    instance.SendMessage("Awake", SendMessageOptions.DontRequireReceiver);
                }
                _null_protected = true;
            }
            else
            {
                Debug.LogError("Loadʧ��");
            }
        }
    }

    virtual protected void Awake()
    {
        //Debug.Log(this.GetType() + "���ڳ�ʼ��");
        if (instance == null||instance==this)
        {
            instance = (T)this;
            DontDestroyOnLoad(this);
            //Debug.Log(this.GetType() + "��ʼ�����" + (Instance == null).ToString());
        }
        else
        {
            //Debug.LogWarning("�Ѵ���" + this.GetType() + "ʵ����������ʵ��");
            Destroy(this);
        }
    }
}
