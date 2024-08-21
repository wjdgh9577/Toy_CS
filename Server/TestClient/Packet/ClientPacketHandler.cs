using CoreLibrary.Network;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;

public partial class PacketHandler
{
	static PacketHandler _instance { get; } = new PacketHandler();

	PacketHandler()
	{
		Register();
	}

	Dictionary<ushort, Action<SessionBase, ushort, ArraySegment<byte>>> _deserializers = new Dictionary<ushort, Action<SessionBase, ushort, ArraySegment<byte>>>();
    Dictionary<ushort, Action<SessionBase, IMessage>> _handlers = new Dictionary<ushort, Action<SessionBase, IMessage>>();
		
	void Register()
	{		
		_deserializers.Add((ushort)MsgId.SPing, Deserialize<S_Ping>);
        _handlers.Add((ushort)MsgId.SPing, HandleSPing);		
		_deserializers.Add((ushort)MsgId.SConnected, Deserialize<S_Connected>);
        _handlers.Add((ushort)MsgId.SConnected, HandleSConnected);		
		_deserializers.Add((ushort)MsgId.SEnterRoom, Deserialize<S_EnterRoom>);
        _handlers.Add((ushort)MsgId.SEnterRoom, HandleSEnterRoom);		
		_deserializers.Add((ushort)MsgId.SLeaveRoom, Deserialize<S_LeaveRoom>);
        _handlers.Add((ushort)MsgId.SLeaveRoom, HandleSLeaveRoom);		
		_deserializers.Add((ushort)MsgId.SChat, Deserialize<S_Chat>);
        _handlers.Add((ushort)MsgId.SChat, HandleSChat);
	}

	public static void HandlePacket(SessionBase session, ArraySegment<byte> buffer)
	{
		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + sizeof(ushort));

		if (_instance._deserializers.TryGetValue(id, out var deserializer))
            deserializer.Invoke(session, id, buffer);
	}

	void Deserialize<T>(SessionBase session, ushort id, ArraySegment<byte> buffer) where T : IMessage, new()
	{
		T packet = new T();
        packet.MergeFrom(buffer.Array, buffer.Offset + sizeof(ushort) * 2, buffer.Count - sizeof(ushort) * 2);

        if (_handlers.TryGetValue(id, out var handler))
            HandleLogic(handler, session, packet);
	}

	public static ArraySegment<byte> Serialize(IMessage message)
	{
		string msgName = message.Descriptor.Name.Replace("_", string.Empty);
		MsgId msgId = (MsgId)Enum.Parse(typeof(MsgId), msgName);

        ushort size = (ushort)message.CalculateSize();
		ushort fullSize = (ushort)(size + sizeof(ushort) * 2);
        ushort id = (ushort)msgId;

		byte[] buffer = new byte[fullSize];
		Array.Copy(BitConverter.GetBytes(fullSize), 0, buffer, 0, sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(id), 0, buffer, sizeof(ushort), sizeof(ushort));
		Array.Copy(message.ToByteArray(), 0, buffer, sizeof(ushort) * 2, size);

		return new ArraySegment<byte>(buffer, 0, fullSize);
	}
}