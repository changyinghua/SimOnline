using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using OPCClient;

namespace com.acs.sim.online
{
    public interface IResultBuilder
    {
        Client4OPC OpcClient
        {
            set;
        }

        bool Configure(IDictionary<string, double> outputValues);
        bool Open();
        void Close();
        bool WriteTagValues();
    }
}
