using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BindingChange : MonoBehaviour
{
    public PlayerSettings inputControl;

    private void Awake()
    {
        inputControl = new PlayerSettings();
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
        inputControl.GamePlay.Dash.ChangeBinding("Dash").WithPath("<Keyboard>/x");
    }
}
