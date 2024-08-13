using CoreLibrary.Log;
using CoreLibrary.Network;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class PacketHandler
{
    void HandleSPing(SessionBase session, IMessage message)
    {
        S_Ping packet = (S_Ping)message;
        LogHandler.Log(LogCode.CONSOLE, packet.Test);
    }
}
