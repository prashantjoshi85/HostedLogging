using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogProcessor
{
    public interface ILogProcessor
    {
        bool IsRunning { get; set; }
        void ProcessLog();
    }
}
