
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


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
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

    public static T LoadData<T>(string dataName)//加载数据
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

}


