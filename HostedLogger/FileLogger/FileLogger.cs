using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger;

namespace FileLogger
{
    public class FileLogger : ILogger
    {
        void ILogger.WriteLog(Common.LogType LogType, string Log)
        {
            // Write into file
        }
    }
}
