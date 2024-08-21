using CoreLibrary.Log;
using CoreLibrary.Network;
using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class Packet
{
    public SessionBase Session { get; set; }
    public IMessage Message { get; set; }
    public Action<SessionBase, IMessage> Handler {  get; set; }

    public void Handle()
    {
        Handler?.Invoke(Session, Message);
    }
}

public class PacketQueue
{
    Queue<Packet> _queue = new Queue<Packet>();

    object _lock = new object();

    public void Enqueue(Action<SessionBase, IMessage> handler, SessionBase session, IMessage message)
    {
        Packet packet = new Packet()
        {
            Session = session,
            Message = message,
            Handler = handler
        };

        lock (_lock)
        {
            _queue.Enqueue(packet);
        }
    }

    public Queue<Packet> DequeueAll()
    {
        lock (_lock)
        {
            Queue<Packet> ret = _queue;
            _queue = new Queue<Packet>();

            return ret;
        }
    }
}
