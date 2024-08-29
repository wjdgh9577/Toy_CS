using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIWaitingRoom : UIBase
{
    [Header("Chat")]
    [SerializeField]
    Transform _chatItemRoot;
    [SerializeField]
    InputField _chatInputField;

    [Header("Map")]
    [SerializeField]
    Image _mapImage;
    [SerializeField]
    Text _mapInfoText;

    [Header("Players")]
    [SerializeField]
    Transform _playerItemRoot;

    WaitingRoomInfo _roomInfo;

    public void Show(WaitingRoomInfo roomInfo)
    {
        OnRefresh(roomInfo);

        Clear();

        Show();
    }

    void Clear()
    {
        for (int i = 0; i < _chatItemRoot.childCount; i++)
        {
            var chatItem = _chatItemRoot.GetChild(i).GetComponent<UIChatItem>();
            if (chatItem != null)
            {
                chatItem.Using = false;
                chatItem.SetActive(false);
            }
        }
    }

    public void OnRefresh(WaitingRoomInfo roomInfo)
    {
        _roomInfo = roomInfo;

        var list = Managers.Instance.UIManager.GetUIItems<UIPlayerItem>(8, _playerItemRoot);

        for (int i = 0; i < list.Count; i++)
        {
            list[i].Init(i < roomInfo.players.Count ? roomInfo.players[i] : null);
        }
    }

    public void OnRefresh(AccountInfo accountInfo)
    {
        var info = _roomInfo.players.Find(p => p.Uuid == accountInfo.Uuid);
        info.Ready = accountInfo.Ready;

        OnRefresh(_roomInfo);
    }

    public void Submit()
    {
        string inputText = _chatInputField.text;
        _chatInputField.text = "";
        OnChat(AccountInfo.Info, inputText);

        Managers.Instance.NetworkManager.Send(PacketHandler.C_Chat(inputText));
    }

    public void OnChat(AccountInfo info, string chat)
    {
        Managers.Instance.UIManager.GetUIItem<UIChatItem>(_chatItemRoot).Init(info.Name, chat);
    }

    public void ReadyButton()
    {
        var ready = AccountInfo.Info.Ready = !AccountInfo.Info.Ready;
        OnRefresh(AccountInfo.Info);
        Managers.Instance.NetworkManager.Send(PacketHandler.C_ReadyWaitingRoom(ready));
    }

    public void ExitButton()
    {
        if (!AccountInfo.Info.Ready)
            Managers.Instance.NetworkManager.Send(PacketHandler.C_LeaveWaitingRoom(_roomInfo.uniqueId));
    }
}
