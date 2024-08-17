using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLibrary.Log;
using CoreLibrary.Utility;

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
            string template = ConsoleMessageTemplate(type, code, args);

            Console.WriteLine(template.ToString());
        }
        else
        {
            // TODO: DB 연동
            ArgumentContainer container = new ArgumentContainer();
            for (int i = 0; i < args.Length; i++)
                container[$"key{i}"] = args[i].ToString();

#if DEBUG
            string template = ConsoleMessageTemplate(type, code, args);

            Console.WriteLine(template.ToString());
#endif
        }
    }

    public override void Log(LogType type, LogCode code, params (LogKey key, object value)[] args)
    {
        if (code == LogCode.CONSOLE)
        {
            string template = ConsoleMessageTemplate(type, code, args);

            Console.WriteLine(template.ToString());
        }
        else
        {
            // TODO: DB 연동
            ArgumentContainer container = new ArgumentContainer();
            foreach (var arg in args)
                container[arg.key.ToString()] = arg.value.ToString();

#if DEBUG
            string template = ConsoleMessageTemplate(type, code, args);

            Console.WriteLine(template.ToString());
#endif
        }
    }

    public override void Log(LogType type, LogCode code, params (string key, object value)[] args)
    {
        if (code == LogCode.CONSOLE)
        {
            string template = ConsoleMessageTemplate(type, code, args);

            Console.WriteLine(template.ToString());
        }
        else
        {
            // TODO: DB 연동
            ArgumentContainer container = new ArgumentContainer();
            foreach (var arg in args)
                container[arg.key.ToString()] = arg.value.ToString();

#if DEBUG
            string template = ConsoleMessageTemplate(type, code, args);

            Console.WriteLine(template.ToString());
#endif
        }
    }
}
