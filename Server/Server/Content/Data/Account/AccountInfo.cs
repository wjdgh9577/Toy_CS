using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Content.Data;

public class AccountInfo
{
    public string Uuid { get; private set; }
    public string Name { get; private set; }
    public int Level { get; private set; }
    public bool Ready { get; set; }

    public AccountInfo(string uuid, string name, int level, bool ready = false)
    {
        this.Uuid = uuid;
        this.Name = name;
        this.Level = level;
        this.Ready = ready;
    }

    public Google.Protobuf.Protocol.AccountInfo GetProto()
    {
        var info = new Google.Protobuf.Protocol.AccountInfo();
        info.Uuid = Uuid;
        info.Name = Name;
        info.Level = Level;
        info.Ready = Ready;

        return info;
    }
}
