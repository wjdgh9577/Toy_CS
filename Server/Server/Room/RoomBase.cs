﻿using CoreLibrary.Network;
using Server.Session;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Room;

public class RoomInfo
{
    /// <summary>
    /// Room의 고유식별자
    /// </summary>
    public int uniqueId;

    /// <summary>
    /// 같은 Type의 Room을 구분하는 식별자
    /// (ex. GameRoom_0, GameRoom_1, ...)
    /// </summary>
    public int id;

    /// <summary>
    /// 현재 접속자 수
    /// </summary>
    public int ccu;

    public RoomInfo(int uniqueId, int id)
    {
        this.uniqueId = uniqueId;
        this.id = id;
        this.ccu = 0;
    }

    public void Enter()
    {
        this.ccu += 1;
    }

    public void Leave()
    {
        this.ccu = Math.Max(this.ccu - 1, 0);
    }

    public override string ToString()
    {
        return $"Unique ID: {uniqueId}, ID: {id}";
    }
}

public abstract class RoomBase
{
    public RoomInfo Info { get; protected set; }

    public virtual void OnStart(RoomInfo info)
    {
        Info = info;
    }

    public virtual void OnUpdate() { }

    public virtual void OnDestroy() { }

    public virtual void OnEnter(GameSession session)
    {
        Info.Enter();
    }

    public virtual void OnLeave(GameSession session)
    {
        Info.Leave();
    }
}
