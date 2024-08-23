using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIPanel
{

}

public enum PopupType
{
    YseNo,
    Ok
}

public interface IUIPopup
{
    void Show(PopupType type, string message, Action onYes, Action onNo);
}
