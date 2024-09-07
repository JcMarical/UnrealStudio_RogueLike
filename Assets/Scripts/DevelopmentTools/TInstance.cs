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
        //Debug.Log(this.GetType() + "���ڳ�ʼ��");
        if (Instance == null||Instance==this)
        {
            instance = (T)this;
            //Debug.Log(this.GetType() + "��ʼ�����" + (Instance == null).ToString());
        }
        else
        {
            //Debug.LogWarning("�Ѵ���" + this.GetType() + "ʵ����������ʵ��");
            Destroy(this);
        }
    }
}
