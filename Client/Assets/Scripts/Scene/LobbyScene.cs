using CoreLibrary.Log;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    void Start()
    {
        LogHandler.SetModule(new LogModule());
        Managers.Instance.AuthenticationManager.Authenticate(authenticated =>
        {
            if (authenticated)
            {
                Managers.Instance.NetworkManager.Connect();
            }
            else
            {
                // TODO: 팝업창, 계정 생성
            }
        });
    }
}
