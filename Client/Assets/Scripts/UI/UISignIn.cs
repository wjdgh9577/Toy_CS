using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISignin : UIBase
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

        string id = _idInputField.text;
        string password = _passwordInputField.text;

        Managers.Instance.AuthenticationManager.Authenticate(id, password, OnAuthenticated);

        void OnAuthenticated(bool authenticated)
        {
            if (authenticated)
            {
                Managers.Instance.NetworkManager.Connect(OnConnected);
            }
            else
            {
                var popup = Managers.Instance.UIManager.GetUI<UIMessagePopup>();
                popup.Show(PopupType.Ok, "���Ե��� ���� �����Դϴ�.", null);
                _onProcess = false;
            }
        }

        void OnConnected(bool connected)
        {
            if (connected)
            {
                Managers.Instance.SceneManager.LoadSceneAsync(SceneName.LobbyScene, () =>
                {
                    _onProcess = false;
                    Hide();
                });
            }
            else
            {
                var popup = Managers.Instance.UIManager.GetUI<UIMessagePopup>();
                popup.Show(PopupType.YseNo, "������ ��õ� �Ͻðڽ��ϱ�?\n�ƴϿ� ���ý� ������ ����˴ϴ�.", () =>
                    Managers.Instance.NetworkManager.Connect(OnConnected), () =>
                    OnExit());
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
