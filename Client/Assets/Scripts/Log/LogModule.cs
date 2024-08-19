using CoreLibrary.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public sealed class LogModule : LogModuleBase
{
    public override void Log(CoreLibrary.Log.LogType type, LogCode code, params object[] args)
    {
        if (code == LogCode.CONSOLE)
        {
#if UNITY_EDITOR
            string template = ConsoleMessageTemplate(type, code, args);
            
            Debug.Log(template.ToString());
#endif
        }
        else
        {
            // TODO: log 파일 출력
#if UNITY_EDITOR
            string template = ConsoleMessageTemplate(type, code, args);

            Debug.Log(template.ToString());
#endif
        }
    }

    public override void Log(CoreLibrary.Log.LogType type, LogCode code, params (LogKey key, object value)[] args)
    {
        if (code == LogCode.CONSOLE)
        {
#if UNITY_EDITOR
            string template = ConsoleMessageTemplate(type, code, args);

            Debug.Log(template.ToString());
#endif
        }
        else
        {
            // TODO: log 파일 출력
#if UNITY_EDITOR
            string template = ConsoleMessageTemplate(type, code, args);

            Debug.Log(template.ToString());
#endif
        }
    }

    public override void Log(CoreLibrary.Log.LogType type, LogCode code, params (string key, object value)[] args)
    {
        if (code == LogCode.CONSOLE)
        {
#if UNITY_EDITOR
            string template = ConsoleMessageTemplate(type, code, args);

            Debug.Log(template.ToString());
#endif
        }
        else
        {
            // TODO: log 파일 출력
#if UNITY_EDITOR
            string template = ConsoleMessageTemplate(type, code, args);

            Debug.Log(template.ToString());
#endif
        }
    }
}