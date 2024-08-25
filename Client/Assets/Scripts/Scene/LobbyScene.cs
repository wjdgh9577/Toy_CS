using CoreLibrary.Log;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : SceneBase
{
    void Start()
    {
        LogHandler.SetModule(new LogModule());
        Managers.Instance.SceneManager.CurrentScene = this;
        Managers.Instance.UIManager.GetUI<UILobby>().Show();
    }
}
