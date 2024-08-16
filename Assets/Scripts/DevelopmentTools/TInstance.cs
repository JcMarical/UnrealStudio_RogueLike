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
        Debug.Log(this.GetType() + "���ڳ�ʼ��" );
        if (Instance == null || instance == this)
        {
            instance = (T)this;
            Debug.Log(this.GetType() + "��ʼ�����" + (Instance == null).ToString());
        }
        else
        {
            Debug.LogWarning("�Ѵ���" + this.GetType() + "ʵ����������ʵ��");
            Destroy(this);
        }
    }
}
