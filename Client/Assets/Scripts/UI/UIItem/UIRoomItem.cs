using Google.Protobuf.Protocol;
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

        _passwordFlag.SetActive(info.Password);
        _roomTitleText.text = info.Title;
        _personnelText.text = $"{info.BaseInfo.Personnel}/{info.BaseInfo.MaxPersonnel}";

        SetActive(true);
    }

    public void OnClick()
    {
        if (_info.Password)
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
        Managers.Instance.NetworkManager.Send(PacketHandler.C_EnterWaitingRoom(_info.BaseInfo.UniqueId, password));
    }
}
