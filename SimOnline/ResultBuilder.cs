using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using OPCClient;

namespace com.acs.sim.online
{
    public class ResultBuilder : IResultBuilder
    {
        private IDictionary<string, double> outputValues;
        private Client4OPC client4OPC;

        public Client4OPC OpcClient
        {
            set { this.client4OPC = value; }
        }

        public ResultBuilder()
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
            try
            {
                foreach (KeyValuePair<string, double> kvp in this.outputValues)
                {
                    client4OPC.Write(kvp.Key, kvp.Value);
                    Console.WriteLine(kvp.Value);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("ResultBuilder.WriteTagValues exception: {0}", ex.Message);
                return false;
            }
        }
    }
}
