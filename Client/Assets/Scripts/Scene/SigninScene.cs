using CoreLibrary.Log;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SigninScene : MonoBehaviour
{
    void Start()
    {
        LogHandler.SetModule(new LogModule());
        Managers.Instance.UIManager.GetUI<UISignin>().Show();
    }
}
