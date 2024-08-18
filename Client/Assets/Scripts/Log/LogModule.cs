using CoreLibrary.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
            // TODO: log 파일 출력
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
#if DEBUG
            string template = ConsoleMessageTemplate(type, code, args);

            Console.WriteLine(template.ToString());
#endif
        }
        else
        {
            // TODO: log 파일 출력
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
#if DEBUG
            string template = ConsoleMessageTemplate(type, code, args);

            Console.WriteLine(template.ToString());
#endif
        }
        else
        {
            // TODO: log 파일 출력
#if DEBUG
            string template = ConsoleMessageTemplate(type, code, args);

            Console.WriteLine(template.ToString());
#endif
        }
    }
}