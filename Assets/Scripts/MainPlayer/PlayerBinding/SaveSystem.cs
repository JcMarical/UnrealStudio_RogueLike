
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

public class RealPlayerData
{
    private SpriteRenderer playerPicture;//玩家图片
    private float playerSpeed;//速度
    private float playerHealth;//生命
    private float playerDenfense;//防御值
    private float maxHealth;//角色最大生命
    private float weight;//玩家重量

    private int lucky;//幸运值
    private int unlucky;//不幸值
    private string strange;//玩家异常状态

    public RealPlayerData()
    {
        playerPicture = Player.Instance.realPlayerPicture;
        playerSpeed= Player.Instance.realPlayerSpeed;
        playerHealth= Player.Instance.realPlayerHealth;
        playerDenfense= Player.Instance.realPlayerDenfense;
        maxHealth = Player.Instance.realMaxHealth;
        weight= Player.Instance.realWeight;
        lucky= Player.Instance.realLucky;
        unlucky= Player.Instance.realUnlucky;
        strange= Player.Instance.realStrange;
    }
}
