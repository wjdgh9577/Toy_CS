using Google.Protobuf.Collections;
using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILobby : UIBase
{
    [SerializeField]
    Transform _roomItemRoot;

    public override void Show()
    {
        Clear();

        RefreshButton();

        base.Show();
    }

    void Clear()
    {
        for (int i = 0; i < _roomItemRoot.childCount; i++)
            _roomItemRoot.GetChild(i).gameObject.SetActive(false);
    }

    public void RefreshButton()
    {
        Managers.Instance.NetworkManager.Send(PacketHandler.C_RefreshWaitingRoom());
    }

    public void OnRefresh(RepeatedField<WaitingRoomInfo> roomInfos)
    {
        Clear();

        var items = Managers.Instance.UIManager.GetUIItems<UIRoomItem>(roomInfos.Count, _roomItemRoot);
        for (int i = 0;i < items.Count;i++)
        {
            items[i].Init(roomInfos[i]);
        }
    }

    public void QuickButton()
    {
        Managers.Instance.NetworkManager.Send(PacketHandler.C_QuickEnterWaitingRoom());
    }

    public void CreateButton()
    {
        var popup = Managers.Instance.UIManager.GetUI<UICreateRoomPopup>();
        popup.Show(PopupType.YseNo, null);
    }
}
