using CoreLibrary.Network;
using Google.Protobuf;
using Google.Protobuf.Protocol;

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
		_deserializers.Add((ushort)MsgId.CPong, Deserialize<C_Pong>);
        _handlers.Add((ushort)MsgId.CPong, HandleCPong);
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
            handler.Invoke(session, packet);
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