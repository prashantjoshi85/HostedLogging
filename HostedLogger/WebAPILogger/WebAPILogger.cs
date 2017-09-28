using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger;
using WebAPILogProcessor;

namespace WebAPILogger
{
    public class WebAPILogger : ILogger
    {
        private static WebAPILogProcessor.WebAPILogProcessor logProcessor;

        static WebAPILogger()
        {
            // initialize logProcessor
        }

        public WebAPILogger()
        {
        }

        void ILogger.WriteLog(Common.LogType LogType, string Log)
        {
            // write into file



            // if logProcessor.IsRunning is false then call ProcessLog

        }
    }
}
