using CoreLibrary.Network;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

public partial class PacketHandler
{
	static PacketHandler _instance { get; } = new PacketHandler();

	PacketHandler()
	{
		Register();
	}

	Dictionary<ushort, Action<SessionBase, string, ushort, ArraySegment<byte>>> _deserializers = new Dictionary<ushort, Action<SessionBase, string, ushort, ArraySegment<byte>>>();
    Dictionary<ushort, Action<SessionBase, IMessage>> _handlers = new Dictionary<ushort, Action<SessionBase, IMessage>>();
		
	void Register()
	{		
		_deserializers.Add((ushort)MsgId.CPing, Deserialize<C_Ping>);
        _handlers.Add((ushort)MsgId.CPing, HandleCPing);		
		_deserializers.Add((ushort)MsgId.CConnected, Deserialize<C_Connected>);
        _handlers.Add((ushort)MsgId.CConnected, HandleCConnected);		
		_deserializers.Add((ushort)MsgId.CEnterWaitingRoom, Deserialize<C_EnterWaitingRoom>);
        _handlers.Add((ushort)MsgId.CEnterWaitingRoom, HandleCEnterWaitingRoom);		
		_deserializers.Add((ushort)MsgId.CLeaveWaitingRoom, Deserialize<C_LeaveWaitingRoom>);
        _handlers.Add((ushort)MsgId.CLeaveWaitingRoom, HandleCLeaveWaitingRoom);		
		_deserializers.Add((ushort)MsgId.CReadyWaitingRoom, Deserialize<C_ReadyWaitingRoom>);
        _handlers.Add((ushort)MsgId.CReadyWaitingRoom, HandleCReadyWaitingRoom);		
		_deserializers.Add((ushort)MsgId.CRefreshLobby, Deserialize<C_RefreshLobby>);
        _handlers.Add((ushort)MsgId.CRefreshLobby, HandleCRefreshLobby);		
		_deserializers.Add((ushort)MsgId.CQuickEnterWaitingRoom, Deserialize<C_QuickEnterWaitingRoom>);
        _handlers.Add((ushort)MsgId.CQuickEnterWaitingRoom, HandleCQuickEnterWaitingRoom);		
		_deserializers.Add((ushort)MsgId.CCreateWaitingRoom, Deserialize<C_CreateWaitingRoom>);
        _handlers.Add((ushort)MsgId.CCreateWaitingRoom, HandleCCreateWaitingRoom);		
		_deserializers.Add((ushort)MsgId.CChat, Deserialize<C_Chat>);
        _handlers.Add((ushort)MsgId.CChat, HandleCChat);		
		_deserializers.Add((ushort)MsgId.CEnterGameRoom, Deserialize<C_EnterGameRoom>);
        _handlers.Add((ushort)MsgId.CEnterGameRoom, HandleCEnterGameRoom);		
		_deserializers.Add((ushort)MsgId.CSyncPlayer, Deserialize<C_SyncPlayer>);
        _handlers.Add((ushort)MsgId.CSyncPlayer, HandleCSyncPlayer);
	}

	public static void HandlePacket(SessionBase session, ArraySegment<byte> buffer)
	{
		int offset = 0;
		ushort fullSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
		offset += sizeof(ushort);
        ushort tokenSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + offset);
		offset += sizeof(ushort);
		string token = Encoding.UTF8.GetString(buffer.Array, buffer.Offset + offset, tokenSize);
		offset += tokenSize;
		ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + offset);
		offset += sizeof(ushort);

		if (_instance._deserializers.TryGetValue(id, out var deserializer))
            deserializer.Invoke(session, token, id, new ArraySegment<byte>(buffer.Array, buffer.Offset + offset, buffer.Count - offset));
	}

	void Deserialize<T>(SessionBase session, string token, ushort id, ArraySegment<byte> buffer) where T : IMessage, new()
	{
		T packet = new T();
        packet.MergeFrom(buffer.Array, buffer.Offset, buffer.Count);

        if (_handlers.TryGetValue(id, out var handler))
            HandleLogic(handler, session, token, packet);
	}

	public static ArraySegment<byte> Serialize(string token, IMessage message)
	{
		byte[] tokenBytes = Encoding.UTF8.GetBytes(token);
		ushort tokenSize = (ushort)token.Length;

		string msgName = message.Descriptor.Name.Replace("_", string.Empty);
		MsgId msgId = (MsgId)Enum.Parse(typeof(MsgId), msgName);

        ushort messageSize = (ushort)message.CalculateSize();
		ushort fullSize = (ushort)(tokenSize + messageSize + sizeof(ushort) * 3);
        ushort id = (ushort)msgId;

		byte[] buffer = new byte[fullSize];
		int offset = 0;
		Array.Copy(BitConverter.GetBytes(fullSize), 0, buffer, offset, sizeof(ushort));
		offset += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes(tokenSize), 0, buffer, offset, sizeof(ushort));
		offset += sizeof(ushort);
		Array.Copy(tokenBytes, 0, buffer, offset, tokenSize);
		offset += tokenSize;
		Array.Copy(BitConverter.GetBytes(id), 0, buffer, offset, sizeof(ushort));
		offset += sizeof(ushort);
		Array.Copy(message.ToByteArray(), 0, buffer, offset, messageSize);
		offset += messageSize;

		return new ArraySegment<byte>(buffer, 0, fullSize);
	}
}