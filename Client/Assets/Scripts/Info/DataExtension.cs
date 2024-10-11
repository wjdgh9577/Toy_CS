using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataExtension
{
    public static AccountInfo ToLocalData(this Google.Protobuf.Protocol.AccountInfo accountInfo)
    {
        return new AccountInfo(accountInfo);
    }

    public static List<AccountInfo> ToLocalData(this Google.Protobuf.Collections.RepeatedField<Google.Protobuf.Protocol.AccountInfo> accountInfos)
    {
        List<AccountInfo> list = new List<AccountInfo>();

        foreach (var info in accountInfos)
            list.Add(ToLocalData(info));

        return list;
    }

    public static WaitingRoomInfo ToLocalData(this Google.Protobuf.Protocol.WaitingRoomInfo waitingRoomInfo)
    {
        return new WaitingRoomInfo(waitingRoomInfo);
    }

    public static List<WaitingRoomInfo> ToLocalData(this Google.Protobuf.Collections.RepeatedField<Google.Protobuf.Protocol.WaitingRoomInfo> waitingRoomInfos)
    {
        List<WaitingRoomInfo> list = new List<WaitingRoomInfo>();

        foreach (var info in waitingRoomInfos)
            list.Add(ToLocalData(info));

        return list;
    }

    public static GameRoomInfo ToLocalData(this Google.Protobuf.Protocol.GameRoomInfo gameRoomInfo)
    {
        return new GameRoomInfo(gameRoomInfo);
    }

    public static List<GameRoomInfo> ToLocalData(this Google.Protobuf.Collections.RepeatedField<Google.Protobuf.Protocol.GameRoomInfo> gameRoomInfo)
    {
        List<GameRoomInfo> list = new List<GameRoomInfo>();

        foreach (var info in gameRoomInfo)
            list.Add(ToLocalData(info));

        return list;
    }

    public static WaitingRoomPlayerInfo ToLocalData(this Google.Protobuf.Protocol.WaitingRoomPlayerInfo waitingRoomPlayerInfo)
    {
        return new WaitingRoomPlayerInfo(waitingRoomPlayerInfo);
    }

    public static List<WaitingRoomPlayerInfo> ToLocalData(this Google.Protobuf.Collections.RepeatedField<Google.Protobuf.Protocol.WaitingRoomPlayerInfo> waitingRoomPlayerInfos)
    {
        List<WaitingRoomPlayerInfo> list = new List<WaitingRoomPlayerInfo>();

        foreach (var info in waitingRoomPlayerInfos)
            list.Add(ToLocalData(info));

        return list;
    }

    public static GameRoomPlayerInfo ToLocalData(this Google.Protobuf.Protocol.GameRoomPlayerInfo gameRoomPlayerInfo)
    {
        return new GameRoomPlayerInfo(gameRoomPlayerInfo);
    }

    public static List<GameRoomPlayerInfo> ToLocalData(this Google.Protobuf.Collections.RepeatedField<Google.Protobuf.Protocol.GameRoomPlayerInfo> gameRoomPlayerInfo)
    {
        List<GameRoomPlayerInfo> list = new List<GameRoomPlayerInfo>();

        foreach (var info in gameRoomPlayerInfo)
            list.Add(ToLocalData(info));

        return list;
    }
}
