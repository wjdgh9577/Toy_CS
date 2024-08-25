using CoreLibrary.Log;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : SceneBase
{
    void Start()
    {
        LogHandler.SetModule(new LogModule());
        Managers.Instance.SceneManager.CurrentScene = this;
    }
}
