using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvents : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnclickChangeScreenMode()
    {
        if (!Screen.fullScreen)
            Screen.fullScreen = true;
        else 
            Screen.fullScreen = false;
    }

    //[System.Obsolete]
    public void OnCilck_2560x1440()
    {
        if(Screen.fullScreen)
            Screen.SetResolution(2560,1440,true);
        else
            Screen.SetResolution(2560, 1440, false);
    }
    public void OnCilck_2560x1600()
    {
        if (Screen.fullScreen)
            Screen.SetResolution(2560, 1600, true);
        else
            Screen.SetResolution(2560, 1600, false);
    }
    public void OnCilck_1920x1080()
    {
        if (Screen.fullScreen)
            Screen.SetResolution(1920, 1080, true);
        else
            Screen.SetResolution(1920, 1080, false);
    }
    public void OnCilck_1600x900()
    {
        if (Screen.fullScreen)
            Screen.SetResolution(1600, 900, true);
        else
            Screen.SetResolution(1600, 900, false);
    }
}
