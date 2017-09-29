using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Configuration;
using Logger;
using LogProcessor;

namespace WebAPILogger
{
    public class WebAPILogger : ILogger
    {
        private static LogProcessor.ILogProcessor logProcessor;
        private static int clientid, applicationid;
        private static string logFilePath, webAPILogType;
        static WebAPILogger()
        {
            // initialize logProcessor
            clientid = Convert.ToInt32(ConfigurationManager.AppSettings["ClientId"]);
            applicationid = Convert.ToInt32(ConfigurationManager.AppSettings["ApplicationId"]);
            logFilePath = ConfigurationManager.AppSettings["LogFilePath"].ToString();
            webAPILogType = ConfigurationManager.AppSettings["WebAPILogType"].ToString();

            logProcessor = new WebAPILogProcessor.WebAPILogProcessor(clientid, applicationid, logFilePath);
        }

        public WebAPILogger()
        {
        }

        void ILogger.WriteLog(Logger.Common.LogType LogType, string Log)
        {
            if (webAPILogType.Contains(LogType.ToString()))
            {
                // write into file
                if (!Directory.Exists(logFilePath))
                {
                    Directory.CreateDirectory(logFilePath);
                }
                using (System.IO.StreamWriter webAPILoggerFile = new System.IO.StreamWriter(logFilePath + "\\LogFile_" + Guid.NewGuid().ToString() + ".txt", true))
                {
                    webAPILoggerFile.WriteLine("[" + DateTime.Now.ToString() + "]" + LogType.ToString() + ":" + "<" + Log + ">");
                }

                // if logProcessor.IsRunning is false then call ProcessLog
                if (!logProcessor.IsProcessorRunning())
                    logProcessor.ProcessLog();
            }
        }
    }
}
