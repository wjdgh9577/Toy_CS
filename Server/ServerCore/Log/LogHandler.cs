using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore.Log;

public static class LogHandler
{
    static LogBase? Instance;

    public static void SetLogManager(LogBase manager) => Instance = manager;

    public static void Log(LogCode code, params object[] args) => Instance?.Log(LogImportance.NONE, code, args);
    public static void Log(LogCode code, params (object, object)[] args) => Instance?.Log(LogImportance.NONE, code, args);
    public static void LogWarning(LogCode code, params object[] args) => Instance?.Log(LogImportance.WARNING, code, args);
    public static void LogWarning(LogCode code, params (object, object)[] args) => Instance?.Log(LogImportance.WARNING, code, args);
    public static void LogError(LogCode code, params object[] args) => Instance?.Log(LogImportance.ERROR, code, args);
    public static void LogError(LogCode code, params (object, object)[] args) => Instance?.Log(LogImportance.ERROR, code, args);
}

public abstract class LogBase
{
    public abstract void Log(LogImportance importance, LogCode code, params object[] args);
    public abstract void Log(LogImportance importance, LogCode code, params (object key, object value)[] args);

    protected (string importance, string code) GetCommonMessage(LogImportance importance, LogCode code)
    {
        string importanceMessage = "";
        switch (importance)
        {
            case LogImportance.NONE:
                importanceMessage = "Level: 1";
                break;
            case LogImportance.WARNING:
                importanceMessage = "Level: 2";
                break;
            case LogImportance.ERROR:
                importanceMessage = "Level: 3";
                break;
        }

        string codeMessage = "";
        switch (code)
        {
            case LogCode.SOCKET_ERROR:
                codeMessage = "Socket Error!";
                break;
        }

        return (importanceMessage, codeMessage);
    }
}
