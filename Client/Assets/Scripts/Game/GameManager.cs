using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AccountInfo AccountInfo { get; private set; }

    public WaitingRoomInfo MyWaitingRoomInfo { get; private set; }
    public WaitingRoomPlayerInfo MyWaitingRoomPlayerInfo { get; private set; }

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
}
