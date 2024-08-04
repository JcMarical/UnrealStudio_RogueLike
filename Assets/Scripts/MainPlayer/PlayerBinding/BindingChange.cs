using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using MainPlayer;
using System;
using UnityEngine.UIElements;
using System.Drawing.Text;
using System.ComponentModel;
using System.Reflection;

/// <summary>
/// 更改绑定的脚本
/// </summary>
public class BindingChange : MonoBehaviour
{
    public PlayerSettings inputControl;
    private static BindingChange instance;
    public static BindingChange Instance
    {
        get
        {
            return instance;
        }
    }

    public TextMeshProUGUI bindingText;
    public TMP_Dropdown dropdown;
    public TMP_Dropdown bindingDropdown;

    public Dictionary<string, string> bindings;

    private string preBinding;//记录切换绑定前对应的字符

    private PlayerAnimation playerAnimation;

    public TextAsset textAsset;

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
        inputControl = new PlayerSettings();
        playerAnimation=FindObjectOfType<PlayerAnimation>();
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        InitDictionary();
        InitDropDown();
        bindingText = bindingText.GetComponent<TextMeshProUGUI>();
        dropdown = dropdown.GetComponent<TMP_Dropdown>();
        bindingDropdown = bindingDropdown.GetComponent<TMP_Dropdown>();
    }

    private void InitDictionary()//初始化字典
    {
        if(SaveSystem.Instance.dic == null)//第一次进入游戏时，对与按键绑定有关的字典初始化
        {
            bindings = new Dictionary<string, string>();
            bindings.Add(" ", " ");
            string[] str=textAsset.text.Split(',');
            foreach (string s in str)//遍历获取keycode枚举类型中的键值
            {
                 bindings.Add("<Keyboard>/" +s, " ");
            }

            foreach(var item in bindings)
            {
                Debug.Log(item.Key+" "+item.Value);
            }

            bindings["<Keyboard>/w"] = "Up";
            bindings["<Keyboard>/s"] = "Down";
            bindings["<Keyboard>/a"] = "Left";
            bindings["<Keyboard>/d"] = "Right";
            bindings["<Keyboard>/l"] = "Dash";
            bindings["<Keyboard>/space"] = "ChangeWeapon";
        }
        else//保存绑定后，将保存的字典赋值给当前脚本的字典，并根据字典改变绑定
        {
            bindings=new Dictionary<string, string>(SaveSystem.Instance.dic);
            foreach (KeyValuePair<string, string> item in bindings)
            {
                if (item.Value!=" ")
                {
                    if (string.Equals(item.Value,"Up")|| string.Equals(item.Value, "Down")|| string.Equals(item.Value, "Left")|| string.Equals(item.Value, "Right"))
                    {
                        int index = 0;
                        switch(item.Value)
                        {
                            case "Up":index=1; break;
                            case "Left": index = 2; break;
                            case "Down": index = 3; break;
                            case "Right": index = 4; break;
                            default: break; 
                        }
                        
                        inputControl.FindAction("Move").ChangeBinding(index).WithPath(item.Key);
                        playerAnimation.inputControl.FindAction("Move").ChangeBinding(index).WithPath(item.Key);
                    }
                    else
                    {
                        inputControl.FindAction(item.Value).ChangeBinding(0).WithPath(item.Key);
                        playerAnimation.inputControl.FindAction(item.Value).ChangeBinding(0).WithPath(item.Key);
                    }
                }
            }
        }

    }

    private void InitDropDown()
    {
        List<string> list = new List<string>();
        string[] str = textAsset.text.Split(',');
        foreach(var item in str)
        {
            list.Add(item);
        }
        bindingDropdown.AddOptions(list);
    }

    public void OnValueChanged()//dropdown值变化时调用
    {
        if (dropdown.options[dropdown.value].text != " ")
        {
            bindingText.text = bindings.FirstOrDefault(x => x.Value == dropdown.options[dropdown.value].text).Key.Remove(0, 11);
            preBinding = bindings.FirstOrDefault(x => x.Value == dropdown.options[dropdown.value].text).Key;
        }
        else
        {
            bindingText.text = " ";
            preBinding = " ";
        }

    }

    public void ShowBinding()//显示绑定切换
    {
        if (dropdown.value >= 0 && dropdown.value < dropdown.options.Count)
        {
            if (bindings["<Keyboard>/" + bindingDropdown.options[bindingDropdown.value].text] == " ")
            {
                bindings["<Keyboard>/" + bindingDropdown.options[bindingDropdown.value].text] = dropdown.options[dropdown.value].text;
                bindings[preBinding] = " ";
                if (dropdown.value >= 0 && dropdown.value <= 3)
                {
                    inputControl.FindAction("Move").ChangeBinding(dropdown.value + 1).WithPath("<Keyboard>/" + bindingDropdown.options[bindingDropdown.value].text);
                    playerAnimation.inputControl.FindAction("Move").ChangeBinding(dropdown.value + 1).WithPath("<Keyboard>/" + bindingDropdown.options[bindingDropdown.value].text);
                }
                if (dropdown.value > 3)
                {
                    inputControl.FindAction(dropdown.options[dropdown.value].text).ChangeBinding(0).WithPath("<Keyboard>/" + bindingDropdown.options[bindingDropdown.value].text);
                    playerAnimation.inputControl.FindAction(dropdown.options[dropdown.value].text).ChangeBinding(0).WithPath("<Keyboard>/" + bindingDropdown.options[bindingDropdown.value].text);
                }
                bindingText.text = bindingDropdown.options[bindingDropdown.value].text;
            }
            else
            {
                Debug.Log("Error");
                bindingText.text = bindings.FirstOrDefault(x => x.Value == dropdown.options[dropdown.value].text).Key.Remove(0, 11);
            }
        }
        else
        {
            Debug.Log("Error");
            bindingText.text = " ";
        }
    }

    public void Resetting()//重置
    {
        string[] str = textAsset.text.Split(',');
        foreach (string s in str)//遍历获取keycode枚举类型中的键值
        {
            bindings["<Keyboard>/" + s]=" ";
        }

        bindings["<Keyboard>/space"] = "ChangeWeapon";
        bindings["<Keyboard>/w"] = "Up";
        bindings["<Keyboard>/s"] = "Down";
        bindings["<Keyboard>/a"] = "Left";
        bindings["<Keyboard>/d"] = "Right";
        bindings["<Keyboard>/l"] = "Dash";

        inputControl.FindAction("Move").ChangeBinding(1).WithPath("<Keyboard>/w");
        inputControl.FindAction("Move").ChangeBinding(2).WithPath("<Keyboard>/a");
        inputControl.FindAction("Move").ChangeBinding(3).WithPath("<Keyboard>/s");
        inputControl.FindAction("Move").ChangeBinding(4).WithPath("<Keyboard>/d");
        inputControl.FindAction("Dash").ChangeBinding(0).WithPath("<Keyboard>/l");
        inputControl.FindAction("ChangeWeapon").ChangeBinding(0).WithPath("<Keyboard>/space");


        playerAnimation.inputControl.FindAction("Move").ChangeBinding(1).WithPath("<Keyboard>/w");
        playerAnimation.inputControl.FindAction("Move").ChangeBinding(2).WithPath("<Keyboard>/a");
        playerAnimation.inputControl.FindAction("Move").ChangeBinding(3).WithPath("<Keyboard>/s");
        playerAnimation.inputControl.FindAction("Move").ChangeBinding(4).WithPath("<Keyboard>/d");
        playerAnimation.inputControl.FindAction("Dash").ChangeBinding(0).WithPath("<Keyboard>/l");
        playerAnimation.inputControl.FindAction("ChangeWeapon").ChangeBinding(0).WithPath("<Keyboard>/space");

        dropdown.value = -1;

    }

   

}
