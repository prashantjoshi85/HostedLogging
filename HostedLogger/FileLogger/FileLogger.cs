using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger;
using System.Configuration;

namespace FileLogger
{
    public class FileLogger : ILogger
    {
        private static int clientId, applicationId;
        private static string logFilePath;

        public FileLogger()
        {
            clientId = Convert.ToInt32(ConfigurationManager.AppSettings["ClientId"]);
            applicationId = Convert.ToInt32(ConfigurationManager.AppSettings["ApplicationId"]);
            logFilePath = ConfigurationManager.AppSettings["LogFilePath"].ToString();
        }

        void ILogger.WriteLog(Common.LogType LogType, string Log)
        {
            // write into file
            System.IO.StreamWriter webAPILoggerFile = new System.IO.StreamWriter(logFilePath + "\\LogFile_" + Guid.NewGuid().ToString() + ".txt", true);

            loggerFile.WriteLine("[" + DateTime.Now.ToString() + "]" + LogType.ToString() + ":" + "<" + Log + ">");
        }
    }
}
