using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using OPCClient;
using log4net;

namespace com.acs.sim.online
{
    public class PlantDataCollector: IRealtimeDataCollector
    {
        public readonly ILog logger = LogManager.GetLogger(typeof(PlantDataCollector));
        private string serverName;
        private string userName;
        private string userPassword;
        private IDictionary<string, double> rawData = new Dictionary<string, double>();
        private Client4OPC client4OPC;

        public Client4OPC OpcClient
        {
            get { return this.client4OPC;  }
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

        public PlantDataCollector()
        {
        }

        public bool Configure(IList<string> tagNames)
        {
            // create placeholder for tags
            try
            {
                foreach (string name in tagNames)
                {
                    this.rawData.Add(name, 0.0);
                }
            }
            catch (Exception ex)
            {
                // tag name is null or tag name exists
                logger.Error(ex.Message);
                return false;
            }
            return true;

        }

        public bool Open()
        {
            this.client4OPC = new Client4OPC();

            bool connectSuccess = false;
            connectSuccess = client4OPC.Connect("Intellution.OPCiFIX.1");

            if (connectSuccess)
            {
                logger.Debug("Connect to OPC Server Success");
                // register tags to OPC 
                foreach (KeyValuePair<string, double> kvp in this.rawData.ToArray())
                {
                    client4OPC.RegisterSyncIOGroup(kvp.Key);
                }



                //OPC.DataChange += new DataChangeHandler(OPC_DataChange);
               // client4OPC.OpcServer.ShutdownRequested += new OPCDA.NET.ShutdownRequestEventHandler(OpcServer_ShutdownRequested);
            }
            return connectSuccess;
        }

        public void Close()
        {
        }

        public bool ReadTagValues()
        {
            try
            {
                foreach (KeyValuePair<string, double> kvp in this.rawData.ToArray())
                {
                    if (client4OPC.Read(kvp.Key))
                    {
                        this.rawData[kvp.Key] = double.Parse(client4OPC.Result);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                Console.Out.WriteLine("PlantDataCollector.ReadTagValues exception: {0}", e.Message);
                return false;
            }
        }

        void OpcServer_ShutdownRequested(object sender, OPCDA.NET.ShutdownRequestEventArgs e)
        {
            // Todo
            //Environment.Exit(0);
        }

    }
}
