using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPasswordPopup : UIBase
{
    [SerializeField]
    GameObject _yseNoGroup;
    [SerializeField]
    GameObject _okGroup;

    [SerializeField]
    InputField _passwordInput;

    Action<string> _onYes;
    Action _onNo;

    public void Show(PopupType type, Action<string> onYes, Action onNo = null)
    {
        _yseNoGroup.SetActive(false);
        _okGroup.SetActive(false);

        switch (type)
        {
            case PopupType.YseNo:
                _yseNoGroup.SetActive(true);
                break;
            case PopupType.Ok:
                _okGroup.SetActive(true);
                break;
        }

        _onYes = onYes;
        _onNo = onNo;

        Show();
    }

    public void OnYesButton()
    {
        var password = _passwordInput.text;

        Hide();

        _onYes?.Invoke(password);
    }

    public void OnNoButton()
    {
        Hide();

        _onNo?.Invoke();
    }
}
