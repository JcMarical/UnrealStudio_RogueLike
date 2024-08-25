
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json;
using UnityEditor;
using Unity.VisualScripting;
using System;
using MainPlayer;
/// <summary>
/// 保存绑定的脚本
/// <summary>



public class SaveSystem : TInstance<SaveSystem>
{
    public Dictionary<string, string> dic= new Dictionary<string, string>();


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }



    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        dic=LoadData<Dictionary<string,string>>("/BindingDictionary.json");
    }



    public static void SaveData<T>(T t,string dataName)//保存数据
    {
        if (!File.Exists(Application.persistentDataPath + "/SaveData"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveData");
        }
        string jsonData = JsonConvert.SerializeObject(t);
        File.WriteAllText(Application.persistentDataPath + "/SaveData"+dataName, jsonData);
    }

    public T LoadData<T>(string dataName)//加载数据
    {
        string path = Application.persistentDataPath + "/SaveData" + dataName;
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            T t = JsonConvert.DeserializeObject<T>(jsonData);
            return t;
        }
        else
        {
            return default(T);
        }
    }

    public void Quit()
    {
        SaveData(BindingChange.Instance.bindings, "/BindingDictionary.json");
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    } 
}


