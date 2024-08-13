using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLibrary.Log;

namespace Server;

/// <summary>
/// LogHandler를 통해 접근
/// </summary>
public sealed class LogModule : LogModuleBase
{
    public override void Log(LogType type, LogCode code, params object[] args)
    {
        if (code == LogCode.CONSOLE)
        {
            StringBuilder sb = new StringBuilder();

            var common = GetCommonMessage(type, code);

            sb.Append($"{common.importance}");
            foreach (var arg in args)
                sb.Append($" / ({arg})");

            Console.WriteLine(sb.ToString());
        }
        else
        {
            // TODO: DB 연동

            StringBuilder sb = new StringBuilder();

            var common = GetCommonMessage(type, code);

            sb.Append($"{common.importance} / {common.code}");
            foreach (var arg in args)
                sb.Append($" / ({arg})");

            Console.WriteLine(sb.ToString());
        }
    }

    public override void Log(LogType type, LogCode code, params (object key, object value)[] args)
    {
        if (code == LogCode.CONSOLE)
        {
            StringBuilder sb = new StringBuilder();

            var common = GetCommonMessage(type, code);

            sb.Append($"{common.importance}");
            foreach (var arg in args)
                sb.Append($" / ({arg.key}, {arg.value})");

            Console.WriteLine(sb.ToString());
        }
        else
        {
            // TODO: DB 연동

            StringBuilder sb = new StringBuilder();

            var common = GetCommonMessage(type, code);

            sb.Append($"{common.importance} / {common.code}");
            foreach (var arg in args)
                sb.Append($" / ({arg.key}, {arg.value})");

            Console.WriteLine(sb.ToString());
        }
    }
}
