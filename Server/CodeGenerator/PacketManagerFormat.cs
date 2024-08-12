using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator;

internal class PacketManagerFormat
{
    // {0} 패킷 등록
    public static string managerFormat =
@"using CoreLibrary.Network;
using Google.Protobuf;
using Google.Protobuf.Protocol;

public class PacketManager
{{
	public static PacketManager Instance {{ get; private set; }} = new PacketManager();

	PacketManager()
	{{
		Register();
	}}

	Dictionary<ushort, Action<SessionBase, ushort, ArraySegment<byte>>> _deserializers = new Dictionary<ushort, Action<SessionBase, ushort, ArraySegment<byte>>>();
    Dictionary<ushort, Action<SessionBase, IMessage>> _handlers = new Dictionary<ushort, Action<SessionBase, IMessage>>();
		
	public void Register()
	{{{0}
	}}

	public void HandlePacket(SessionBase session, ArraySegment<byte> buffer)
	{{
		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + sizeof(ushort));

		if (_deserializers.TryGetValue(id, out var deserializer))
            deserializer.Invoke(session, id, buffer);
	}}

	void Deserialize<T>(SessionBase session, ushort id, ArraySegment<byte> buffer) where T : IMessage, new()
	{{
		T packet = new T();
        packet.MergeFrom(buffer.Array, buffer.Offset + sizeof(ushort) * 2, buffer.Count - sizeof(ushort) * 2);

        if (_handlers.TryGetValue(id, out var handler))
            handler.Invoke(session, packet);
	}}

	public ArraySegment<byte> Serialize(IMessage message)
	{{
		string msgName = message.Descriptor.Name.Replace(""_"", string.Empty);
		MsgId msgId = (MsgId)Enum.Parse(typeof(MsgId), msgName);

        ushort size = (ushort)message.CalculateSize();
		ushort fullSize = (ushort)(size + sizeof(ushort) * 2);
        ushort id = (ushort)msgId;

		byte[] buffer = new byte[fullSize];
		Array.Copy(BitConverter.GetBytes(fullSize), 0, buffer, 0, sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(id), 0, buffer, sizeof(ushort), sizeof(ushort));
		Array.Copy(message.ToByteArray(), 0, buffer, sizeof(ushort) * 2, size);

		return new ArraySegment<byte>(buffer, 0, fullSize);
	}}
}}";

    // {0} MsgId
    // {1} 패킷 이름
    public static string managerRegisterFormat =
@"		
		_deserializers.Add((ushort)MsgId.{0}, Deserialize<{1}>);
        _handlers.Add((ushort)MsgId.{0}, PacketHandler.Handle{0});";

}
