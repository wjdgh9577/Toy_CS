using CoreLibrary.Log;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SigninScene : SceneBase
{
    void Start()
    {
        Screen.SetResolution(1920, 1080, false);
        
        LogHandler.SetModule(new LogModule());
        Managers.Instance.SceneManager.CurrentScene = this;
        Managers.Instance.UIManager.GetUI<UISignin>().Show();
    }
}
