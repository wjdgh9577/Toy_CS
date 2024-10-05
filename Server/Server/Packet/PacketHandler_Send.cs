using Google.Protobuf.Protocol;
using Google.Protobuf.WellKnownTypes;
using Server.Content.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class PacketHandler
{
    public static S_Ping S_Ping(DateTime dateTime)
    {
        S_Ping packet = new S_Ping();
        packet.ServerTime = Timestamp.FromDateTime(dateTime);

        return packet;
    }

    public static S_Connected S_Connected()
    {
        S_Connected packet = new S_Connected();

        return packet;
    }

    public static S_AccountInfo S_AccountInfo(AccountInfo info)
    {
        S_AccountInfo packet = new S_AccountInfo();
        packet.Info = info;

        return packet;
    }

    public static S_EnterWaitingRoom S_EnterWaitingRoom(WaitingRoomInfo? roomInfo, bool enterOk)
    {
        S_EnterWaitingRoom packet = new S_EnterWaitingRoom();
        packet.RoomInfo = roomInfo;
        packet.EnterOk = enterOk;

        return packet;
    }

    public static S_LeaveWaitingRoom S_LeaveWaitingRoom(bool leaveOk)
    {
        S_LeaveWaitingRoom packet = new S_LeaveWaitingRoom();
        packet.LeaveOk = leaveOk;

        return packet;
    }

    public static S_RefreshWaitingRoom S_RefreshWaitingRoom(WaitingRoomInfo? roomInfo)
    {
        S_RefreshWaitingRoom packet = new S_RefreshWaitingRoom();
        packet.RoomInfo = roomInfo;

        return packet;
    }

    public static S_ReadyWaitingRoom S_ReadyWaitingRoom(AccountInfo accountInfo, bool ready)
    {
        S_ReadyWaitingRoom packet = new S_ReadyWaitingRoom();
        packet.Info = new WaitingRoomPlayerInfo();
        packet.Info.BaseInfo = new RoomPlayerInfo();
        packet.Info.BaseInfo.AccountInfo = accountInfo;
        packet.Info.Ready = ready;

        return packet;
    }

    public static S_RefreshLobby S_RefreshLobby(List<WaitingRoom> roomList)
    {
        S_RefreshLobby packet = new S_RefreshLobby();
        foreach (WaitingRoom room in roomList)
            packet.RoomInfos.Add(room.Info.GetProto());

        return packet;
    }

    public static S_Chat S_Chat(AccountInfo info, string chat)
    {
        S_Chat packet = new S_Chat();
        packet.Info = info;
        packet.Chat = chat;

        return packet;
    }
}
