using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AccountInfo AccountInfo { get; private set; }

    public WaitingRoomInfo MyWaitingRoomInfo { get; private set; }
    public WaitingRoomPlayerInfo MyWaitingRoomPlayerInfo { get; private set; }

    public GameRoomInfo MyGameRoomInfo { get; private set; }
    public GameRoomPlayerInfo MyGameRoomPlayerInfo { get; private set; }

    private void Start()
    {
        Init();
    }

    void Init()
    {

    }

    public void SetAccountInfo(Google.Protobuf.Protocol.AccountInfo info)
    {
        AccountInfo = new AccountInfo(info);
    }

    public void SetWaitingRoomInfo(WaitingRoomInfo info)
    {
        MyWaitingRoomInfo = info;
        MyWaitingRoomPlayerInfo = info?.players.Find(p => p.accountInfo.Uuid == AccountInfo.Uuid);
    }

    public void SetGameRoomInfo(GameRoomInfo info)
    {
        MyGameRoomInfo = info;
        MyGameRoomPlayerInfo = info?.players.Find(p => p.accountInfo.Uuid == AccountInfo?.Uuid);
    }

    public void UpdatePlayers(List<GameRoomPlayerInfo> infos)
    {
        foreach (var info in infos)
        {
            if (info.accountInfo.Uuid == AccountInfo.Uuid)
            {
                if (!info.IsValid)
                    MyGameRoomPlayerInfo.Player.SetPosition(info.Position);
                continue;
            }

            var player = MyGameRoomInfo.players.Find(p => p.accountInfo.Uuid == info.accountInfo.Uuid).Player;
            player.SetPosition(info.Position);
        }
    }
}
