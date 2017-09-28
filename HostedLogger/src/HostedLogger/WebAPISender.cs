using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;

namespace HostedLogger
{
    public class WebAPISender : ILogSender
    {
        public int ClientId { get; set; }

        public int ApplicationId { get; set; }

        private System.Threading.Timer T1;

     
        public WebAPISender(int ClientId, int ApplicationId)
        {
            this.ClientId = ClientId;
            this.ApplicationId = ApplicationId;
            T1 = 
        }


        void ILogSender.ProcessLog()
        {
                        

        }

        public void SendLogEntries()
        {
            while(true)
            {
                // connect to web API - If failed Log in text, start timer and break loop

                // read log file - if empty start timer and break loop
            }
        }

    }
}
