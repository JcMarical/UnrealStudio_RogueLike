using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json;
using UnityEditor;
using Unity.VisualScripting;
using System;
/// <summary>
/// 保存绑定的脚本
/// </summary>

public class SaveBinding : MonoBehaviour//
{

    public Dictionary<string, string> dic= new Dictionary<string, string>();

    private static SaveBinding instance;

    public static SaveBinding Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
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
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/SaveData");
        }
        string jsonData = JsonConvert.SerializeObject(t);
        File.WriteAllText(Application.persistentDataPath + "/SaveData"+dataName, jsonData);
    }

    public T LoadData<T>(string dataName) where T : class//加载数据
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
            return null;
        }
    }

    public void Quit()
    {
        SaveData(BindingChange.Instance.bindings, "/BindingDictionary.json");

        EditorApplication.isPlaying = false;
        Application.Quit();
    }

   

    
}
