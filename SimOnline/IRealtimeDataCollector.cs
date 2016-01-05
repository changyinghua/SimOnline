using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using OPCClient;

namespace com.acs.sim.online
{
    public interface IRealtimeDataCollector
    {
        Client4OPC OpcClient
        {
            get;  
        }

        string ServerName
        {
            get; set; 
        }

        string UserName
        {
            get; set; 
        }

        string UserPassword
        {
            get; set; 
        }

        IDictionary<string, double> RawData
        {
            get; 
        }

        bool Configure(IList<string> tagNames);
        bool Open();
        void Close();
        bool ReadTagValues();
    }
}
