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

       // public bool IsRunning { get; set; }

        private System.Timers.Timer T1;

        public string LogFilePath { get; set; }

        public string APIUrl { get; set; }

        public bool IsRunning
        {
            get;


            set;
            
        }


        public WebAPILogProcessor(int ClientId, int ApplicationId, string LogFilePath)
        {
            this.ClientId = ClientId;
            this.ApplicationId = ApplicationId;
            this.LogFilePath = LogFilePath;
            T1 = new System.Timers.Timer();
            T1.Elapsed += T1_Elapsed;
            T1.Interval = 5 * 60 * 1000;
            T1.Enabled = false;
            APIUrl = ConfigurationManager.AppSettings["APIURL"].ToString();

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
            try
            {
                IsRunning = true;

                Task.Factory.StartNew(() =>
               {
                   SendLogEntries();
               });
            }
            catch(Exception ex)
            {
                IsRunning = false;
            }
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
                    DirectoryInfo info = new DirectoryInfo(LogFilePath);
                    FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
                    foreach (FileInfo file in files)
                    {
                        List<string> allLines = new List<string>();
                        //using (StreamReader fileReader = new StreamReader(file.FullName))
                        //{
                        if (File.Exists(file.FullName) == false) continue;
                        allLines = File.ReadAllLines(file.FullName).ToList();
                        if (allLines.Count == 0)
                        {
                            startTimer();
                            break;
                        }
                        else
                        {
                            foreach (string line in allLines)
                            {
                                if (line.Trim() == string.Empty) continue;
                                int EndIndex = line.IndexOf("]");
                                DateTime LogDateTime = Convert.ToDateTime(line.Substring(1, EndIndex - 1));
                                int ErrorIndex = line.IndexOf("<");
                                int LineLength = line.Length;
                                string LogType = line.Substring(EndIndex + 1, (ErrorIndex - (EndIndex + 2)));
                                string PayLoads = line.Substring(ErrorIndex + 1, (LineLength - (ErrorIndex + 2)));
                                Int32 LogTypeId = 0;

                                switch (LogType.ToUpper())
                                {
                                    case "ERROR":
                                        LogTypeId = (int)Common.LogType.Error;
                                        break;
                                    case "WARNING":
                                        LogTypeId = (int)Common.LogType.Warning;
                                        break;
                                    case "INFO":
                                        LogTypeId = (int)Common.LogType.Error;
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
                                        //var result = client.UploadString("http://10.31.24.53/LoggerAPI/api/LogEntries", JsonConvert.SerializeObject(lEntry));
                                        var result = client.UploadString(APIUrl, JsonConvert.SerializeObject(lEntry));
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
                                        
                                        break;
                                    }
                                }

                            }
                        }
                        //}
                        if (File.Exists(file.FullName))
                        {
                            File.Delete(file.FullName);
                        }
                    }

                    #region "DeletedCode1"

                    //String Parent = Directory.GetParent(LogFilePath).FullName;
                    //string LogReplicaPath = Parent + "\\LogReplica";
                    //if (!Directory.Exists(LogReplicaPath))
                    //{
                    //    Directory.CreateDirectory(LogReplicaPath);
                    //}

                    //string LogReplicaFileName = LogReplicaPath + "\\TempLogFile_" + DateTime.Now.ToString("yyyy-MM-ddHH:mm:ss") + ".txt";

                    //File.Copy(LogFilePath, LogReplicaFileName);
                    //using (StreamWriter writer = new StreamWriter(LogReplicaFileName))
                    //{
                    //    foreach (string fLine in allLines)
                    //    {
                    //        writer.WriteLine(fLine);
                    //    }
                    //}
                    //using (StreamWriter orgWriter = new StreamWriter(LogFilePath))
                    //{
                    //    orgWriter.Write("");
                    //}



                    //using (StreamWriter orgWriter = new StreamWriter(LogReplicaFileName))
                    //{
                    //    orgWriter.Write("");
                    //}
                    #endregion

                    //IsRunning = false;
                }
            }
            catch (Exception Ex)
            {
                // log ex in text file

                Task.Factory.StartNew(() =>
                {
                    WriteToText(Ex);
                });
               // IsRunning = false;
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

        bool ILogProcessor.IsProcessorRunning()
        {
            return IsRunning;
        }
    }
}
