using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore.Log;

public enum LogCode
{
    SOCKET_ERROR,
    EXCEPTION,
}

public enum LogImportance
{
    NONE,
    WARNING,
    ERROR,
}
