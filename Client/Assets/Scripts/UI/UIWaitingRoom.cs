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
        OnRefresh();

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

    public void OnRefresh()
    {
        _roomInfo = Managers.Instance.GameManager.MyWaitingRoomInfo;

        var list = Managers.Instance.UIManager.GetUIItems<UIPlayerItem>(8, _playerItemRoot);

        for (int i = 0; i < list.Count; i++)
        {
            list[i].Init(i < _roomInfo.players.Count ? _roomInfo.players[i] : null);
        }
    }

    public void OnRefresh(WaitingRoomPlayerInfo info)
    {
        var player = _roomInfo.players.Find(p => p.accountInfo.Uuid == info.accountInfo.Uuid);
        player.ready = info.ready;

        OnRefresh();
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
        var ready = Managers.Instance.GameManager.MyWaitingRoomPlayerInfo.ready = !Managers.Instance.GameManager.MyWaitingRoomPlayerInfo.ready;
        OnRefresh();
        Managers.Instance.NetworkManager.Send(PacketHandler.C_ReadyWaitingRoom(ready));
    }

    public void ExitButton()
    {
        if (!Managers.Instance.GameManager.MyWaitingRoomPlayerInfo.ready)
            Managers.Instance.NetworkManager.Send(PacketHandler.C_LeaveWaitingRoom(_roomInfo.uniqueId));
    }
}
