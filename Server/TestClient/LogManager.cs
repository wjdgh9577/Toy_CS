using CoreLibrary.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClient;

public sealed class LogManager : LogBase
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
            // TODO: log 파일 출력

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
            // TODO: log 파일 출력

            StringBuilder sb = new StringBuilder();

            var common = GetCommonMessage(type, code);

            sb.Append($"{common.importance} / {common.code}");
            foreach (var arg in args)
                sb.Append($" / ({arg.key}, {arg.value})");

            Console.WriteLine(sb.ToString());
        }
    }
}
