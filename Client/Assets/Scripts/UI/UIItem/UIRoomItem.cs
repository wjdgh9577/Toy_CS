using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomItem : UIItemBase
{
    [SerializeField]
    GameObject _passwordFlag;
    [SerializeField]
    Text _roomTitleText;
    [SerializeField]
    Text _personnelText;

    WaitingRoomInfo _info;

    public void Init(WaitingRoomInfo info)
    {
        _info = info;

        _passwordFlag.SetActive(info.password);
        _roomTitleText.text = info.title;
        _personnelText.text = $"{info.personnel}/{info.maxPersonnel}";

        SetActive(true);
    }

    public void OnClick()
    {
        if (_info.password)
        {
            var popup = Managers.Instance.UIManager.GetUI<UIPasswordPopup>();
            popup.Show(PopupType.YseNo, password =>
            {
                Enter(password); 
            });
            return;
        }

        Enter("");
    }

    void Enter(string password)
    {
        Managers.Instance.NetworkManager.Send(PacketHandler.C_EnterWaitingRoom(_info.uniqueId, password));
    }
}
