using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SigninPanel : MonoBehaviour
{
    [SerializeField]
    InputField _idInputField;
    [SerializeField]
    InputField _passwordInputField;

    bool _onProcess = false;

    public void OnSignin()
    {
        if (_onProcess)
            return;

        _onProcess = true;

        Managers.Instance.AuthenticationManager.Authenticate(_idInputField.text, _passwordInputField.text, OnAuthenticated);

        void OnAuthenticated(bool authenticated)
        {
            if (authenticated)
            {
                Managers.Instance.NetworkManager.Connect(OnConnected);
            }
            else
            {
                // TODO: 팝업창, 계정 생성
                _onProcess = false;
            }
        }

        void OnConnected(bool connected)
        {
            if (connected)
            {
                Managers.Instance.SceneManager.LoadSceneAsync("LobbyScene", () =>
                {
                    _onProcess = false;
                });
            }
            else
            {
                // TODO: 팝업창, 연결 재시도
                _onProcess = false;
            }
        }
    }

    public void OnSignup()
    {
        if (_onProcess)
            return;
    }

    public void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
