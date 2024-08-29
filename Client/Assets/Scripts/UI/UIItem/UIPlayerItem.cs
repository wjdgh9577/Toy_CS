using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerItem : UIItemBase
{
    [SerializeField]
    Text _levelText;
    [SerializeField]
    Text _nameText;
    [SerializeField]
    GameObject _ready;

    public void Init(AccountInfo info)
    {
        if (info == null)
        {
            Clear();

            return;
        }
        _levelText.text = $"Lv. {info.Level}";
        _nameText.text = info.Name;
        _ready.SetActive(info.Ready);

        SetActive(true);
    }

    void Clear()
    {
        _levelText.text = "";
        _nameText.text = "";
        _ready.SetActive(false);
    }
}
