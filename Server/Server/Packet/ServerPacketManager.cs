using CoreLibrary.Network;
using Google.Protobuf;
using Google.Protobuf.Protocol;

public class PacketManager
{
	public static PacketManager Instance { get; private set; } = new PacketManager();

	PacketManager()
	{
		Register();
	}

	Dictionary<ushort, Action<SessionBase, ushort, ArraySegment<byte>>> _deserializers = new Dictionary<ushort, Action<SessionBase, ushort, ArraySegment<byte>>>();
    Dictionary<ushort, Action<SessionBase, IMessage>> _handlers = new Dictionary<ushort, Action<SessionBase, IMessage>>();
		
	public void Register()
	{		
		_deserializers.Add((ushort)MsgId.CPong, Deserialize<C_Pong>);
        _handlers.Add((ushort)MsgId.CPong, PacketHandler.HandleCPong);
	}

	public void HandlePacket(SessionBase session, ArraySegment<byte> buffer)
	{
		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + sizeof(ushort));

		if (_deserializers.TryGetValue(id, out var deserializer))
            deserializer.Invoke(session, id, buffer);
	}

	void Deserialize<T>(SessionBase session, ushort id, ArraySegment<byte> buffer) where T : IMessage, new()
	{
		T packet = new T();
        packet.MergeFrom(buffer.Array, buffer.Offset + sizeof(ushort) * 2, buffer.Count - sizeof(ushort) * 2);

        if (_handlers.TryGetValue(id, out var handler))
            handler.Invoke(session, packet);
	}
}