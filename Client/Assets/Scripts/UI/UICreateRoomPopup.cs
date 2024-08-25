using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICreateRoomPopup : UIBase
{
    [SerializeField]
    GameObject _yseNoGroup;
    [SerializeField]
    GameObject _okGroup;

    [SerializeField]
    InputField _titleInput;
    [SerializeField]
    InputField _passwordInput;
    [SerializeField]
    Text _personnelText;

    int _personnel = 2;
    Action _onYes;
    Action _onNo;

    public void Show(PopupType type, Action onYes, Action onNo = null)
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

        _personnel = 2;
        _personnelText.text = _personnel.ToString();

        _onYes = onYes;
        _onNo = onNo;

        Show();
    }

    public void OnIncreaseButton()
    {
        _personnel = Math.Min(_personnel + 1, 8);
        _personnelText.text = _personnel.ToString();
    }

    public void OnDecreaseButton()
    {
        _personnel = Math.Max(_personnel - 1, 2);
        _personnelText.text = _personnel.ToString();
    }

    public void OnYesButton()
    {
        var title = _titleInput.text;
        var password = _passwordInput.text;
        var personnel = _personnel;

        Managers.Instance.NetworkManager.SendCCreateWaitingRoom(1, personnel, title, password);

        Hide();

        _onYes?.Invoke();
    }

    public void OnNoButton()
    {
        Hide();

        _onNo?.Invoke();
    }
}
