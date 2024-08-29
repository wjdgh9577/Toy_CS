using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClient.Data;

public class AccountInfo
{
    public string Uuid { get; private set; }
    public string Name { get; private set; }
    public int Level { get; private set; }
    public bool Ready { get; private set; }

    public AccountInfo(Google.Protobuf.Protocol.AccountInfo info)
    {
        this.Uuid = info.Uuid;
        this.Name = info.Name;
        this.Level = info.Level;
        this.Ready = info.Ready;
    }
}
