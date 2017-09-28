using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPILogger
{
    public class LogEntry
    {
        public int ClientId { get; set; }
        public int ApplicationId { get; set; }
        public int LogType { get; set; }
        public System.DateTime LogDateTime { get; set; }
        public List<LogPayload> LogPayloads { get; set; }
    }

    public class LogPayload
    {
        public string PayLoad { get; set; }
    }
}
