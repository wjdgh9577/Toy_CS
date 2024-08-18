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
#if DEBUG
            string template = ConsoleMessageTemplate(type, code, args);

            Console.WriteLine(template.ToString());
#endif
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

    public override void Log(LogType type, LogCode code, params (LogKey, object)[] args)
    {
        if (code == LogCode.CONSOLE)
        {
#if DEBUG
            string template = ConsoleMessageTemplate(type, code, args);

            Console.WriteLine(template.ToString());
#endif
        }
        else
        {
            // TODO: DB 연동
            ArgumentContainer container = new ArgumentContainer();
            foreach (var arg in args)
                container[arg.Item1.ToString()] = arg.Item2.ToString();

#if DEBUG
            string template = ConsoleMessageTemplate(type, code, args);

            Console.WriteLine(template.ToString());
#endif
        }
    }

    public override void Log(LogType type, LogCode code, params (string, object)[] args)
    {
        if (code == LogCode.CONSOLE)
        {
#if DEBUG
            string template = ConsoleMessageTemplate(type, code, args);

            Console.WriteLine(template.ToString());
#endif
        }
        else
        {
            // TODO: DB 연동
            ArgumentContainer container = new ArgumentContainer();
            foreach (var arg in args)
                container[arg.Item1.ToString()] = arg.Item2.ToString();

#if DEBUG
            string template = ConsoleMessageTemplate(type, code, args);

            Console.WriteLine(template.ToString());
#endif
        }
    }
}
