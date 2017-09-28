using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using LogProcessor;

namespace WebAPILogProcessor
{
    public class WebAPILogProcessor : ILogProcessor
    {
        public int ClientId { get; set; }

        public int ApplicationId { get; set; }

        public bool IsRunning;

        private System.Timers.Timer T1;

        public string LogFilePath { get; set; }
                
        public WebAPILogProcessor(int ClientId, int ApplicationId, string LogFilePath)
        {
            //this.ClientId = ClientId;
            // this.ApplicationId = ApplicationId;
            // this.LogFilePath = LogFilePath;
           // T1 = new System.Timers.Timer();
        }

        void ILogProcessor.ProcessLog()
        {
            // start send logentries with task tpl
        }

        public void SendLogEntries()
        {
            try
            {
                while (true)
                {

                    IsRunning = true;

                    // connect to web API - If failed Log in text, start timer and break loop, set IsRunning to false
                    // read log file - if empty start timer and break loop, Set IsRunning to False


                    // read each line
                    // seggregate Datetime, log type, payload and send over webAPI
                    // delete log line in file

                }
            }
            catch (Exception Ex)
            {
                // log ex in text file
                IsRunning = false;
            }

        }

 
    }
}
