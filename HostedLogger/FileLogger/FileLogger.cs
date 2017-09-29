using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger;
using System.Configuration;
using System.IO;

namespace FileLogger
{
    public class FileLogger : ILogger
    {
        private static int clientId, applicationId;
        private static string logFilePath, webAPILogType;

        public FileLogger()
        {
            clientId = Convert.ToInt32(ConfigurationManager.AppSettings["ClientId"]);
            applicationId = Convert.ToInt32(ConfigurationManager.AppSettings["ApplicationId"]);
            logFilePath = ConfigurationManager.AppSettings["LogFilePath"].ToString();
            webAPILogType = ConfigurationManager.AppSettings["WebAPILogType"].ToString();
        }

        void ILogger.WriteLog(Common.LogType LogType, string Log)
        {
            if (webAPILogType.Contains(LogType.ToString()))
            {
                // write into file
                if (!Directory.Exists(logFilePath))
                {
                    Directory.CreateDirectory(logFilePath);
                }

                //using (System.IO.StreamWriter loggerFile = new System.IO.StreamWriter(logFilePath + "\\LogFile_" + Guid.NewGuid().ToString() + ".txt", true))
                //{
                //    loggerFile.WriteLine("[" + DateTime.Now.ToString() + "]" + LogType.ToString() + ":" + "<" + Log + ">");
                //}
                System.IO.File.WriteAllText(logFilePath + "\\LogFile_" + Guid.NewGuid().ToString() + ".txt", "[" + DateTime.Now.ToString() + "]" + LogType.ToString() + ":" + "<" + Log + ">");
            }
        }
    }
}
