using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountInfo
{
    public string Uuid { get; private set; }
    public string Name { get; private set; }
    public int Level { get; private set; }

    public AccountInfo(Google.Protobuf.Protocol.AccountInfo info)
    {
        this.Uuid = info.Uuid;
        this.Name = info.Name;
        this.Level = info.Level;
    }

    public Google.Protobuf.Protocol.AccountInfo GetProto()
    {
        var info = new Google.Protobuf.Protocol.AccountInfo()
        {
            Uuid = this.Uuid,
            Name = this.Name,
            Level = this.Level
        };

        return info;
    }

    public static AccountInfo Info
    {
        get
        {
            return Managers.Instance.GameManager.AccountInfo;
        }
    }
}
