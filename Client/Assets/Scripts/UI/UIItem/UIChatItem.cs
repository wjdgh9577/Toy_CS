using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChatItem : UIItemBase
{
    [SerializeField]
    Text _chatText;

    public void Init(string name, string chat)
    {
        _chatText.text = $"{name} : {chat}";

        SetActive(true);
    }
}
