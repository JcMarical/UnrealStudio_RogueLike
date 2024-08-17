using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resolutionSetting : MonoBehaviour
{
    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);
    }

    public void SwitchRe1080()
    {
        Screen.SetResolution(1920, 1080, true);
        Debug.Log(1);
    }
    public void SwitchRe2560()
    {
        Screen.SetResolution(2560, 1600, true);
        Debug.Log(1);
    }
    public void SwitchRe800()
    {
        Screen.SetResolution(800, 800, false);
        Debug.Log(1);
    }
}
