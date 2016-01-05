using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using OPCClient;

namespace com.acs.sim.online
{
    public class MockDataCollector: IRealtimeDataCollector
    {
        private string serverName;
        private string userName;
        private string userPassword;
        private IDictionary<string, double> rawData = new Dictionary<string, double>();
        private Random rg = new Random();

        public Client4OPC OpcClient
        {
            get { return null;  }
        }

        public string ServerName
        {
            get { return this.serverName; }
            set { this.serverName = value; }
        }

        public string UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
        }

        public string UserPassword
        {
            get { return this.userPassword; }
            set { this.userPassword = value; }
        }

        public IDictionary<string, double> RawData
        {
            get
            {
                return this.rawData;
            }
        }

        public MockDataCollector()
        {
        }

        public bool Configure(IList<string> tagNames)
        {
           foreach (string name in tagNames)
           {
               this.rawData.Add(name, 0.0);
           }
           return true;
        }

        public bool Open()
        {
            return true;
        }

        public void Close()
        {
        }

        public bool ReadTagValues()
        {
        	  // fill raw data
            IList<string> names = this.rawData.Keys.ToArray();
           foreach (string name in names)
           {
               this.rawData[name] = rg.Next(2,50);
           }
        	  
            return true;
        }


    }
}
