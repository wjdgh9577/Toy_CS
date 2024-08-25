using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMessagePopup : UIBase
{
    [SerializeField]
    GameObject _yseNoGroup;
    [SerializeField]
    GameObject _okGroup;

    [SerializeField]
    Text _message;

    Action _onYes;
    Action _onNo;

    public void Show(PopupType type, string message, Action onYes, Action onNo = null)
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

        _message.text = message;

        _onYes = onYes;
        _onNo = onNo;

        Show();
    }

    public void OnYesButton()
    {
        Hide();

        _onYes?.Invoke();
    }

    public void OnNoButton()
    {
        Hide();

        _onNo?.Invoke();
    }
}
