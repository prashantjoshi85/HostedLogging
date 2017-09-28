using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostedLogger
{
    public interface ILogSender
    {
        void ProcessLog();
    }
}
