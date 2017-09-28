using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using LogProcessor;
using System.Threading.Tasks;
using System.Net;
using WebAPILogger;
using Newtonsoft.Json;
using System.IO;
using System.Configuration;

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
            this.ClientId = ClientId;
            this.ApplicationId = ApplicationId;
            this.LogFilePath = LogFilePath;
            T1 = new System.Timers.Timer();
            T1.Elapsed += T1_Elapsed;
            T1.Interval = 5 * 60 * 1000;
            T1.Enabled = false;

        }

        private void T1_Elapsed(object sender, ElapsedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                SendLogEntries();
            });
            T1.Enabled = false;

        }

        void ILogProcessor.ProcessLog()
        {
            // start send logentries with task tpl
            Task.Factory.StartNew(() =>
           {
               SendLogEntries();
           });
        }

        public void SendLogEntries()
        {

            // connect to web API - If failed Log in text, start timer and break loop, set IsRunning to false

            // read log file - if empty start timer and break loop, Set IsRunning to False

            try
            {
                while (true)
                {

                    IsRunning = true;
                    using (StreamReader fileReader = new StreamReader(LogFilePath))
                    {
                        string line;
                        if (new FileInfo(LogFilePath).Length == 0)
                        {
                            startTimer();
                        }
                        else
                        {
                            while ((line = fileReader.ReadLine()) != null)
                            {
                                int EndIndex = line.IndexOf("]");
                                DateTime LogDateTime = Convert.ToDateTime(line.Substring(1, EndIndex - 1));
                                int ErrorIndex = line.IndexOf(":");
                                int LineLength = line.Length;
                                string LogType = line.Substring(EndIndex + 1, (ErrorIndex - (EndIndex + 1)));
                                string PayLoads = line.Substring(ErrorIndex + 1, (LineLength - (ErrorIndex + 1)));
                                Int32 LogTypeId = 0;

                                switch (LogType.ToUpper())
                                {
                                    case "ERROR":
                                        LogTypeId = Int32.Parse(Common.LogType.Error.ToString());
                                        break;
                                    case "WARNING":
                                        LogTypeId = Int32.Parse(Common.LogType.Warning.ToString());
                                        break;
                                    case "INFO":
                                        LogTypeId = Int32.Parse(Common.LogType.Error.ToString());
                                        break;
                                }


                                using (var client = new WebClient())
                                {
                                    LogEntry lEntry = new LogEntry();
                                    lEntry.ApplicationId = ApplicationId;
                                    lEntry.ClientId = ClientId;
                                    lEntry.LogDateTime = LogDateTime;
                                    lEntry.LogType = LogTypeId;
                                    lEntry.LogPayloads = new List<LogPayload>();
                                    lEntry.LogPayloads.Add(new LogPayload { PayLoad = PayLoads });
                                    try
                                    {
                                        client.Headers.Add("Content-Type:application/json");
                                        client.Headers.Add("Accept:application/json");
                                        var result = client.UploadString("http://10.31.24.53/LoggerAPI/api/LogEntries", JsonConvert.SerializeObject(lEntry));
                                    }
                                    catch (WebException ex)
                                    {
                                        //fileReader.Close();
                                        Task.Factory.StartNew(() =>
                                        {
                                            WriteToText(ex);
                                        });

                                        if (ex.Response is HttpWebResponse)
                                        {
                                            switch (((HttpWebResponse)ex.Response).StatusCode)
                                            {
                                                case HttpStatusCode.GatewayTimeout:
                                                    startTimer();
                                                    break;
                                                case HttpStatusCode.InternalServerError:
                                                    startTimer();
                                                    break;
                                                case HttpStatusCode.RequestTimeout:
                                                    startTimer();
                                                    break;
                                                case HttpStatusCode.ServiceUnavailable:
                                                    startTimer();
                                                    break;
                                                default:
                                                    throw ex;
                                            }
                                        }
                                        IsRunning = false;
                                        break;
                                    }
                                }

                            }

                            //file.Close();
                        }
                    }
                    // read each line
                    // seggregate Datetime, log type, payload and send over webAPI
                    // delete log line in file

                }
            }
            catch (Exception Ex)
            {
                // log ex in text file
                
                Task.Factory.StartNew(() =>
                {
                    WriteToText(Ex);
                });
                IsRunning = false;
            }

        }

        private void startTimer()
        {
            T1.Enabled = true;
        }

        private void WriteToText(Exception ex)
        {
            string logFilePath = ConfigurationManager.AppSettings["LogFilePath"].ToString();
            using (StreamWriter writer = File.AppendText(logFilePath + "\\LogProcessor.txt"))
            {
                writer.WriteLine(ex.ToString());
            }
        }

    }
}
