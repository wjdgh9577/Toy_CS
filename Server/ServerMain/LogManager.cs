using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerCore;
using ServerCore.Log;

namespace ServerMain;

/// <summary>
/// LogHandler를 통해 접근
/// </summary>
public sealed class LogManager : LogBase
{
    public override void Log(LogImportance importance, LogCode code, params object[] args)
    {
        // TODO: DB 연동

        StringBuilder sb = new StringBuilder();

        var common = GetCommonMessage(importance, code);

        sb.Append($"{common.importance} / {common.code}");
        foreach (var arg in args)
            sb.Append($" / ({arg})");

        Console.WriteLine(sb.ToString());
    }

    public override void Log(LogImportance importance, LogCode code, params (object key, object value)[] args)
    {
        // TODO: DB 연동

        StringBuilder sb = new StringBuilder();

        var common = GetCommonMessage(importance, code);

        sb.Append($"{common.importance} / {common.code}");
        foreach (var arg in args)
            sb.Append($" / ({arg.key}, {arg.value})");

        Console.WriteLine(sb.ToString());
    }
}
