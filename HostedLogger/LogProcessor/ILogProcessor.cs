using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

namespace LogProcessor
{

    public interface ILogProcessor
    {
        bool IsRunning { get; set; }

        void ProcessLog();

        bool IsProcessorRunning();
        
    }
}
