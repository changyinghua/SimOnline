using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using OPCClient;

namespace com.acs.sim.online
{
    public class MockResultBuilder : IResultBuilder
    {
        private IDictionary<string, double> outputValues;

        public Client4OPC OpcClient
        {
            set { ; }
        }

        public MockResultBuilder()
        {
        }

        public bool Configure(IDictionary<string, double> outputValues)
        {
            this.outputValues = outputValues;
            return true;
        }

        public bool Open()
        {
            return true;
        }

        public void Close()
        {
        }

        public bool WriteTagValues()
        {
            foreach (KeyValuePair<string, double> kvp in this.outputValues)
            {
                Console.Out.WriteLine("{0}={1}", kvp.Key, kvp.Value);
            }
            return true;
        }
    }
}
