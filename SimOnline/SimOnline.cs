using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reactive.Linq;
using log4net;
using OPCClient;

namespace com.acs.sim.online
{
    // Online simulator
    public class SimOnline
    {
        public readonly ILog logger = LogManager.GetLogger(typeof(SimOnline));

        private IRealtimeDataCollector plantDataCollector = new PlantDataCollector();
       // private IRealtimeDataCollector plantDataCollector = new MockDataCollector();
        private ModelExecutor modelExecutor = new ModelExecutor();
        private ResultBuilder resultBuilder = new ResultBuilder();
      //  private IResultBuilder resultBuilder = new MockResultBuilder();
        private string caseFilePath;

        private Client4OPC opcClient;

        private List<string> aliveTags;

        private IDisposable subscription;

        public string CaseFilePath
        {
            set { this.caseFilePath = value; }
        }

        public SimOnline()
        {
            log4net.Config.XmlConfigurator.Configure();
            this.aliveTags = new List<string>();
        }

        public bool Configure(IList<string> inputTagNames, IList<InputBlockMap> ibm, IList<InputStreamMap> ism, IList<OutputBlockMap> obm, IList<OutputStreamMap> osm)
        {
            plantDataCollector.Configure(inputTagNames);
            modelExecutor.Configure(ibm, ism, obm, osm);
            resultBuilder.Configure(modelExecutor.OutputValues);

            return true;
        }

        public void ConfigAliveTag(IList<string> aliveTags)
        {
            this.aliveTags.Clear();
            foreach (string s in aliveTags)
            {
                //this.opcClient.RegisterSyncIOGroup(s);
                this.aliveTags.Add(s);
            }
        }

        public bool Open()
        {
            if (!plantDataCollector.Open())
            {
                logger.Debug("Connect to OPC Server Failure");
                return false;
            }
            else
            {
                 resultBuilder.OpcClient = plantDataCollector.OpcClient;
                 this.opcClient = plantDataCollector.OpcClient;
                 this.opcClient.OpcServer.ShutdownRequested += new OPCDA.NET.ShutdownRequestEventHandler(OpcServerShutdownRequested);
                 ReportAlive();
            }

            if (!modelExecutor.Open(this.caseFilePath))
                return false;

            if (!resultBuilder.Open())
                return false;

            return true;
        }

        public void Close()
        {
            resultBuilder.Close();
            modelExecutor.Close();
            plantDataCollector.Close();
           // this.subscription.Dispose();
        }

        // online simulation main work flow
        public void Run()
        {
            // React to timer event in 30 seconds interval, with 5 seconds delay
            //Observable.Timer(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5)).Subscribe(x =>
            //{
            //    DoWork(x);
            //});
            IObservable<long> source = Observable.Timer(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5));
            this.subscription = source.Subscribe(x =>
            {
                DoWork(x);
            });
        }

        public void DoWork(long x)
        {
            ReportAlive();

            // read tag values via OPC
            plantDataCollector.ReadTagValues();

            // map tag values to model variables
            modelExecutor.MapModelInputs(plantDataCollector.RawData);

            // execute model
            modelExecutor.Run();

            // build model outputs
            resultBuilder.WriteTagValues();

        }

        public bool ReportAlive()
        {
            foreach (string s in this.aliveTags)
            {
                this.opcClient.RegisterSyncIOGroup(s);
            }

            foreach (string s in this.aliveTags)
            {
                if (this.opcClient.Write(s, 1))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public void ReportShutdown()
        {
            foreach (string s in this.aliveTags)
            {
                if (this.opcClient.Write(s, 0))
                {
                }
            }

        }

        void OpcServerShutdownRequested(object sender, OPCDA.NET.ShutdownRequestEventArgs e)
        {
            foreach (string s in this.aliveTags)
            {
                this.opcClient.Write(s, 0);
            }
        }
    }
}
