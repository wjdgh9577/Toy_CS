using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProtocolExtension
{
    public static AccountInfo ToLocalData(this Google.Protobuf.Protocol.AccountInfo accountInfo)
    {
        return new AccountInfo(accountInfo);
    }

    public static List<AccountInfo> ToLocalData(this Google.Protobuf.Collections.RepeatedField<Google.Protobuf.Protocol.AccountInfo> accountInfos)
    {
        List <AccountInfo> list = new List <AccountInfo>();

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
}
